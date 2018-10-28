using Harmony;
using System;


namespace NaphthaViscosityMod
{
    [HarmonyPatch(typeof(ElementLoader), "ParseLiquid", new Type[] { typeof(ElementLoader.LiquidEntry) })]
    internal class NaphthaViscosityMod_ElementLoader_ParseLiquid
    {
        private static void Postfix()
        {
            Debug.Log(" === NaphthaViscosityMod_ElementLoader_ParseLiquid === ");

            Element naphtha = ElementLoader.FindElementByHash(SimHashes.Naphtha);

            naphtha.viscosity = 0;
            /*
            Element water = ElementLoader.FindElementByHash(SimHashes.Water);
            Element dirtyWater = ElementLoader.FindElementByHash(SimHashes.DirtyWater);

            Debug.Log(" === water.viscosity: " + water.viscosity + " === ");
            Debug.Log(" === water.minHorizontalLiquidFlow: " + water.minHorizontalLiquidFlow + " === ");
            Debug.Log(" === water.minVerticalLiquidFlow: " + water.minVerticalLiquidFlow + " === ");

            Debug.Log(" === dirtyWater.viscosity: " + dirtyWater.viscosity + " === ");
            Debug.Log(" === dirtyWater.minHorizontalLiquidFlow: " + dirtyWater.minHorizontalLiquidFlow + " === ");
            Debug.Log(" === dirtyWater.minVerticalLiquidFlow: " + dirtyWater.minVerticalLiquidFlow + " === ");
            */


        }

    }
}
