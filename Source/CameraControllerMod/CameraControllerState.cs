using System.Collections.Generic;
using ONI_Common.Json;

namespace CameraControllerMod
{
    public class CameraControllerState
    {
        public bool Enabled { get; set; } = true;

		public bool Debug { get; set; } = false;


        public bool maxOrthographicSizeEnabled { get; set; } = true;

        public float maxOrthographicSize { get; set; } = 100f;

        public float maxOrthographicSizeDebug { get; set; } = 300f;


        public bool ConstrainToWorld { get; set; } = true;

		public static BaseStateManager<CameraControllerState> StateManager
			                    = new BaseStateManager<CameraControllerState>("CameraController");
	}
}