# Roto VR SDK

Our freely available Unity SDK unlocks additional features for genuine game-changing possibilities. Roto’s bi-directional motor functionality combined with bi-directional yaw tracking (of both the user's head and body), together will access almost every kind of control device, opening up exciting opportunities for creativity.

Follow the instructions below to start integration:

* [Instalation](https://github.com/Roto-VR/com.rotovr.sdk/wiki/Instalation)
* **Setup**
  * [Connection to RotoVR Chair](https://github.com/Roto-VR/com.rotovr.sdk/wiki/Connection-to-RotoVR-Chair) 
  * [Meta Quest Setup](https://github.com/Roto-VR/com.rotovr.sdk/wiki/Connection-to-RotoVR-Chair#meta-quest-setup)
* **Integration**
  * [Using RotoBehaviour (Simplified Integration)](https://github.com/Roto-VR/com.rotovr.sdk/wiki/Integrating-the-Roto-SDK-Into-Your-Project#using-rotobehaviour-simplified-integration) 
  * [Using Roto API Directly (Advanced)](https://github.com/Roto-VR/com.rotovr.sdk/wiki/Integrating-the-Roto-SDK-Into-Your-Project#using-roto-api-directly--advanced)
* **Additional Guides**
  * [Editor Emulation Support](https://github.com/Roto-VR/com.rotovr.sdk/wiki/Editor-Emulation-Support) 
  * [For Unreal and other platform users](https://github.com/Roto-VR/com.rotovr.sdk/wiki#for-unreal-and-other-platform-users)
  * [Remarks](https://github.com/Roto-VR/com.rotovr.sdk/wiki#remarks)

---
## For Unreal and other platform users

For platforms other than Unity, such as Unreal Engine or any system that supports managed DLLs, you can integrate functionality by downloading the SDK as a managed DLL library. However, there are some important considerations:

* **Unreal's C++ Environment**: Unreal Engine primarily uses native C++ for scripting, so integrating a managed DLL (written in languages like C#) requires a bridging solution. This can involve using C++/CLI, Mono, or .NET Core to host the managed runtime within Unreal's ecosystem.
* **Managed DLL Integration**: Platforms that natively support the .NET runtime can use managed DLLs directly. For engines like Unreal, you'll likely need a wrapper to expose the managed DLL’s functionality as native C++.

[Download SDK as a managed DLL library](https://github.com/Roto-VR/com.rotovr.sdk/tree/master/Lib)

---
### Remarks
- **Preferred Connection:**
For maximum stability, use a USB connection on Windows 11.
(Note: Windows 10 may encounter issues maintaining a stable USB connection.)

- **Verifying USB Connection:**
Before launching your application, verify the USB connection using rotoVRCmd.exe.

- **Limitations with Bluetooth (BLE):**
The RotoVR SDK does not support multiple simultaneous BLE connections.
Ensure that only one RotoVR chair is active within the room to avoid connection issues.
