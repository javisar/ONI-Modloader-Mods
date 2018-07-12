# ONI-Modloader Mods

Used with ONI-Modloader:
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
* AlternateOrdersMod: The Fabricators and Refineries will alternate between infinity orders. Made by [@javisar](https://github.com/javisar).
* BuildableAETNMod: Makes the AETN buildable and researchable. 20k Refined Metal. There is no preprint sprite. Made by [@javisar](https://github.com/javisar).
* CameraControllerMod: Enable further zoom-outs in play and dev mode (taken from [@Moonkis](https://github.com/Moonkis) Onion patcher, adapted by [@Killface1980](https://github.com/Killface1980)).
* CritterNumberSensor: Sensor for the critter number in a room (thanks to R9MX4 from Klei forum)
* CustomWorldMod: Enables the player to use custom world sizes. (Made by [@Moonkis](https://github.com/Moonkis), remade by [@Killface1980](https://github.com/Killface1980))
* FastModeMod: Duplicants will build and dig very fast. Made by [@javisar](https://github.com/javisar).
* GasTankMod: Storage for gases. Made by [@javisar](https://github.com/javisar).
* DisplayDraggedBoxSize: Shows selected rectangle dimensions using any tool. Made by [@fistak](https://github.com/fistak).
* InstantResearchMod: Forces instant research without Debug mode. Made by [@javisar](https://github.com/javisar).
* InsulatedDoorsMod: Doors can be constructed using any buildable element (ie: Abyssalite). Also it adds a new element Insulated Pressure Door. Made by [@javisar](https://github.com/javisar).
* InverseElectrolyzerMod: Combines hydrogen and oxygen into steam. Made by [@javisar](https://github.com/javisar).
* LiquidTankMod: Storage for liquids. Made by [@javisar](https://github.com/javisar).
* Patches (Do not use): Some incomplete tests.
* PressureDoorMod: Removes the energy need for the mechanized pressure door and makes it buildable from all material. Made by [@Killface1980](https://github.com/Killface1980).
* SensorsMod: Allows for increased ranges in automation sensors (Made by [@fistak](https://github.com/fistak), adapted by [@Killface1980](https://github.com/Killface1980)).
* SpeedControlMod: Overwrites the method SpeedControlScreen.OnChange. Fast Speed set to behave like Ultra Speed in debug mode. Made by [@javisar](https://github.com/javisar).
* StorageLockerMod: Storage lockers won't need a foundation to be built. Made by [@Killface1980](https://github.com/Killface1980).
* ONI-Common: Common code, required by these mods:
  * DraggablePanelMod: Makes panels draggable (Made by [@fistak](https://github.com/fistak), adapted by [@Killface1980](https://github.com/Killface1980)).
  * ImprovedGasColourMod: Replaces the oxygen overlay with gas overlay. Also visualizes the density (Made by [@fistak](https://github.com/fistak) and [@Killface1980](https://github.com/Killface1980)).
  * MaterialColor: Adds an overlay option to visualize what a building is made of (Made by [@fistak](https://github.com/fistak), adapted by [@Killface1980](https://github.com/Killface1980)).
  * OnionPatches: Custom world seeds. DebugHandler hook. Made by [@Moonkis](https://github.com/Moonkis), adapted by [@Killface1980](https://github.com/Killface1980)


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
