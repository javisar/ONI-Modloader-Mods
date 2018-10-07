# ONI-Modloader Mods
Javisar's ONI mods for ONI-Modloader

[**For use with ONI-Modloader**](https://github.com/javisar/ONI-Modloader)

[**Forums in Klei**](https://forums.kleientertainment.com/topic/88186-mod01-oni-modloader/)


Disclaimers
----------
* Please DON'T REPORT BUGS you encounter to Klei while mods are active.
* BE AWARE that many of the mods are still a WIP and may fail. If you are having problems use a clean ONI installation and try to test the mods one by one to narrow the error. Then post a issue in github.
* We do not take any responsibility for broken saves or any other damage. Use this software at your own risk.
* If you load a savegame, it requires that you have exactly the same mods when you saved it.

This project uses source code of and is based on:
* https://github.com/zeobviouslyfakeacc/ModLoaderInstaller
* https://github.com/spaar/besiege-modloader
* https://github.com/pardeike/Harmony
* https://forums.kleientertainment.com/topic/81296-mod159-materialcolor-onionpatcher/


**NOTE**: Compiled for **RU-285450** and ONI-Modloader v0.4.8

**Report Bugs/Request features** for Mods here: https://github.com/javisar/ONI-Modloader-Mods/issues

**Contribute**: There are a list of ideas and requested mods here: https://github.com/javisar/ONI-Modloader-Mods/issues


Mods Installation
-----------------
0. Prerequisites:
   * Make SURE you're using the latest version from Github main branch.
   * [ONI-Modloader](https://github.com/javisar/ONI-Modloader#installation) must be installed
   * Make sure you deleted all previous mod files and its config:
     * \OxygenNotIncluded\Mods\
     * \OxygenNotIncluded\OxygenNotIncluded_Data\Managed\
1. Select the mod you want to install.
2. Click "Clone or Download" and "Download ZIP" for the current version as the releases may not be up to date.
3. Copy the mod **dll file** from "Mods" to the folder to ([Example of Mod folder structure](https://github.com/javisar/ONI-Modloader/blob/master/.github/modsfolder.png)):
   * \OxygenNotIncluded\Mods\
4. Copy the **mod configs files WITH THE SAME FOLDER STRUCTURE** from "Mods" to the folder to ([Example of Mod config structure](https://github.com/javisar/ONI-Modloader/blob/master/.github/modconfig.png)):
   * \OxygenNotIncluded\Mods\\[MOD_NAME]\Config\\[MOD_NAME]State.json
5. **Copy ONI-Common** (and configs) if the mod require it.
6. Run the game.
7. Check for error logs in:
   * \OxygenNotIncluded\Mods\Mod_Log.txt
   * \OxygenNotIncluded\Mods\\_Logs\
   * Windows: %USERPROFILE%\AppData\LocalLow\Klei\Oxygen Not Included\\**output_log.txt** 
   * Linux: ~/.config/unity3d/Klei/Oxygen Not Included/**Player.log**
   * MacOS: ~/Library/Logs/Unity/**Player.log**


ONI-Common
--------------------
| Name  | Description | ONI-Common | Creator |
| ----- | ----------- | ---------- | ------- |
| **ONI-Common** | Common code. Provides config load/save functionality, logger, help tools. | Y | [@fistak](https://github.com/fistak) [@Killface1980](https://github.com/Killface1980) [@javisar](https://github.com/javisar) |


Mods - New Buildings
--------------------
| Name  | Description | ONI-Common | Creator |
| ----- | ----------- | ---------- | ------- |
| BuildableAETN | Makes the AETN buildable and researchable. 20k Refined Metal. There is no preprint sprite. | N | [@javisar](https://github.com/javisar) |
| FluidWarp | Teleports liquids and gases between places. More at: [HowTo](https://github.com/javisar/ONI-Modloader-Mods/blob/master/Mods/FluidWarp/FluidWarpModHowto.txt) | N | [@javisar](https://github.com/javisar) |
| InverseElectrolyzer | Combines hydrogen and oxygen into steam. Uses two input conduits instead of getting oxygen from the environment. | N | [@javisar](https://github.com/javisar) |
| InverseElectrolyzerAlt | Combines hydrogen and oxygen into steam. Uses two input conduits instead of getting oxygen from the environment. | N | [@javisar](https://github.com/javisar) |
| SculpturesReloaded | Adds a new sculpture building that allows more materials. | N | [@javisar](https://github.com/javisar) |
| ZeroPointModule | A battery that gets unlimited energy from the vacuum. | N | [@javisar](https://github.com/javisar) |


Mods - GUI
--------------------
| Name  | Description | ONI-Common | Creator |
| ----- | ----------- | ---------- | ------- |
| CameraController | Enable further zoom-outs in play and dev mode. | N | [@Moonkis](https://github.com/Moonkis) [@Killface1980](https://github.com/Killface1980) |
| CustomWorld | Enables the player to use custom world sizes. | N | [@Moonkis](https://github.com/Moonkis) [@javisar](https://github.com/javisar) |
| DisplayDraggedBoxSize | Shows selected rectangle dimensions using any tool. | N | [@fistak](https://github.com/fistak) |
| ImprovedGasColour | Replaces the oxygen overlay with gas overlay. Also visualizes the density. | Y | [@fistak](https://github.com/fistak) [@Killface1980](https://github.com/Killface1980) [@javisar](https://github.com/javisar) |
| MaterialColor | Adds an overlay option to visualize what a building is made of. Modifies temperature overlay ranges and colors.  More at: [HowTo](https://github.com/javisar/ONI-Modloader-Mods/blob/master/Mods/MaterialColor/MaterialColorHowto.txt) | Y | [@fistak](https://github.com/fistak) [@Killface1980](https://github.com/Killface1980) [@javisar](https://github.com/javisar) |
| SpeedControl | Overwrites the method SpeedControlScreen.OnChange. Fast Speed set to behave like Ultra Speed in debug mode. | N | [@javisar](https://github.com/javisar) |


Mods - Mechanics
--------------------
| Name  | Description | ONI-Common | Creator |
| ----- | ----------- | ---------- | ------- |
| AllBuildingsDestroyable | Allows to construct on top of Gravitas furniture. | N | [@javisar](https://github.com/javisar) |
| AlternateOrders | The Fabricators and Refineries will alternate between infinity orders. | N | [@javisar](https://github.com/javisar) |
| BuildingModifier | Allows to modify building attributes. EXPERIMENTAL. More at: [HowTo](https://github.com/javisar/ONI-Modloader-Mods/blob/master/Mods/BuildingModifier/BuildingModifierHowto.txt) | Y | [@javisar](https://github.com/javisar) |
| FastMode | Duplicants will build and dig very fast. | N | [@javisar](https://github.com/javisar) |
| FluidPhysics | Overwrite some fluids molar mass to make them equal, this produces more mixing. EXPERIMENTAL. (Oxygen, Hydrogen, ChlorineGas, ContaminatedOxygen, Propane, Helium, Methane, CarbonDioxide, Water, DirtyWater, CrudeOil, Petroleum). | N | [@javisar](https://github.com/javisar) |
| InstantResearch | Forces instant research without Debug mode. | N | [@javisar](https://github.com/javisar) |
| MoreMaterials | Allows the construction of some buildings with any material (Doors, filters, bed, canvas, tables, wall fire pole, sculptures, shower, toilets, wash basins, bridges, conduits/wires, ladder, pumps, valves, ventS, tiles, lockers, sensors and gates). | N | [@javisar](https://github.com/javisar) |
| NaphthaViscosity | Recovers the old behaviour in Naphtha. Sets the viscosity to 0 and allows building vertical airlocks. | N | [@javisar](https://github.com/javisar) |
| NoDamage | Disables various damages in game. Overload, Overheat, boiling, cold, buildings. | Y | [@javisar](https://github.com/javisar) |
| NoFixedTemps | The output fluid temperatures of the machinery depends on the input (Except AirFilter, AlgaeTerraium and PacuCleaner). | N | [@javisar](https://github.com/javisar) |
| RoomSize | Recognizes rooms (count cells) to a room size maximum of 1024. Configures maximum room sizes. | Y | [@javisar](https://github.com/javisar) |
| SuperMiner | Digging drops the complete mass of the cell. | N | [@javisar](https://github.com/javisar) |
| WorldGenReloaded | Changes geysers properties, frequency and allowed zones. EXPERIMENTAL. More at: [HowTo](https://github.com/javisar/ONI-Modloader-Mods/blob/master/Mods/WorldGenReloaded/WorldGenReloadedHowto.txt) | Y | [@javisar](https://github.com/javisar) |


Outdated Mods
-------------
| Name  | Description | ONI-Common | Creator |
| ----- | ----------- | ---------- | ------- |
| CritterNumberSensor | Sensor for the critter number in a room (original from R9MX4). | N | [@Cairath](https://github.com/Cairath) |
| DraggablePanelMod | Makes panels draggable. | N | [@fistak](https://github.com/fistak) [@Killface1980](https://github.com/Killface1980) |
| GasTankMod | Storage for gases. | N | [@javisar](https://github.com/javisar) |
| InsulatedDoorsMod | Adds a new element Insulated Pressure Door. | N | [@javisar](https://github.com/javisar) |
| LiquidTankMod | Storage for liquids. | N | [@javisar](https://github.com/javisar) |
| NoOverloadedWiresMod | Avoid overloaded wires. Check NoDamageMod. | N | [@javisar](https://github.com/javisar) |
| Patches | (Do not use): Some incomplete tests and snippets. | N | [@javisar](https://github.com/javisar) |
| PressureDoor | Removes the energy need for the mechanized pressure door and makes it buildable from all material. | N | [@Killface1980](https://github.com/Killface1980) |
| Sensors | Allows for increased ranges in automation sensors. | N | [@fistak](https://github.com/fistak) [@Killface1980](https://github.com/Killface1980) |
| StorageLocker | Storage lockers won't need a foundation to be built. | N | [@Killface1980](https://github.com/Killface1980) |
| OnionPatches | Custom world seeds. DebugHandler hook. | N | [@Moonkis](https://github.com/Moonkis) [@Killface1980](https://github.com/Killface1980) |
| VentPressure | Allows to change the vents maximum pressure. | Y | [@javisar](https://github.com/javisar) |


Downloads
---------
* Choose 'Download or Clone'.
* Put the desired dlls into the Mods folder "OxygenNotIncluded\Mods".
* BE SURE to also copy **ALL** required config and icons folders.


