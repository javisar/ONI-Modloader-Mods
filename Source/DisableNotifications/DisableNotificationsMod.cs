using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;
using static Notification;

namespace DisableNotifications
{
	[HarmonyPatch(typeof(StatusItem), "AddNotification")]
	internal class DisableNotificationsMod_StatusItem_AddNotification
	{

		private static void Postfix(StatusItem __instance)
		{
			if (!DisableNotificationsState.StateManager.State.Enabled) return;

			Debug.Log(" === DisableNotificationsMod_StatusItem_AddNotification Postfix === ");

			//__instance.shouldNotify = false;
			
			//__instance.notificationDelay = 10f;

			/*
			BuildingStatusItems.CreateStatusItems() 
			CreatureStatusItems.CreateStatusItems() 
			DuplicantStatusItems.CreateStatusItems() 
			*/
		}
	}

	[HarmonyPatch(typeof(Notification), MethodType.Constructor, new Type[] { typeof(string), typeof(NotificationType), typeof(HashedString), typeof(Func<List<Notification>, object, string>), typeof(object), typeof(bool), typeof(float), typeof(ClickCallback), typeof(object), typeof(Transform) })]
	internal class DisableNotificationsMod_Notification_Constructor
	{

		private static void Postfix(ref Notification __instance)
		{
			if (!DisableNotificationsState.StateManager.State.Enabled) return;

			Debug.Log(" === DisableNotificationsMod_Notification_Constructor Postfix === ");
			__instance.expires = true;
		}
	}

	[HarmonyPatch(typeof(NotificationScreen), "Update")]
	internal class DisableNotificationsMod_Notification_Update
	{

		private static void Prefix(NotificationScreen __instance)
		{
			if (!DisableNotificationsState.StateManager.State.Enabled) return;

			Debug.Log(" === DisableNotificationsMod_Notification_Update Prefix === "+ __instance.lifetime);			
		}
	}
}
