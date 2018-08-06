using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluidPhysicsMod
{
    
    [HarmonyPatch(typeof(ElementLoader), "FinaliseElementsTable")]
    internal class FluidPhysicsMod_ElementLoader_FinaliseElementsTable
    {
        private static void CopyElementProps(SimHashes src, SimHashes dst)
        {
            Element eSrc = ElementLoader.FindElementByHash(src);
            Element eDst = ElementLoader.FindElementByHash(dst);

            eDst.molarMass = eSrc.molarMass;
            eDst.flow = eSrc.flow;
            eDst.viscosity = eSrc.viscosity;    // doesnt have effect in gases?
        }
        private static void Prefix(ref Hashtable substanceList, ref SubstanceTable substanceTable)
        {
            Debug.Log(" === FluidPhysicsMod_ElementLoader_FinaliseElementsTable Postfix === ");

            ElementLoader.FindElementByHash(SimHashes.Oxygen).flow = 0.2f;

            CopyElementProps(SimHashes.Oxygen, SimHashes.Hydrogen);
            CopyElementProps(SimHashes.Oxygen, SimHashes.ChlorineGas);
            CopyElementProps(SimHashes.Oxygen, SimHashes.ContaminatedOxygen);
            CopyElementProps(SimHashes.Oxygen, SimHashes.Propane);
            CopyElementProps(SimHashes.Oxygen, SimHashes.Helium);
            CopyElementProps(SimHashes.Oxygen, SimHashes.Methane);
            CopyElementProps(SimHashes.Oxygen, SimHashes.CarbonDioxide);

            ElementLoader.FindElementByHash(SimHashes.Water).viscosity = 200f;
            ElementLoader.FindElementByHash(SimHashes.Water).flow = 0.2f; // doesnt have effect in liquids?

            CopyElementProps(SimHashes.Water, SimHashes.DirtyWater);
            CopyElementProps(SimHashes.Water, SimHashes.CrudeOil);
            CopyElementProps(SimHashes.Water, SimHashes.Petroleum);

        }

    }
    /*
    [HarmonyPatch(typeof(ElementLoader), "ParseLiquid", new Type[] { typeof(ElementLoader.LiquidEntry[]) })]
    internal class FluidPhysicsMod_ElementLoader_ParseLiquid
    {
        private static void Postfix(ElementLoader.LiquidEntry entry)
        {
            Debug.Log(" === FluidPhysicsMod_ElementLoader_ParseLiquid Postfix === ");

            Element element = ElementLoader.FindElementByHash(entry.elementId);
            element.molarMass = 159.994f;
            element.flow = 0.2f;
            //element.viscosity = 200f;
            //oxygen.molarMass
            
        }

    }

    [HarmonyPatch(typeof(ElementLoader), "ParseGas", new Type[] { typeof(ElementLoader.GasEntry[]) })]
    internal class FluidPhysicsMod_ElementLoader_GasLiquid
    {
        private static void Postfix(ElementLoader.LiquidEntry entry)
        {
            Debug.Log(" === FluidPhysicsMod_ElementLoader_ParseGas Postfix === ");

            Element element = ElementLoader.FindElementByHash(entry.elementId);
            element.molarMass = 1801528f;
            element.flow = 0.2f;
            //element.viscosity = 200f;
            //oxygen.molarMass



        }

    }
    */
}
