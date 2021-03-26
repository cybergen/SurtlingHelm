A Valheim mod stub project including build tools for debugging your code in Visual Studio and publishing a mod including BepInEx as zip files for Windows and Unix. There is no actual plugin content included, just a bare minimum plugin class. 

# Development Environment Setup

How to setup the development enviroment for this project.

1. Install [Visual Studio 2019](https://visualstudio.microsoft.com) and add the C# workload.
2. Download this package: https://mega.nz/file/hBM0BDBS#eAY26LfmSSVlIz2MZn0ox8WKspfKmeIssz7durp5SfM
3. Unpack into your Valheim root folder. You should now see a new folder called `<ValheimDir>\unstripped_corlib`.
4. [Compile](https://github.com/MrPurple6411/Bepinex-Tools) or [Download](https://mega.nz/file/oQxEjCJI#_XPXEjwLfv9zpcF2HRakYzepMwaUXflA9txxhx4tACA) Publicizers executable version. This will result in a new executable `AssemblyPublicizer.exe`.
5. Drag and drop all `assembly_*.dll` files from `<ValheimDir>\valheim_Data\Managed` onto `AssemblyPublicizer.exe`. You should get a new folder `<ValheimDir>\valheim_Data\Managed\publicized_assemblies` with all dragged dll files in it.
6. Clone this repository using git. That should create a new folder `ValheimModStub`.
7. Create a new user project file `ValheimModStub.csproj.user` alongside the project file within `<ValheimModStub>\ValheimModStub` to define some properties local to you. Paste this snippet and configure the paths as they are present on your computer.
```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="Current" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <!-- Needs to be your path to the base Valheim folder -->
    <ValheimDir>F:\Steam\steamapps\common\Valheim</ValheimDir>
  </PropertyGroup>
  <PropertyGroup Condition="'$(Configuration)|$(Platform)' == 'Debug|AnyCPU'">
    <StartAction>Program</StartAction>
    <!-- Start valheim.exe after building Debug-->
    <StartProgram>$(ValheimDir)\valheim.exe</StartProgram>
    <!-- If you want to connect to a server automatically, add '+connect <ip-address>:<port>' as StartArguments -->
    <StartArguments>+console</StartArguments>
  </PropertyGroup>
</Project>
```
8. Open the Solution file `<ValheimModStub>\ValheimModStub.sln`. It should prompt you a message at the top that some NuGet-Packages are missing. Click "Restore" and restart Visual Studio when finished.

# Build automations

Included in this repo are a PowerShell script `publish.ps1` and a folder `resources`. The script is referenced in the project file as a build event. Depending on the chosen configuration in Visual Studio the script executes the following actions.

## Building Debug

* The contents of `<ValheimModStub>\resources\Local` are copied to `<ValheimDir>`. You can put there whatever you want to copy to the game directoy alongside the plugin dll.
* The BepInEx core libraries are copied from `<ValheimModStub>\packages` and `<ValheimModStub>\libraries\BepInEx` to `<ValheimDir>\BepInEx\core`.
* The compiled dll file for this project is copied to `<ValheimDir>\BepInEx\plugins`.
* A .mdb file is generated for the compiled project dll and copied to `<ValheimDir>\BepInEx\plugins`.
* `<ValheimModStub>\libraries\Debug\mono-2.0-bdwgc.dll` is copied to `<ValheimDir>\MonoBleedingEdge\EmbedRuntime` replacing the original file (a backup is created before).

## Building Release

* The contents of `<ValheimModStub>\resources\Windows` and `<ValheimModStub>\resources\Unix` are copied to respective folders in `<ValheimModStub>\release`.
* BepInEx core is created in both folders
* The project dll is copied.
* Both folders are compressed into a zip file and then deleted.

# Debugging with dnSpy

Thanks to mono and unity-mono being open source, we patched and compiled our own mono runtime to enable actual live debugging of the game and the mod itself with dnSpy.

1. Download [dnSpy-net-win64](https://github.com/dnSpy/dnSpy/releases) and extract the exe.
2. Load all assemblies from `<ValheimDir>\unstripped_corlib` into dnSpy (just drag&drop the folder onto it).
3. Load all `assembly_*` from `<ValheimDir>\valheim_Data\Managed` into dnSpy (*do not load the publicized ones, they will not be loaded into the process and therefore can not be debugged*).
4. Load `ValheimModStub.dll` from `<ValheimDir>\BepInEx\plugins` into dnSpy.
5. Copy `<ValheimModStub>\libraries\Debug\mono-2.0-bdwgc.dll` into `<ValheimDir>\MonoBleedingEdge\EmbedRuntime` and overwrite the existing file.
6. Now go to `Debug` -> `Start Debugging` and select Unity debug engine. Select your valheim.exe as the executable and hit OK.
7. If you did set some breakpoints, the game will halt when it hits the breakpoint in memory and dnSpy will show you the objects in memory and much more useful stuff.

# Debugging with Visual Studio

Your own code can be debugged in source with Visual Studio itself. You cannot debug game disassemblies as with dnSpy, though.

1. Install Visual Studio Tools for Unity (can be done in Visual Studio installer via `Individual Components` -> `Visual Studio Tools for Unity`)
3. Build the project with target `Debug`. The publish.ps1 PowerShell script from this repo...
   * copies the generated mod .dll and .pdb to \<ValheimDir>\BepInEx\plugins after a successful build
   * automatically generates a ValheimModStub.dll.mdb file, which is needed for Unity/mono debugging. It should be in \<ValheimDir>\BepInEx\plugins, too.
   * copies the patched mono-2.0-bdwgc.dll from .\libraries\Debug to \<ValheimDir>\MonoBleedingEdge\EmbedRuntime and makes a backup copy of the original.
4. Start Valheim (either directly from Steam or hit F5 in Visual Studio when Steam is running)
5. Go to `Debug` -> `Attach Unity debugger`
6. Since the patched mono dll does not open the same port as Unity Dev builds, you have to click on `Input IP`. It should fill in your local IP automatically. you just have to change the port to `55555`and the debugger should connect.

# Actions after a game update

When Valheim updates it is likely that parts of the assembly files change. If this is the case, the references to the assembly files must be renewed in Visual Studio and Unity.

## Visual Studio actions

1. Make new versions of the publicized assemblies as described in [Development Environment Setup] step 4 and 5.
2. Go to VS and clean/build (or just rebuild) the project. The building process makes copies of the referenced dlls and stores them in the project folder. To get the newly created dlls it is important to grab the new publicized dlls from step 1.