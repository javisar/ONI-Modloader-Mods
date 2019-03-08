using System.Collections.Generic;
using ONI_Common.Json;

namespace DisableNotifications
{
	public class DisableNotificationsState
	{
		public bool Enabled { get; set; } = true;

		public int Parameter { get; set; } = 512;


		public static BaseStateManager<DisableNotificationsState> StateManager
			= new BaseStateManager<DisableNotificationsState>("DisableNotifications");
	}
}