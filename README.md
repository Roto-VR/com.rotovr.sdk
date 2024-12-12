# Roto VR SDK

Our freely available Unity SDK unlocks additional features for genuine game-changing possibilities. Roto’s bi-directional motor functionality combined with bi-directional yaw tracking (of both the user's head and body), together will access almost every kind of control device, opening up exciting opportunities for creativity.

### Install from a Git URL
Yoy can also install this package via Git URL. To load a package from a Git URL:

* Open [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui.html) window.
* Click the add **+** button in the status bar.
* The options for adding packages appear.
* Select Add package from git URL from the add menu. A text box and an Add button appear.
* Enter the `https://github.com/Roto-VR/com.rotovr.sdk.git?path=/com.rotovr.sdk` Git URL in the text box and click Add.
* You may also install a specific package version by using the URL with the specified version.
  * `https://github.com/Roto-VR/com.rotovr.sdk.git#X.Y.X?path=/com.rotovr.sdk`
  * Please note that the version `X.Y.Z` stated here is to be replaced with the version you would like to get.
  * You can find all the available releases [here](https://github.com/Roto-VR/com.rotovr.sdk/releases).
  * The latest available release version is [![Last Release](https://img.shields.io/github/v/release/roto-vr/com.rotovr.sdk)](https://github.com/Roto-VR/com.rotovr.sdk/releases/latest)

For more information about what protocols Unity supports, see [Git URLs](https://docs.unity3d.com/Manual/upm-git.html).


# Integration
Follow the instructions below to start integration:

* [RotoBehaviour Settings](https://github.com/Roto-VR/com.rotovr.sdk/wiki/RotoBehaviour-Settings)
* [Implementation Instrcutions](https://github.com/Roto-VR/com.rotovr.sdk/wiki/Implementation-Instrcutions)
* [Meta Quest2 Setup](https://github.com/Roto-VR/com.rotovr.sdk/wiki/Meta-Quest-2-Setup)
* [Visualization in Unity Editor](https://github.com/Roto-VR/com.rotovr.sdk/wiki/Chair-Visualization-in-Unity-Editor)

# For Unreal and other platform users

For platforms other than Unity, such as Unreal Engine or any system that supports managed DLLs, you can integrate functionality by downloading the SDK as a managed DLL library. However, there are some important considerations:

* **Unreal's C++ Environment**: Unreal Engine primarily uses native C++ for scripting, so integrating a managed DLL (written in languages like C#) requires a bridging solution. This can involve using C++/CLI, Mono, or .NET Core to host the managed runtime within Unreal's ecosystem.
* **Managed DLL Integration**: Platforms that natively support the .NET runtime can use managed DLLs directly. For engines like Unreal, you'll likely need a wrapper to expose the managed DLL’s functionality as native C++.

[Download SDK as a managed DLL library](https://github.com/Roto-VR/com.rotovr.sdk/tree/master/Lib)
