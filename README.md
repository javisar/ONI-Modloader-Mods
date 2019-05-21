# ONI-Modloader Mods
Javisar's Oxygen Not Included Mods for **Steam Workshop** or [**ONI-Modloader**](https://github.com/javisar/ONI-Modloader)

[**Forums in Klei**](https://forums.kleientertainment.com/forums/topic/97444-mods-trevices-mods-lair/)


Disclaimers
-----------
* Please DON'T REPORT BUGS you encounter to Klei while mods are active unless they relate to the builtin ONI Modlioader
* BE AWARE that many of the mods are still a WIP and may fail. If you are having problems use a clean ONI installation and try to test the mods one by one to narrow the error. Then post a issue in github.
* **We do not take any responsibility for broken saves or any other damage. Use this software at your own risk.**
* If you load a savegame, it probably requires that you have exactly the same mods when you saved it.

This project uses source code of and is based on: [Harmony](https://github.com/pardeike/Harmony), [ModLoader Installer](https://github.com/zeobviouslyfakeacc/ModLoaderInstaller), [Besiege Modloader](https://github.com/spaar/besiege-modloader), [OnionPatcher](https://forums.kleientertainment.com/topic/81296-mod159-materialcolor-onionpatcher/)


**NOTE**: Compiled for **Q3-327401**

**[Report Mod Bugs](https://github.com/javisar/ONI-Modloader-Mods/issues/new/choose)**

**Contribute**: ANY PULL REQUEST IS WELCOME. Check the contributors [here](https://github.com/javisar/ONI-Modloader-Mods/graphs/contributors). 

There are a list of ideas and requested mods [here](https://github.com/javisar/ONI-Modloader/issues).


Mods Installation
-----------------
0. Prerequisites:
   * This mod installation guide ONLY applies to the mods below.
   * Make SURE you're using the latest version from Github main branch.
   * **[ONI-Modloader](https://github.com/javisar/ONI-Modloader#quick-start) must be installed ONLY if you DO NOT use ONI Builtin Modloader**
   * Make sure you deleted all previous mod files and its config:
     1. ONI-Modloader	
        * Windows: %PROGRAMFILES(X86)%\Steam\steamapps\common\OxygenNotIncluded\Mods\
        * Mac: /OxygenNotIncluded/OxygenNotIncluded.app/Contents/Resources/Mods/
     2. ONI Builtin Modloader (Steam)
        * Windows: %USERPROFILE%\Documents\Klei\OxygenNotIncluded\mods\local\
        * Mac: ???
1. Select the mod you want to install from [HERE](https://github.com/javisar/ONI-Modloader-Mods/tree/master/Mods).
2. Click "Clone or Download" and "Download ZIP" for the latest version as the releases may not be up to date.
3. Copy the desired mod folder [WITH THE SAME FOLDER STRUCTURE](https://github.com/javisar/ONI-Modloader-Mods/tree/master/.github/folders.png)** into the folder:
   1. ONI-Modloader	
      * Windows: %PROGRAMFILES(X86)%\Steam\steamapps\common\OxygenNotIncluded\Mods\
      * Mac: /OxygenNotIncluded/OxygenNotIncluded.app/Contents/Resources/Mods/
   2. ONI Builtin Modloader (Steam)
      * Windows: %USERPROFILE%\Documents\Klei\OxygenNotIncluded\mods\local\
      * Mac: ???
4. The main mod config files must be located in:
   1. ONI-Modloader
      * \Mods\\[MOD_NAME]\Config\\*.json
   2. ONI Builtin Modloader (Steam)
      * \mods\local\\[MOD_NAME]\Config\\*.json
5. **IMPORTANT: Copy also ONI-Common folder** since many mods use it as a support library.
6. Run the game.
7. **Check for error logs in**:
   * Windows: %USERPROFILE%\AppData\LocalLow\Klei\Oxygen Not Included\\**output_log.txt** 
   * Linux: ~/.config/unity3d/Klei/Oxygen Not Included/**Player.log**
   * MacOS: ~/Library/Logs/Unity/**Player.log**
   * \OxygenNotIncluded\Mods\Mod_Log.txt
   * \OxygenNotIncluded\Mods\\_Logs\


Mods
----
| Name  | Description | Steam | Contributors |
| ----- | ----------- | ----- | ------------ |
| Amphibious | Adds new duplicant traits: Amphibious (They also breath under water). EXPERIMENTAL. | [Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=1741246395) | [@javisar](https://github.com/javisar) |
| Better Mod Load Logs | Adds exception log in case of Harmony patch error at mod loading. | [Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=1744626595) | [@javisar](https://github.com/javisar) |
| BuildableAETN | Makes the AETN buildable and researchable. 20k Refined Metal. There is no preprint sprite. | [Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=1714094338) | [@javisar](https://github.com/javisar) |
| BuildOverFacilities | Allows to construct on top of Gravitas furniture. | [Steam]() | [@javisar](https://github.com/javisar) |
| BuildingModifier | Allows to modify building attributes. EXPERIMENTAL. More at: [HowTo](https://github.com/javisar/ONI-Modloader-Mods/blob/master/Mods/BuildingModifier/BuildingModifierHowto.txt) | [Steam]() | [@javisar](https://github.com/javisar) |
| CustomWorld | Enables the player to use custom world sizes. | [Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=1713687582) | [@Moonkis](https://github.com/Moonkis) [@javisar](https://github.com/javisar) |
| FluidPhysics | Overwrite some fluids molar mass to make them equal, this produces more mixing. EXPERIMENTAL. | [Steam]() | [@javisar](https://github.com/javisar) |
| FluidWarp | Teleports liquids and gases between places. More at: [HowTo](https://github.com/javisar/ONI-Modloader-Mods/blob/master/Mods/FluidWarp/FluidWarpModHowto.txt) | [Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=1741267647) | [@javisar](https://github.com/javisar) [@Blindfold](https://github.com/Blindfold) [@Moonkis](https://github.com/Moonkis) |
| Export Daily Reports | Export your colony daily reports to CSV format. EXPERIMENTAL. | [Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=1736659376) | [@javisar](https://github.com/javisar) |
| InstantResearch | Forces instant research without Debug mode. | [Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=1714091093) | [@javisar](https://github.com/javisar) |
| InverseElectrolyzer | Combines hydrogen and oxygen into water. | [Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=1742051024) | [@javisar](https://github.com/javisar) |
| NoDamage | Disables various damages in game. Overload, Overheat, boiling, cold, buildings. | [Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=1728703506) | [@javisar](https://github.com/javisar) |
| NoFixedTemps | The output fluid temperatures of the machinery depends on the input (Except AirFilter, AlgaeTerraium and PacuCleaner). | [Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=1742003542) | [@javisar](https://github.com/javisar) |
| RoomSize | Recognizes rooms (count cells) to a room size maximum of 1024. Configures maximum room sizes. | [Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=1715802131) | [@javisar](https://github.com/javisar) |
| SpeedControl | Overwrites the method SpeedControlScreen.OnChange. Fast Speed set to behave like Ultra Speed in debug mode. | [Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=1713359495) | [@javisar](https://github.com/javisar) |
| SuperMiner | Digging drops the complete mass of the cell. | [Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=1728728517) | [@javisar](https://github.com/javisar) |
| WorldGenReloaded | Changes geysers properties, frequency and allowed zones. EXPERIMENTAL. Don't use small world sizes! More at: [HowTo](https://github.com/javisar/ONI-Modloader-Mods/blob/master/Mods/WorldGenReloaded/WorldGenReloadedHowto.txt) | [Steam]() | [@javisar](https://github.com/javisar) |
| WorkableMuiltipliers | Duplicants will build and dig very fast. | [Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=1742986928) | [@javisar](https://github.com/javisar) |
| ZeroPointModule | A battery that gets unlimited energy from the vacuum. | [Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=1715786411) | [@javisar](https://github.com/javisar) |



[Outdated/Deprecated Mods](https://github.com/javisar/ONI-Modloader-Mods/blob/master/Outdated.md)


Downloads
---------
* Choose 'Download or Clone'.

