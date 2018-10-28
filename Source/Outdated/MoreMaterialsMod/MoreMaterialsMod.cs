using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace MoreMaterialsMod
{
    public class MoreMaterialsMod
    {
		
		[HarmonyPatch(typeof(BuildingTemplates), "CreateBuildingDef")]
		internal class MoreMaterialsMod_BuildingTemplates_CreateBuildingDef
		{
			private static void Prefix(string id, ref string[] construction_materials)
			{
				//Debug.Log(" === MoreMaterialsMod_BuildingTemplates_CreateBuildingDef Prefix === ");

				switch (id) {

					//case AdvancedResearchCenterConfig.ID:
					//case AirborneCreatureLureConfig.ID:
					//case AirConditionerConfig.ID:
					case AirFilterConfig.ID:
					//case AlgaeDistilleryConfig.ID:
					//case AlgaeHabitatConfig.ID:
					//case ApothecaryConfig.ID:
					//case ArcadeMachineConfig.ID:
					case "Bed":
					//case BottleEmptierConfig.ID:
					//case BunkerDoorConfig.ID:
					//case BunkerTileConfig.ID:
					case CanvasConfig.ID:
					//case CeilingLightConfig.ID:
					//case CheckpointConfig.ID:
					//case ClothingFabricatorConfig.ID:
					//case CO2ScrubberConfig.ID:
					//case "CometDetector":
					//case CompostConfig.ID:
					//case CookingStationConfig.ID:
					//case CreatureDeliveryPointConfig.ID:
					//case CreatureFeederConfig.ID:
					//case CreatureTrapConfig.ID:
					case DiningTableConfig.ID:
					case DoorConfig.ID:
					//case EggCrackerConfig.ID:
					//case EggIncubatorConfig.ID:
					//case ElectrolyzerConfig.ID:
					//case EspressoMachineConfig.ID:
					case ExteriorWallConfig.ID:
					//case FacilityBackWallWindowConfig.ID:
					//case FarmStationConfig.ID:
					//case FarmTileConfig.ID:
					//case FertilizerMakerConfig.ID:
					case FirePoleConfig.ID:
					//case FishDeliveryPointConfig.ID:
					//case FishFeederConfig.ID:
					//case FishTrapConfig.ID:
					//case FloorLampConfig.ID:
					//case FloorSwitchConfig.ID:
					//case FlowerVaseConfig.ID:
					case FlushToiletConfig.ID:
					case GasConduitBridgeConfig.ID:
					case GasConduitConfig.ID:
					//case GasConduitOverflowConfig.ID:
					//case GasConduitPreferentialFlowConfig.ID:
					case GasConduitRadiantConfig.ID:
					case GasFilterConfig.ID:
					//case GasLogicValveConfig.ID:
					case GasMiniPumpConfig.ID:
					case GasPermeableMembraneConfig.ID:
					case GasPumpConfig.ID:
					case GasValveConfig.ID:
					case GasVentConfig.ID:
					//case GasVentHighPressureConfig.ID:
					//case GeneratorConfig.ID:
					//case GenericFabricatorConfig.ID:
					//case GlassForgeConfig.ID:
					//case GlassTileConfig.ID:
					case GraveConfig.ID:
					//case HandSanitizerConfig.ID:
					//case HydrogenGeneratorConfig.ID:
					//case HeadquartersConfig.ID:
					//case HydroponicFarmConfig.ID:
					//case IceSculptureConfig.ID:
					case InsulatedGasConduitConfig.ID:
					case InsulatedLiquidConduitConfig.ID:
					case InsulationTileConfig.ID:
					//case KilnConfig.ID:
					case LadderConfig.ID:
					//case LadderFastConfig.ID:
					//case LiquidConditionerConfig.ID:
					case LiquidConduitBridgeConfig.ID:
					case LiquidConduitConfig.ID:
					//case LiquidConduitOverflowConfig.ID:
					//case LiquidConduitPreferentialFlowConfig.ID:
					case LiquidConduitRadiantConfig.ID:
					//case LiquidCooledFanConfig.ID:
					case LiquidFilterConfig.ID:
					//case LiquidHeaterConfig.ID:
					//case LiquidLogicValveConfig.ID:
					case LiquidMiniPumpConfig.ID:
					case LiquidPumpConfig.ID:
					case LiquidPumpingStationConfig.ID:
					case LiquidValveConfig.ID:
					case LiquidVentConfig.ID:
					case "LogicDiseaseSensor":
					case "LogicElementSensorGas":
					case "LogicElementSensorLiquid":
					case "LogicMemory":
					case "LogicPowerRelay":
					case "LogicPressureSensorGas":
					case "LogicPressureSensorLiquid":
					case "LogicSwitch":
					case "LogicTemperatureSensor":
					case "LogicTimeOfDaySensor":
					case LogicWireBridgeConfig.ID:
					case LogicWireConfig.ID:
					//LuxuryBedcase "LuxuryBed":
					//case MachineShopConfig.ID:
					//case ManualGeneratorConfig.ID:
					case ManualPressureDoorConfig.ID:
					case MassageTableConfig.ID:
					//case MassiveHeatSinkConfig.ID:
					case MedicalBedConfig.ID:
					case MedicalCotConfig.ID:
					case MeshTileConfig.ID:
					//case MetalRefineryConfig.ID:
					//case MetalTileConfig.ID:
					//case MethaneGeneratorConfig.ID:
					//case MicrobeMusherConfig.ID:
					//case MineralDeoxidizerConfig.ID:
					//case OilRefineryConfig.ID:
					//case OilWellCapConfig.ID:
					//case OreScrubberConfig.ID:
					case OuthouseConfig.ID:
					//case PetroleumGeneratorConfig.ID:
					//case PhonoboxConfig.ID:
					//case PlanterBoxConfig.ID:
					//case PlasticTileConfig.ID:
					//case "POIBunkerExteriorDoor":
					//case "POIDoorInternal":
					//case POIFacilityDoorConfig.ID:
					//case PolymerizerConfig.ID:
					//case PowerControlStationConfig.ID:
					//case PowerTransformerConfig.ID:
					//case PowerTransformerSmallConfig.ID:
					case PressureDoorConfig.ID:
					//case "PressureSwitchGas":
					//case "PressureSwitchLiquid":
					//case RanchStationConfig.ID:
					//case RationBoxConfig.ID:
					//case RefrigeratorConfig.ID:
					//case ResearchCenterConfig.ID:
					//case RockCrusherConfig.ID:
					//case RoleStationConfig.ID:
					case SculptureConfig.ID:
					//case SeedSplicerConfig.ID:
					//case ShearingStationConfig.ID:
					case "Shower":
					//case SolarPanelConfig.ID:
					case SolidConduitBridgeConfig.ID:
					case SolidConduitConfig.ID:
					case SolidConduitInboxConfig.ID:
					case SolidConduitOutboxConfig.ID:
					case SolidTransferArmConfig.ID:
					//case SpaceHeaterConfig.ID:
					//case SteamTurbineConfig.ID:
					case StorageLockerConfig.ID:
					case StorageLockerSmartConfig.ID:
					//case SuitFabricatorConfig.ID:
					//case SuitLockerConfig.ID:
					//case SuitMarkerConfig.ID:
					//case "Switch":
					//case "TemperatureControlledSwitch":
					//case ThermalBlockConfig.ID:
					case TileConfig.ID:
					//case "TilePOI":
					//case TravelTubeConfig.ID:
					//case TravelTubeEntranceConfig.ID:
					//case TravelTubeWallBridgeConfig.ID:
					case WashBasinConfig.ID:
					case WashSinkConfig.ID:
					//case WaterCoolerConfig.ID:
					//case WaterPurifierConfig.ID:
					case WireBridgeConfig.ID:
					case WireBridgeHighWattageConfig.ID:

					//case BatteryConfig.ID:
					//case BatteryMediumConfig.ID:
					//case BatterySmartConfig.ID:

					case WireConfig.ID:
					case "HighWattageWire":
					//case WireRefinedConfig.ID:
					//case WireRefinedHighWattageConfig.ID:

					case "GasConduitDiseaseSensor":
					case "GasConduitElementSensor":
					case "GasConduitTemperatureSensor":
					case "LiquidConduitDiseaseSensor":
					case "LiquidConduitElementSensor":
					case "LiquidConduitTemperatureSensor":

					case LogicGateAndConfig.ID:
					case LogicGateOrConfig.ID:
					case LogicGateXorConfig.ID:
					case LogicGateNotConfig.ID:
					case LogicGateBufferConfig.ID:
					case LogicGateFilterConfig.ID:
						construction_materials = TUNING.MATERIALS.ANY_BUILDABLE;
						break;
				}

			}
		}

		/*
		[HarmonyPatch(typeof(ElementLoader), "ParseSolid", new Type[] { typeof(ElementLoader.SolidEntry) })]
		internal class MoreMaterialsMod_ElementLoader_ParseSolid
		{
			private static void Postfix()
			{
				Debug.Log(" === MoreMaterialsMod_ElementLoader_ParseSolid === ");

				Element ice = ElementLoader.FindElementByHash(SimHashes.Ice);

				ice.tag += " | BuildableAny";
			}

		}
		*/
	}
}
