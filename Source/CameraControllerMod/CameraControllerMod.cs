namespace CameraControllerMod
{
    using Harmony;

    [HarmonyPatch(typeof(CameraController), "OnPrefabInit")]
    public static class CameraControllerMod
    {
        public static void Prefix(CameraController __instance)
        {
            if (!CameraControllerState.StateManager.State.Enabled 
                || !CameraControllerState.StateManager.State.maxOrthographicSizeEnabled) return;

            Debug.Log(" === CameraControllerMod INI === ");


            AccessTools.Field(typeof(CameraController), "maxOrthographicSize").SetValue(__instance, CameraControllerState.StateManager.State.maxOrthographicSize);
			//AccessTools.Field(typeof(CameraController), "maxOrthographicSizeDebug").SetValue(__instance, 300f);
			TuningData<CameraController.Tuning>.Get().maxOrthographicSizeDebug = CameraControllerState.StateManager.State.maxOrthographicSizeDebug;

			// Traverse.Create<CameraController>().Property("maxOrthographicSize").SetValue(100.0);
			// Traverse.Create<CameraController>().Property("maxOrthographicSizeDebug").SetValue(200.0);
			Debug.Log(" === CameraControllerMod END === ");
        }
    }
    /// <summary>
    /// Needed when a new world is generated. SetMaxOrthographicSize is run once and only then.
    /// </summary>
    [HarmonyPatch(typeof(CameraController), nameof(CameraController.SetMaxOrthographicSize))]
    public static class CameraControllerMod2
    {
        public static void Prefix(CameraController __instance, ref float size)
        {
            if (!CameraControllerState.StateManager.State.Enabled
                || !CameraControllerState.StateManager.State.maxOrthographicSizeEnabled) return;

            size = CameraControllerState.StateManager.State.maxOrthographicSize;
        }
    }

    /// <summary>
    /// Stop constraining camera to the world.
    /// </summary>
    [HarmonyPatch(typeof(CameraController), "ConstrainToWorld")]
    public static class FreeCameraMod
    {
        public static bool Prefix()
        {
            if (!CameraControllerState.StateManager.State.Enabled
                || !CameraControllerState.StateManager.State.ConstrainToWorld) return true;

            return false;
        }
    }
}