using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Steamworks;

namespace RepoCustomPhotonServer;


[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
internal sealed class Plugin : BaseUnityPlugin
{
    internal static readonly ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_NAME);

    internal static ConfigEntry<bool> isActive;
    internal static ConfigEntry<bool> isChangeSteamAppId;
    internal static ConfigEntry<string> AppIdRealtime;
    internal static ConfigEntry<string> AppIdVoice;
    internal static ConfigEntry<string> FixedRegion;

    internal static AuthTicket steamAuthTicket;

    private void Awake()
    {
        logger.LogInfo(MyPluginInfo.PLUGIN_NAME + " Loaded !");


        isActive = Config.Bind("Settings", "Enable", false);
        isChangeSteamAppId = Config.Bind("Settings", "Change SteamAppId", false);
        AppIdRealtime = Config.Bind("Photon", "AppIdRealtime", "99515094-45b7-4c98-ab70-a448c548c83d", new ConfigDescription("Photon Realtime App ID", null, "HideFromREPOConfig"));
        AppIdVoice = Config.Bind("Photon", "AppIdVoice", "dfc40a01-c3a4-4395-9707-b71118816d2b", new ConfigDescription("Photon Voice App ID", null, "HideFromREPOConfig"));
        FixedRegion = Config.Bind("Photon", "Region", "", new ConfigDescription("Region", new AcceptableValueList<string>("", "asia", "au", "cae", "eu", "hk", "in", "jp", "za", "sa", "kr", "tr", "uae", "us", "usw", "ussc")));

        var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll();

    }

    private static string GetSteamAuthTicket(out AuthTicket ticket)
    {
        logger.LogInfo("Getting Steam Auth Ticket...");
        ticket = SteamUser.GetAuthSessionTicket();
        System.Text.StringBuilder stringBuilder = new();
        for (int i = 0; i < ticket.Data.Length; i++)
        {
            stringBuilder.AppendFormat("{0:x2}", ticket.Data[i]);
        }
        return stringBuilder.ToString();
    }

    private static void UpdatePhotonSettings()
    {
        ServerSettings serverSettings = FindObjectOfType<ServerSettings>() ?? Resources.Load<ServerSettings>("PhotonServerSettings");
        if (serverSettings != null)
        {
            serverSettings.AppSettings.AppIdRealtime = AppIdRealtime.Value ?? AppIdRealtime.DefaultValue.ToString();
            serverSettings.AppSettings.AppIdVoice = AppIdVoice.Value ?? AppIdVoice.DefaultValue.ToString();
            serverSettings.AppSettings.FixedRegion = FixedRegion.Value;

            logger.LogDebug($"AppIdRealtime : {serverSettings.AppSettings.AppIdRealtime}");
            logger.LogDebug($"AppIdVoice : {serverSettings.AppSettings.AppIdVoice}");
            logger.LogDebug($"Region : {serverSettings.AppSettings.FixedRegion}");

            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private static void UpdateAuthMethod()
    {
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AuthValues.UserId = SteamClient.SteamId.ToString();
        PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.None;

        string value = GetSteamAuthTicket(out steamAuthTicket);
        logger.LogDebug($"SteamAuthTicket: {value}");
        PhotonNetwork.AuthValues.AddAuthParameter("ticket", value);
    }


    [HarmonyPatch(typeof(NetworkConnect))]
    public class NetworkConnectPatch
    {
        [HarmonyPatch(nameof(NetworkConnect.Start))]
        [HarmonyPrefix]
        public static void Start()
        {
            if (isActive.Value)
            {
                logger.LogInfo("Updating Photon Settings");
                UpdatePhotonSettings();
            }
        }
    }

    [HarmonyPatch(typeof(SteamManager))]
    public class SteamManagerPatch
    {
        [HarmonyPatch(nameof(SteamManager.Awake))]
        [HarmonyPrefix]
        public static void Awake()
        {
            if (isChangeSteamAppId.Value)
            {
                logger.LogInfo($"Change appid to {480U}");
                SteamClient.Init(480U, true);
            }
        }

        [HarmonyPatch(nameof(SteamManager.SendSteamAuthTicket))]
        [HarmonyPrefix]
        public static bool SendSteamAuthTicket()
        {
            if (isActive.Value)
            {
                logger.LogInfo("Updating Auth Method");
                UpdateAuthMethod();
                return false;
            }

            return true;
        }
    }

}