using Harmony;
using System.Reflection;


namespace FluidWarpMod
{
    public static class ValveBaseAccess
    {
        private static FieldInfo outputCell = AccessTools.Field(typeof(ValveBase), "outputCell");

        private static FieldInfo inputCell = AccessTools.Field(typeof(ValveBase), "inputCell");

        public static int GetOutputCell(this ValveBase valveBase)
        {
            return (int)outputCell.GetValue(valveBase);
        }

        public static int GetInputCell(this ValveBase valveBase)
        {
            return (int)inputCell.GetValue(valveBase);
        }
    }

    public static class ValveAccess
    {
        private static FieldInfo valveBase = AccessTools.Field(typeof(Valve), "valveBase");

        public static ValveBase GetValveBase(this Valve valve)
        {
            return (ValveBase)valveBase.GetValue(valve);
        }
    }
}
