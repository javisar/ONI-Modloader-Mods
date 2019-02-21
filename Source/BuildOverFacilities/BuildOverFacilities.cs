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

namespace BuildOverFacilities
{

    [HarmonyPatch(typeof(BuildingDef), "IsAreaClear")]
    internal class BuildOverFacilities_BuildingDef_IsAreaClear
    {
        private static void Postfix(BuildingDef __instance, ref bool __result, GameObject source_go, int cell, Orientation orientation, ObjectLayer layer, ObjectLayer tile_layer, ref string fail_reason)
        {           
            if (!Grid.IsValidCell(cell))
            {
                Debug.Log(" === BuildOverFacilities_BuildingDef_IsAreaClear: cell(cell) not valid = "+cell);
                return;
            }

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
                if (!Grid.IsValidCell(num))
                {
                    Debug.Log(" === BuildOverFacilities_BuildingDef_IsAreaClear: cell(num) not valid = " + num);
                    return;
                }

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
                
            }

            //Debug.Log("__result: "+ __result+ ", fail_reason:" + fail_reason+", flag: "+flag);
            
            if (flag)
            {
                __result = true;
                fail_reason = null;
            }
            
            //__result = flag && __instance.IsValidConduitLocation(source_go, cell, orientation, out fail_reason) && __instance.AreLogicPortsInValidPositions(source_go, cell, out fail_reason);            
        }

    
    }


}
