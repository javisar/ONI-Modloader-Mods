# ONI-Modloader Mods

To use with ONI-Modloader. More info:
https://github.com/javisar/ONI-Modloader

Forums in Klei:
https://forums.kleientertainment.com/topic/88186-mod01-oni-modloader/

If you want to **contribute**, there are a list of ideas and requested mods here: https://github.com/javisar/ONI-Modloader-Mods/issues

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


Examples
--------
* **Buildings**:
  * **BuildableAETNMod**: Makes the AETN buildable and researchable. 20k Refined Metal. There is no preprint sprite. Made by [@javisar](https://github.com/javisar).
  * **FluidWarpMod**: Teleports liquids and gases between places. Made by [@javisar](https://github.com/javisar).
  * **InverseElectrolyzerMod**: Combines hydrogen and oxygen into steam. Uses oxygen from the environment. Made by [@javisar](https://github.com/javisar).
  * **InverseElectrolyzerAltMod**: Combines hydrogen and oxygen into steam. Uses two input conduits instead of getting oxygen from the environment. Made by [@javisar](https://github.com/javisar).
  * **SculpturesReloadedMod**: Adds a new sculpture building that allows more materials. Made by [@javisar](https://github.com/javisar).

* **AllBuildingsDestroyable**: Allows to construct on top of Gravitas furniture. Made by [@javisar](https://github.com/javisar).
* **AlternateOrdersMod**: The Fabricators and Refineries will alternate between infinity orders. Made by [@javisar](https://github.com/javisar).
* **CameraControllerMod**: Enable further zoom-outs in play and dev mode (taken from [@Moonkis](https://github.com/Moonkis) Onion patcher, adapted by [@Killface1980](https://github.com/Killface1980)).
* **CustomWorldMod**: Enables the player to use custom world sizes. (Made by [@Moonkis](https://github.com/Moonkis), remade by [@Killface1980](https://github.com/Killface1980)), maintained by [@javisar](https://github.com/javisar).
* **DisplayDraggedBoxSize**: Shows selected rectangle dimensions using any tool. Made by [@fistak](https://github.com/fistak).
* **FastModeMod**: Duplicants will build and dig very fast. Made by [@javisar](https://github.com/javisar).
* **FluidPhysicsMod**: Overwrite some fluids molar mass to make them equal, this produces more mixing. Very Experimental. (Oxygen,Hydrogen,ChlorineGas,ContaminatedOxygen,Propane,Helium,Methane,CarbonDioxide,Water,DirtyWater,CrudeOil,Petroleum). Made by [@javisar](https://github.com/javisar).
* **InstantResearchMod**: Forces instant research without Debug mode. Made by [@javisar](https://github.com/javisar).
* **MoreMaterialsMod**: Allows the construction of some buildings with any material (Doors, filters, bed, canvas, tables, wall fire pole, sculptures, shower, toilets, wash basins, bridges, conduits/wires, ladder, pumps, valves, ventS, tiles, lockers, sensors and gates). Made by [@javisar](https://github.com/javisar).
* **NaphthaViscosityMod**: Recovers the old behaviour in Naphtha. Sets the viscosity to 0 and allows building vertical airlocks. Made by [@javisar](https://github.com/javisar).
* **NoFixedTemps**: The output fluid temperatures of the machinery depends on the input (Except AirFilter, AlgaeTerraium and PacuCleaner). Made by [@javisar](https://github.com/javisar).
* **PressureDoorMod**: Removes the energy need for the mechanized pressure door and makes it buildable from all material. Made by [@Killface1980](https://github.com/Killface1980).
* **SensorsMod**: Allows for increased ranges in automation sensors (Made by [@fistak](https://github.com/fistak), adapted by [@Killface1980](https://github.com/Killface1980)).
* **SpeedControlMod**: Overwrites the method SpeedControlScreen.OnChange. Fast Speed set to behave like Ultra Speed in debug mode. Made by [@javisar](https://github.com/javisar).
* **StorageLockerMod**: Storage lockers won't need a foundation to be built. Made by [@Killface1980](https://github.com/Killface1980).
* **SuperMinerMod**: Digging drops the complete mass of the cell. Made by [@javisar](https://github.com/javisar).
* **ZeroPointModuleMod**: A battery that gets unlimited energy from the vacuum. Made by [@javisar](https://github.com/javisar).
* **ONI-Common**: Common code. Provides config load/save functionality, logger, help tools. **Required by these mods**:
  * **ImprovedGasColourMod**: Replaces the oxygen overlay with gas overlay. Also visualizes the density (Made by [@fistak](https://github.com/fistak) and [@Killface1980](https://github.com/Killface1980)), maintained by [@javisar](https://github.com/javisar).
  * **MaterialColor**: Adds an overlay option to visualize what a building is made of (Made by [@fistak](https://github.com/fistak), adapted by [@Killface1980](https://github.com/Killface1980)), maintained by [@javisar](https://github.com/javisar).
  * **NoDamageMod**: Disables various damages in game. Overload, Overheat, boiling, cold, buildings.. Made by [@javisar](https://github.com/javisar).
  * **RoomSizeMod**:  Recognizes rooms (count cells) to a room size maximum of 1024. Configures maximum room sizes. Made by [@javisar](https://github.com/javisar).
  * **VentPressureMod**:  Allows to change the vents maximum pressure. Made by [@javisar](https://github.com/javisar).

Outdated mods:
* **CritterNumberSensor**: Sensor for the critter number in a room (original from R9MX4). Have a look to the new one from [@Cairath](https://github.com/Cairath) here. https://forums.kleientertainment.com/forums/topic/94120-mods-bunch-of-various-mods-usable-with-the-modloader/
* **DraggablePanelMod**: Makes panels draggable (Made by [@fistak](https://github.com/fistak), adapted by [@Killface1980](https://github.com/Killface1980)).
* **GasTankMod**: Storage for gases. Made by [@javisar](https://github.com/javisar).
* **InsulatedDoorsMod**: Adds a new element Insulated Pressure Door. Requires ONI-Common. Made by [@javisar](https://github.com/javisar).
* **LiquidTankMod**: Storage for liquids. Made by [@javisar](https://github.com/javisar).
* **NoOverloadedWiresMod**: Avoid overloaded wires. Check NoDamageMod. Made by [@javisar](https://github.com/javisar). 
* **Patches** (Do not use): Some incomplete tests and snippets.
* **OnionPatches**: Custom world seeds. DebugHandler hook. Requires ONI-Common. Made by [@Moonkis](https://github.com/Moonkis), adapted by [@Killface1980](https://github.com/Killface1980)



Downloads
---------
* Choose 'Download or Clone'.
* Put the desired dlls into the Mods folder "OxygenNotIncluded\Mods".
* BE SURE to also copy ALL required config and icons folders.


