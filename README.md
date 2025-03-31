# RepoCustomPhotonServer
[![GitHub](https://img.shields.io/badge/GitHub-RepoCustomPhotonServer-brightgreen?style=for-the-badge&logo=GitHub)](https://github.com/ArifRios1st/RepoCustomPhotonServer)
[![Thunderstore Version](https://img.shields.io/thunderstore/v/ArifRios1st/RepoCustomPhotonServer?style=for-the-badge&logo=thunderstore&logoColor=white)](https://thunderstore.io/c/repo/p/ArifRios1st/RepoCustomPhotonServer/)

**A Mod for redirecting custom photon R.E.P.O server.**


## Features
- **Change Photon Realtime Server.**
- **Change Photon Voice Server.**
- **Change Region.**
- **Change SteamAppId (to play with [OFM](https://online-fix.me) client)**

## Usage
1. Go to [Photon Engine](https://www.photonengine.com/).
2. Sign up or log in to your account.
3. Navigate to the [Photon Dashboard](https://dashboard.photonengine.com/).
4. On **Public Cloud**, click **CREATE A NEW APP**.
5. Select **Multiplayer Game** and select **Photon SDK** to **Realtime** for **Realtime Server** and/or **Voice** for **Voice Server**
6. Copy the **App ID** (**Realtime Server** and/or **Voice Server**) and use it in config file (`RepoCustomPhotonServer.cfg`).

## Config
- General
	- Enable
	- Change SteamAppId
- Photon
	- App id realtime (App ID for Realtime Server)
	- App id voice (App ID for Voice Server)
	- Region [More Info](https://doc.photonengine.com/voice/current/connection-and-authentication/regions#available-regions)

## Credits
- BepInEx-PhotonRedir ([Github](https://github.com/awc21/BepInEx-PhotonRedir))
- Nekogiri ([Github](https://github.com/KirigiriX/NekogiriFix)) ([ThunderStore](https://thunderstore.io/c/repo/p/kirigiri/Nekogiri/))
- REPOLib ([Github](https://github.com/ZehsTeam/REPOLib)) ([ThunderStore](https://thunderstore.io/c/repo/p/Zehs/REPOLib/))