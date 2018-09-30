using Harmony;
using Klei.AI;
using ProcGen;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AllBuildingsDestroyableMod
{

    [HarmonyPatch(typeof(BuildingDef), "IsAreaClear")]
    internal class AllBuildingsDestroyableMod_BuildingDef_IsAreaClear
    {
        private static void Postfix(BuildingDef __instance, ref bool __result, GameObject source_go, int cell, Orientation orientation, ObjectLayer layer, ObjectLayer tile_layer, ref string fail_reason)
        {
            //Debug.Log(" === AllBuildingsDestroyableMod_BuildingDef_IsAreaClear Prefix === ");
            //Debug.Log(" === A "+__instance.PrefabID+" ===  ");
            //Debug.Log(" === A " + __instance.PrefabID + " ===  "+ fail_reason);
            bool flag = true;
            //fail_reason = null;
            switch (__instance.BuildLocationRule)
            {
                case BuildLocationRule.Conduit:
                case BuildLocationRule.LogicBridge:
                case BuildLocationRule.NotInTiles:
                    return;
                default:
                    break;
            }

            if (    fail_reason == null 
                ||  fail_reason != UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED)
            {
                return;
            }

            for (int i = 0; i < __instance.PlacementOffsets.Length; i++)
            {
                CellOffset offset = __instance.PlacementOffsets[i];
                CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(offset, orientation);
                int num = Grid.OffsetCell(cell, rotatedCellOffset);
                /*
                if (!Grid.IsValidBuildingCell(num))
                {
                    fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
                    flag = false;
                    break;
                }
                if (Grid.Element[num].id == SimHashes.Unobtanium)
                {
                    fail_reason = null;
                    flag = false;
                    break;
                }
                */
                GameObject gameObject = Grid.Objects[num, (int)layer];
                if ((UnityEngine.Object)gameObject != (UnityEngine.Object)null)
                {
                    //Debug.Log(gameObject.PrefabID().ToString());

                    if (!((UnityEngine.Object)gameObject.GetComponent<Wire>() == (UnityEngine.Object)null) && !((UnityEngine.Object)__instance.BuildingComplete.GetComponent<Wire>() == (UnityEngine.Object)null))
                    {
                        break;
                    }
                    //Debug.Log("Flag 1");
                    bool flag2 = false;
                    switch (gameObject.PrefabID().ToString())
                    {
                        case "PropClock":
                        case "PropDesk":
                        case "PropElevator":
                        case "PropFacilityChair":
                        case "PropFacilityChairFlip":
                        case "PropFacilityChandelier":
                        case "PropFacilityCouch":
                        case "PropFacilityDesk":
                        case "PropFacilityDisplay":
                        case "PropFacilityDisplay2":
                        case "PropFacilityDisplay3":
                        case "PropFacilityGlobeDroors":
                        case "PropFacilityHangingLight":
                        case "PropFacilityPainting":
                        case "PropFacilityStatue":
                        case "PropFacilityTable":
                        case "PropFacilityWallDegree":
                        case "PropLight":
                        case "PropReceptionDesk":
                        case "PropSkeleton":
                        case "PropSurfaceSatellite1":
                        case "PropSurfaceSatellite2":
                        case "PropSurfaceSatellite3":
                        case "PropTable":
                        case "PropTallPlant":
                            //__result = true;
                            //fail_reason = null;
                            flag2 = true;
                            break;
                        default:
                            //fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
                            //__result = false;
                            flag = false;
                            break;
                    }
                    //fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
                    //flag = false;
                    if (!flag2) break;
                }
                if (tile_layer != ObjectLayer.NumLayers && (UnityEngine.Object)Grid.Objects[num, (int)tile_layer] != (UnityEngine.Object)null && (UnityEngine.Object)Grid.Objects[num, (int)tile_layer].GetComponent<BuildingPreview>() == (UnityEngine.Object)null)
                {
                    //fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
                    //Debug.Log("Flag 2");
                    flag = false;
                    break;
                }
                /*
                if ((UnityEngine.Object)gameObject != (UnityEngine.Object)null)
                {
                    if (!((UnityEngine.Object)gameObject.GetComponent<Wire>() == (UnityEngine.Object)null) && !((UnityEngine.Object)__instance.BuildingComplete.GetComponent<Wire>() == (UnityEngine.Object)null))
                    {
                        break;
                    }
                    fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
                    flag = false;
                    break;
                }
                if (tile_layer != ObjectLayer.NumLayers && (UnityEngine.Object)Grid.Objects[num, (int)tile_layer] != (UnityEngine.Object)null && (UnityEngine.Object)Grid.Objects[num, (int)tile_layer].GetComponent<BuildingPreview>() == (UnityEngine.Object)null)
                {
                    fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
                    flag = false;
                    break;
                }
                */
                /*
                if ((UnityEngine.Object)gameObject != (UnityEngine.Object)null)
                {
                    Debug.Log(gameObject.PrefabID().ToString());

                    switch (gameObject.PrefabID().ToString())
                    {
                        case "PropClock":
                        case "PropDesk":
                        case "PropElevator":
                        case "PropFacilityChair":
                        case "PropFacilityChairFlip":
                        case "PropFacilityChandelier":
                        case "PropFacilityCouch":
                        case "PropFacilityDesk":
                        case "PropFacilityDisplay":
                        case "PropFacilityDisplay2":
                        case "PropFacilityDisplay3":
                        case "PropFacilityGlobeDroors":
                        case "PropFacilityHangingLight":
                        case "PropFacilityPainting":
                        case "PropFacilityStatue":
                        case "PropFacilityTable":
                        case "PropFacilityWallDegree":
                        case "PropLight":
                        case "PropReceptionDesk":
                        case "PropSkeleton":
                        case "PropSurfaceSatellite1":
                        case "PropSurfaceSatellite2":
                        case "PropSurfaceSatellite3":
                        case "PropTable":
                        case "PropTallPlant":
                            //__result = true;
                            //fail_reason = null;
                            break;
                        default:
                            //fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
                            //__result = false;
                            flag = false;
                            break;
                    }
                    if (!flag)
                        break;
                }
                
                if (tile_layer != ObjectLayer.NumLayers && (UnityEngine.Object)Grid.Objects[num, (int)tile_layer] != (UnityEngine.Object)null && (UnityEngine.Object)Grid.Objects[num, (int)tile_layer].GetComponent<BuildingPreview>() == (UnityEngine.Object)null)
                {
                    flag = false;
                    break;
                }
                */

                /*
                if ((UnityEngine.Object)gameObject != (UnityEngine.Object)null)
                {
                    if (!((UnityEngine.Object)gameObject.GetComponent<Wire>() == (UnityEngine.Object)null) && !((UnityEngine.Object)__instance.BuildingComplete.GetComponent<Wire>() == (UnityEngine.Object)null))
                    {
                        break;
                    }
                    fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
                    flag = false;
                    break;
                }
                if (tile_layer != ObjectLayer.NumLayers && (UnityEngine.Object)Grid.Objects[num, (int)tile_layer] != (UnityEngine.Object)null && (UnityEngine.Object)Grid.Objects[num, (int)tile_layer].GetComponent<BuildingPreview>() == (UnityEngine.Object)null)
                {
                    fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
                    flag = false;
                    break;
                }
                
                if (__instance.BuildLocationRule == BuildLocationRule.Tile)
                {
                    GameObject gameObject2 = Grid.Objects[num, 25];
                    if ((UnityEngine.Object)gameObject2 != (UnityEngine.Object)null && (UnityEngine.Object)gameObject2 != (UnityEngine.Object)source_go)
                    {
                        Building component = gameObject2.GetComponent<Building>();
                        if (component.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
                        {
                            fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRE_OBSTRUCTION;
                            flag = false;
                            break;
                        }
                    }
                    gameObject2 = Grid.Objects[cell, 2];
                    if ((UnityEngine.Object)gameObject2 != (UnityEngine.Object)null && (UnityEngine.Object)gameObject2 != (UnityEngine.Object)source_go)
                    {
                        Building component2 = gameObject2.GetComponent<Building>();
                        if ((UnityEngine.Object)component2 != (UnityEngine.Object)null && component2.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
                        {
                            fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRE_OBSTRUCTION;
                            flag = false;
                            break;
                        }
                    }
                }
                else if (__instance.BuildLocationRule == BuildLocationRule.OnFloorOverSpace && World.Instance.zoneRenderData.GetSubWorldZoneType(num) != SubWorld.ZoneType.Space)
                {
                    fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_SPACE;
                    flag = false;
                    break;
                }
                */
            }

            //Debug.Log("__result: "+ __result+ ", fail_reason:" + fail_reason+", flag: "+flag);
            
            if (flag)
            {
                __result = true;
                fail_reason = null;
            }
            
            //__result = flag && __instance.IsValidConduitLocation(source_go, cell, orientation, out fail_reason) && __instance.AreLogicPortsInValidPositions(source_go, cell, out fail_reason);            
        }

    

        /*
        private static void Postfix(BuildingDef __instance, ref bool __result, GameObject source_go, int cell, Orientation orientation, ObjectLayer layer, ObjectLayer tile_layer, ref string fail_reason)
        {
            //Debug.Log(" === AllBuildingsDestroyableMod_BuildingDef_IsAreaClear Postfix === ");
            //Debug.Log(" === A "+__instance.PrefabID+" ===  ");
            switch (__instance.BuildLocationRule)
            {
                case BuildLocationRule.Conduit:
                case BuildLocationRule.LogicBridge:
                case BuildLocationRule.NotInTiles:
                    return;
                default:
                    break;
            }

            for (int i = 0; i < __instance.PlacementOffsets.Length; i++)
            {
                CellOffset offset = __instance.PlacementOffsets[i];
                CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(offset, orientation);
                int num = Grid.OffsetCell(cell, rotatedCellOffset);

                if (!Grid.IsValidBuildingCell(num))
                {
                    fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
                    flag = false;
                    break;
                }
                if (Grid.Element[num].id == SimHashes.Unobtanium)
                {
                    fail_reason = null;
                    flag = false;
                    break;
                }

                GameObject gameObject = Grid.Objects[num, (int)layer];
                if ((UnityEngine.Object)gameObject != (UnityEngine.Object)null)
                {
                    //Debug.Log(" === B " + gameObject.PrefabID() + " ===  ");
                    //__instance.BuildLocationRule = BuildLocationRule.Anywhere;
                    switch (gameObject.PrefabID().ToString())
                    {
                        case "PropClock":
                        case "PropDesk":
                        case "PropElevator":
                        case "PropFacilityChair":
                        case "PropFacilityChairFlip":
                        case "PropFacilityChandelier":
                        case "PropFacilityCouch":
                        case "PropFacilityDesk":
                        case "PropFacilityDisplay":
                        case "PropFacilityDisplay2":
                        case "PropFacilityDisplay3":
                        case "PropFacilityGlobeDroors":
                        case "PropFacilityHangingLight":
                        case "PropFacilityPainting":
                        case "PropFacilityStatue":
                        case "PropFacilityTable":
                        case "PropFacilityWallDegree":
                        case "PropLight":
                        case "PropReceptionDesk":
                        case "PropSkeleton":
                        case "PropSurfaceSatellite1":
                        case "PropSurfaceSatellite2":
                        case "PropSurfaceSatellite3":
                        case "PropTable":
                        case "PropTallPlant":
                            __result = true;
                            fail_reason = null;
                            break;
                        default:
                            break;
                    }                  
                }

            }
            //__result = true;
            //fail_reason = null;
        }
        */
    }

	/*
    [HarmonyPatch(typeof(POIBunkerExteriorDoor), "CreateBuildingDef")]
    internal class Patches_POIBunkerExteriorDoor_CreateBuildingDef
    {
        private static void Postfix(ref BuildingDef __result)
        {
            //Debug.Log(" === POIBunkerExteriorDoor_CreateBuildingDef Postfix === ");
            __result.Invincible = false;
        }
    }

    [HarmonyPatch(typeof(POIDoorInternalConfig), "CreateBuildingDef")]
    internal class Patches_POIDoorInternalConfig_CreateBuildingDef
    {
        private static void Postfix(ref BuildingDef __result)
        {
            //Debug.Log(" === POIDoorInternalConfig_CreateBuildingDef Postfix === ");
            __result.Invincible = false;
        }
    }

    [HarmonyPatch(typeof(POIFacilityDoorConfig), "CreateBuildingDef")]
    internal class Patches_POIFacilityDoorConfig_CreateBuildingDef
    {
        private static void Postfix(ref BuildingDef __result)
        {
            //Debug.Log(" === POIFacilityDoorConfig_CreateBuildingDef Postfix === ");
            __result.Invincible = false;
        }
    }

    [HarmonyPatch(typeof(TilePOIConfig), "CreateBuildingDef")]
    internal class Patches_TilePOIConfig_CreateBuildingDef
    {
        private static void Postfix(ref BuildingDef __result)
        {
            //Debug.Log(" === TilePOIConfig_CreateBuildingDef Postfix === ");
            __result.Invincible = false;
        }
    }
	*/


    [HarmonyPatch(typeof(PropLightConfig), "CreatePrefab")]
    internal class AllBuildingsDestroyableMod_PropLightConfig_CreatePrefab
    {
        private static bool Prefix(PropLightConfig __instance, ref GameObject __result)
        {

            //Debug.Log(" === AllBuildingsDestroyableMod_PropLightConfig_CreatePrefab Prefix === ");
            
            return true;
        }

        private static void Postfix(ref GameObject __result)
        {
            /*
            OccupyArea occupyArea = __result.AddOrGet<OccupyArea>();
            occupyArea.DeleteObject();
            */
            /*
            OccupyArea occupyArea = __result.AddOrGet<OccupyArea>();
            occupyArea.objectLayers = new ObjectLayer[1]
            {
             ObjectLayer.Canvases
            };
            */
        }
        
    }
    
    
    /*
    [HarmonyPatch(typeof(Game), "OnPrefabInit")]
    internal class AllBuildingsDestroyableMod_Game_OnPrefabInit
    {

        private static void Postfix(Game __instance)
        {
            Debug.Log(" === AllBuildingsDestroyableMod_Game_OnPrefabInit Postfix === ");

        }
    }

    [HarmonyPatch(typeof(PropFacilityWallDegreeConfig), "CreatePrefab")]
    internal class AllBuildingsDestroyableMod_PropFacilityWallDegreeConfig_CreatePrefab
    {
        private static void Postfix(ref GameObject __result)
        {
            Debug.Log(" === AllBuildingsDestroyableMod_PropFacilityWallDegreeConfig_CreatePrefab Postfix === ");
            __result.AddWeapon(1f, 1f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0f);
        }
    }

    [HarmonyPatch(typeof(PropFacilityChairConfig), "CreatePrefab")]
    internal class AllBuildingsDestroyableMod_PropFacilityChairConfig_CreatePrefab
    {
        private static void Postfix(ref GameObject __result)
        {
            Debug.Log(" === AllBuildingsDestroyableMod_PropFacilityChairConfig_CreatePrefab Postfix === ");
            __result.AddWeapon(1f, 1f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0f);
        }
    }

    [HarmonyPatch(typeof(PropFacilityDeskConfig), "CreatePrefab")]
    internal class AllBuildingsDestroyableMod_PropFacilityDeskConfig_CreatePrefab
    {
        private static void Postfix(ref GameObject __result)
        {
            Debug.Log(" === AllBuildingsDestroyableMod_PPropFacilityDeskConfig_CreatePrefab Postfix === ");
            __result.AddWeapon(1f, 1f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0f);
        }
    }

    
    */
}
