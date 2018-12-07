namespace ModLoader
{
    using Harmony;

    /// <summary>
    /// Stop constraining camera to the world.
    /// </summary>
    [HarmonyPatch(typeof(CameraController), "ConstrainToWorld")]
    public static class FreeCameraMod
    {
        public static bool Prefix()
        {
            return false;
        }
    }
}
