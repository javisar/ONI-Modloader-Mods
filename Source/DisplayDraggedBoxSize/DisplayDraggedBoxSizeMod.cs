using Harmony;
using UnityEngine;

namespace DisplayDraggedBoxSize
{
    public static class DisplayDraggedBoxSizeMod
    {
        [HarmonyPatch(typeof(DragTool), nameof(DragTool.OnMouseMove))]
        public static class StorageLockerConfigPatch
        {
            public static void Postfix(ref DragTool __instance, ref Vector3 cursorPos)
            {
                if (__instance.Dragging)
                {
                    DragTool.Mode mode = (DragTool.Mode)AccessTools.Field(typeof(DragTool), "mode").GetValue(__instance);
                    Vector3 downPos = (Vector3) AccessTools.Field(typeof(DragTool), "downPos").GetValue(__instance);

                    if (mode == DragTool.Mode.Box)
                    {
                        HoverTextConfiguration hoverText = __instance.GetComponent<HoverTextConfiguration>();

                        int index = hoverText.ToolName.IndexOf('[');

                        if (index != -1)
                        {
                            hoverText.ToolName = hoverText.ToolName.Remove(index - 1);
                        }

                        var downPosXY = Grid.PosToXY(downPos);
                        var cursorPosXY = Grid.PosToXY(cursorPos);

                        int x = Mathf.Abs(downPosXY.X - cursorPosXY.X) + 1;
                        int y = Mathf.Abs(downPosXY.Y - cursorPosXY.Y) + 1;

                        string boxSizeInfo = $" [{x}x{y}]";

                        hoverText.ToolName += boxSizeInfo;

                        Debug.Log(cursorPos - downPos);
                    }
                }
            }
        }
    }
}
