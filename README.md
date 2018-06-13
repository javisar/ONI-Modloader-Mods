# ONI-Modloader Mods

Example mods for ONI-Modloader:
https://github.com/javisar/ONI-Modloader

Forums in Klei:
https://forums.kleientertainment.com/topic/88186-mod01-oni-modloader/

This project uses source code of and is based on:
* https://github.com/zeobviouslyfakeacc/ModLoaderInstaller
* https://github.com/spaar/besiege-modloader
* https://github.com/pardeike/Harmony
* https://forums.kleientertainment.com/topic/81296-mod159-materialcolor-onionpatcher/


Examples
--------
* AlternateOrdersMod: The Fabricators and Refineries will alternate between infinity orders.
* CameraControllerMod: Enable further zoom-outs in play and dev mode (taken from Onion patcher).
* CritterNumberSensor: Sensor for the critter number in a room (thanks to R9MX4 from Klei forum)
* CustomWorldMod: Enables the player to user custom world sizes. (Remade by Killface)
* DraggablePanelMod: Makes panels draggable. REQUIRES ONI-Common.
* FastModeMod: Duplicants will build an dig very fast.
* ImprovedGasColourMod: Replaces the oxygen overly with gas colors. Also visualizes the density (taken from Onion patcher, modified). REQUIRES ONI-Common.
* InstantResearchMod: Forces instant research without Debug mode.
* InsulatedDoorsMod: Doors can be constructed using any buildable element (ie: Abyssalite). Also it adds a new element Insulated Pressure Door
* InverseElectrolyzerMod: Combines hydrogen and oxygen into steam.
* LiquidTankMod: Storage for liquids.
* MaterialColor: Adds an overlay option to visualize what a building is made of (taken from Onion patcher). REQUIRES ONI-Common.
* ONI-Common: Common code for Onion Patches and other mods.
* OnionPatches: Custom world seeds. DebugHandler hook. REQUIRES ONI-Common.
* Patches (Do not use): Some incomplete tests.
* PressureDoorMod: Removes the energy need for the mechanized pressure door and makes it buildable from all material.
* SensorsMod: It modifies some ranges y automation sensors (taken from Etiam's MateralColor mod pack).
* SpeedControlMod: Overwrites the method SpeedControlScreen.OnChange. Fast Speed set to behave like Ultra Speed in debug mode.
* StorageLockerMod: Storage lockers won't need a foundation to be built.


Requirements
------------
* .NET Framework 3.5
* Harmony Patcher
* Mono.Cecil
* Visual Studio 2015


Creating a Mod
--------------
1. Copy the following files from  ONI folder to the solution folder '\Source\lib\'
   * Assembly-CSharp.dll
   * Assembly-CSharp-firstpass.dll
   * Assembly-UnityScript-firstpass.dll
   * UnityEngine.dll
   * UnityEngine.UI.dll
2. Open the solution with Visual Studio.
3. Create a new mod or modify the 'Patches' project.
4. Compile it to generate the mod dll file.

Dlls will be recognized by the mod loader if 
• they reside in the main mod direcotory 
OR
• they are inside a subfolder inside a subfolder names 'Assemblies' (see MaterialColor mod)


Downloads
---------
Choose 'Clone or download'.
See Releases section.


Harmony Tutorials
-----------------
* https://github.com/pardeike/Harmony/wiki/
* https://github.com/roxxploxx/RimWorldModGuide/wiki/SHORTTUTORIAL:-Harmony
* https://github.com/UnlimitedHugs/RimworldHugsLib/wiki/Introduction-to-Patching
* https://github.com/UnlimitedHugs/RimworldHugsLib/wiki/Detouring
* https://oxygennotincluded.gamepedia.com/Guide/Working_with_the_Game_Files


Disclaimer
----------
We do not take any responsibility for broken saves or any other damage. Use this software at your own risk.
