// Door
using Harmony;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class AirLockDoor : Door
{
	/*
	new public class Controller : Door.Controller
	{
		
		public override void InitializeStates(out BaseState default_state)
		{
			Debug.Log("=== AirLockDoor.Controller.InitializeStates ===");
			MethodInfo mi = AccessTools.Method(typeof(Door), "SetActive", new Type[] { typeof(bool)});
			MethodInfo mi2 = AccessTools.Method(typeof(Door), "SetWorldState", new Type[] {  });
			MethodInfo mi3 = AccessTools.Method(typeof(Door), "RefreshControlState", new Type[] { });
			MethodInfo mi4 = AccessTools.Method(typeof(Door), "UpdateAnimAndSoundParams", new Type[] {typeof(bool) });
			FieldInfo fi0 = AccessTools.Field(typeof(Door), "doorOpeningSound");
			FieldInfo fi = AccessTools.Field(typeof(Door), "doorClosingSound");
			FieldInfo fi2 = AccessTools.Field(typeof(Door), "hasBeenUnsealed");
			FieldInfo fi3 = AccessTools.Field(typeof(Door),"controlState");
			FieldInfo fi4 = AccessTools.Field(typeof(Door), "on");
			FieldInfo fi5 = AccessTools.Field(typeof(Door), "loopingSounds");
			FieldInfo fi6 = AccessTools.Field(typeof(Door), "SOUND_PROGRESS_PARAMETER");

			base.serializable = true;
			default_state = this.closed;
			base.root.Update("RefreshIsBlocked", delegate (Instance smi, float dt)
			{
				smi.RefreshIsBlocked();
			}, UpdateRate.SIM_200ms, false).ParamTransition(this.isSealed, this.Sealed.closed, (Instance smi, bool p) => p);
			this.closeblocked.PlayAnim("open").ParamTransition(this.isOpen, this.open, (Instance smi, bool p) => p).ParamTransition(this.isBlocked, this.closedelay, (Instance smi, bool p) => !p);
			this.closedelay.PlayAnim("open").ScheduleGoTo(0.5f, this.closing).ParamTransition(this.isOpen, this.open, (Instance smi, bool p) => p)
				.ParamTransition(this.isBlocked, this.closeblocked, (Instance smi, bool p) => p);
			//this.closing.ParamTransition(this.isBlocked, this.closeblocked, (Instance smi, bool p) => p).ToggleTag(GameTags.Transition).ToggleLoopingSound("Closing loop", (Instance smi) => smi.master.doorClosingSound, (Instance smi) => !string.IsNullOrEmpty(smi.master.doorClosingSound))
			this.closing.ParamTransition(this.isBlocked, this.closeblocked, (Instance smi, bool p) => p).ToggleTag(GameTags.Transition).ToggleLoopingSound("Closing loop", (Instance smi) => (string)fi.GetValue(smi.master), (Instance smi) => !string.IsNullOrEmpty((string)fi.GetValue(smi.master)))
				.Enter("SetParams", delegate (Instance smi)
				{
					//smi.master.UpdateAnimAndSoundParams(smi.master.on);
					mi4.Invoke(smi.master, new object[] { (bool)fi4.GetValue(smi.master) });
				})
				.Update(delegate (Instance smi, float dt)
				{
					//if (smi.master.doorClosingSound != null)
					//{
					//	smi.master.loopingSounds.UpdateSecondParameter(smi.master.doorClosingSound, Door.SOUND_PROGRESS_PARAMETER, smi.animController.GetPositionPercent());
					//}
					if ((string)fi.GetValue(smi.master) != null)
					{
						((LoopingSounds)fi5.GetValue(smi.master)).UpdateSecondParameter((string)fi.GetValue(smi.master), (string)fi6.GetValue(null), smi.animController.GetPositionPercent());
					}

				}, UpdateRate.SIM_33ms, false)
				.Enter("SetActive", delegate (Instance smi)
				{
					//smi.master.SetActive(true);
					mi.Invoke(smi.master, new object[] { true });
				})
				.Exit("SetActive", delegate (Instance smi)
				{
					//smi.master.SetActive(false);
					mi.Invoke(smi.master, new object[] { false });
				})
				.PlayAnim("closing")
				.OnAnimQueueComplete(this.closed);
			this.open.PlayAnim("open").ParamTransition(this.isOpen, this.closeblocked, (Instance smi, bool p) => !p).Enter("SetWorldStateOpen", delegate (Instance smi)
			{
				//smi.master.SetWorldState();
				mi2.Invoke(smi.master, null);
			});
			this.closed.PlayAnim("closed").ParamTransition(this.isOpen, this.opening, (Instance smi, bool p) => p).ParamTransition(this.isLocked, this.locking, (Instance smi, bool p) => p)
				.Enter("SetWorldStateClosed", delegate (Instance smi)
				{
					//smi.master.SetWorldState();
					mi2.Invoke(smi.master, null);
				});
			this.locking.PlayAnim("locked_pre").OnAnimQueueComplete(this.locked).Enter("SetWorldStateClosed", delegate (Instance smi)
			{
				//smi.master.SetWorldState();
				mi2.Invoke(smi.master, null);
			});
			this.locked.PlayAnim("locked").ParamTransition(this.isLocked, this.unlocking, (Instance smi, bool p) => !p);
			this.unlocking.PlayAnim("locked_pst").OnAnimQueueComplete(this.closed);
			//this.opening.ToggleTag(GameTags.Transition).ToggleLoopingSound("Opening loop", (Instance smi) => smi.master.doorOpeningSound, (Instance smi) => !string.IsNullOrEmpty(smi.master.doorOpeningSound)).Enter("SetParams", delegate (Instance smi)			
			this.opening.ToggleTag(GameTags.Transition).ToggleLoopingSound("Opening loop", (Instance smi) => (string)fi0.GetValue(smi.master), (Instance smi) => !string.IsNullOrEmpty((string)fi0.GetValue(smi.master))).Enter("SetParams", delegate (Instance smi)
			{
				//smi.master.UpdateAnimAndSoundParams(smi.master.on);
				mi4.Invoke(smi.master, new object[] { (bool)fi4.GetValue(smi.master) });
			})
				.Update(delegate (Instance smi, float dt)
				{
					//if (smi.master.doorOpeningSound != null)
					//{
					//	smi.master.loopingSounds.UpdateSecondParameter(smi.master.doorOpeningSound, Door.SOUND_PROGRESS_PARAMETER, smi.animController.GetPositionPercent());
					//}

					if ((string)fi0.GetValue(smi.master) != null)
					{
						((LoopingSounds)fi5.GetValue(smi.master)).UpdateSecondParameter((string)fi0.GetValue(smi.master), (string)fi6.GetValue(null), smi.animController.GetPositionPercent());
					}

				}, UpdateRate.SIM_33ms, false)
				.Enter("SetActive", delegate (Instance smi)
				{
					//smi.master.SetActive(true);
					mi.Invoke(smi.master, new object[] { true });
				})
				.Exit("SetActive", delegate (Instance smi)
				{
					//smi.master.SetActive(false);
					mi.Invoke(smi.master, new object[] { false });
				})
				.PlayAnim("opening")
				.OnAnimQueueComplete(this.open);
			this.Sealed.Enter(delegate (Instance smi)
			{
				OccupyArea component = ((Component)smi.master).GetComponent<OccupyArea>();
				for (int i = 0; i < component.OccupiedCellsOffsets.Length; i++)
				{
					Grid.PreventFogOfWarReveal[Grid.OffsetCell(Grid.PosToCell(smi.master.gameObject), component.OccupiedCellsOffsets[i])] = false;
				}
				smi.sm.isLocked.Set(true, smi);
				//smi.master.controlState = ControlState.Closed;
				fi3.SetValue(smi.master, ControlState.Closed);
				//smi.master.RefreshControlState();
				mi3.Invoke(smi.master, new object[] { false });
				if (((Component)smi.master).GetComponent<Unsealable>().facingRight)
				{
					KBatchedAnimController component2 = ((Component)smi.master).GetComponent<KBatchedAnimController>();
					component2.FlipX = true;
				}
			}).Enter("SetWorldStateClosed", delegate (Instance smi)
			{
				//smi.master.SetWorldState();
				mi2.Invoke(smi.master, null);
			}).Exit(delegate (Instance smi)
			{
				smi.sm.isLocked.Set(false, smi);
				((Component)smi.master).GetComponent<AccessControl>().controlEnabled = true;
				//smi.master.controlState = ControlState.Opened;
				fi3.SetValue(smi.master, ControlState.Opened);
				//smi.master.RefreshControlState();
				mi3.Invoke(smi.master, new object[] { false });
				smi.sm.isOpen.Set(true, smi);
				smi.sm.isLocked.Set(false, smi);
				smi.sm.isSealed.Set(false, smi);
			});
			this.Sealed.closed.PlayAnim("sealed", KAnim.PlayMode.Once);
			this.Sealed.awaiting_unlock.ToggleChore((Instance smi) => this.CreateUnsealChore(smi, true), this.Sealed.chore_pst);
			this.Sealed.chore_pst.Enter(delegate (Instance smi)
			{
				//smi.master.hasBeenUnsealed = true;
				fi2.SetValue(smi.master, true);
				if (((Component)smi.master).GetComponent<Unsealable>().unsealed)
				{
					smi.GoTo(this.opening);
					FogOfWarMask.ClearMask(Grid.CellRight(Grid.PosToCell(smi.master.gameObject)));
					FogOfWarMask.ClearMask(Grid.CellLeft(Grid.PosToCell(smi.master.gameObject)));
				}
				else
				{
					smi.GoTo(this.Sealed.closed);
				}
			});
		}

		private Chore CreateUnsealChore(Instance smi, bool approach_right)
		{
			return new WorkChore<Unsealable>(Db.Get().ChoreTypes.Toggle, smi.master, null, null, true, null, null, null, true, null, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 0, false);
		}
	}


	protected override void OnSpawn()
	{
		base.OnSpawn();

		FieldInfo fi = AccessTools.Field(typeof(Door), "controller");
		fi.SetValue(this, new Controller.Instance(this));
		((Controller.Instance)fi.GetValue(this)).StartSM();
		//this.controller = new Controller.Instance(this);
		//this.controller.StartSM();
	}

	*/
}
