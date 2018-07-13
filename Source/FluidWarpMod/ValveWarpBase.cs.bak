// ValveBase
using FluidWarpMod;
using Harmony;
using KSerialization;
using System;
using System.Reflection;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class ValveWarpBase : ValveBase
{	

	//[Serialize]
	//private float currentFlow;

	private int inputCell;

	private int outputCell;

	/*
	new public float CurrentFlow
	{
		get
		{
			return this.currentFlow;
		}
		set
		{
			this.currentFlow = value;
		}
	}
	
	*/
	/*
	protected override void OnPrefabInit()
	{
		//base.OnPrefabInit();
		this.flowAccumulator = Game.Instance.accumulators.Add("Flow", this);
	}
	*/
	protected override void OnSpawn()
	{
		//base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		Conduit.GetFlowManager(this.conduitType).AddConduitUpdater(this.ConduitUpdate, ConduitFlowPriority.Default);
		this.UpdateAnim();
		this.OnCmpEnable();		
	}
	
	
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.flowAccumulator);
		Conduit.GetFlowManager(this.conduitType).RemoveConduitUpdater(this.ConduitUpdate);
		//base.OnCleanUp();
	}
	
	private void ConduitUpdate(float dt)
	{
		//Debug.Log("ConduitUpdate " + dt);
		ConduitFlow flowManager = Conduit.GetFlowManager(this.conduitType);
		ConduitFlow.Conduit conduit = flowManager.GetConduit(this.inputCell);

		if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell))
		{
			this.UpdateAnim();
		}

		if (this.inputCell > 0 && flowManager.HasConduit(this.inputCell) &&
            (this.outputCell > 0 && !flowManager.HasConduit(this.outputCell)))
		{
			ConduitFlow.ConduitContents contents = conduit.GetContents(flowManager);
			//float num = Mathf.Min(contents.mass, this.currentFlow * dt);
			FieldInfo fi = AccessTools.Field(typeof(ValveWarpBase), "currentFlow");
            //float num = Mathf.Min(contents.mass, (float)fi.GetValue(this) * dt);
            float num = Mathf.Min(contents.mass, 10f * dt);
            Debug.Log("ConduitUpdate " + num);
			if (num > 0f)
			{
				
				float num2 = num / contents.mass;
				int disease_count = (int)(num2 * (float)contents.diseaseCount);
                Debug.Log("List "+num);

                LiquidWarpData.LiquidPackets.Add(new PacketData((float)fi.GetValue(this), this.outputCell, contents.element, num, contents.temperature, contents.diseaseIdx, disease_count));
                /*
				float num3 = flowManager.AddElement(this.outputCell, contents.element, num, contents.temperature, contents.diseaseIdx, disease_count);
				Game.Instance.accumulators.Accumulate(this.flowAccumulator, num3);				
				*/
                //float num3 = Mathf.Min(num, 10f - contents.mass);
                float num3 = num;
                if (num3 > 0f)
                {
                    flowManager.RemoveElement(this.inputCell, num3);
                }
            }
			this.UpdateAnim();
            return;
		}
		

		if (this.outputCell > 0 && flowManager.HasConduit(this.outputCell))
		{
            ConduitFlow.Conduit conduitO = flowManager.GetConduit(this.outputCell);
            FieldInfo fi = AccessTools.Field(typeof(ValveWarpBase), "currentFlow");

            PacketData toRemove = null;

            foreach (PacketData packet in LiquidWarpData.LiquidPackets)
            {
                Debug.Log("currentFlow = "+ (float)fi.GetValue(this)+", packet.currentFlow = " + packet.current_flow);
                if ((float)fi.GetValue(this) == packet.current_flow)
                {                    
                    float num3 = flowManager.AddElement(this.outputCell, packet.element, packet.mass, packet.temperature, packet.disease_idx, packet.disease_count);
                    Debug.Log("Adding Element to pipe: " + packet.mass +","+ num3);
                    Game.Instance.accumulators.Accumulate(this.flowAccumulator, num3);
                    toRemove = packet;
                    break;
                }
            }

            if (toRemove != null)
            {
                LiquidWarpData.LiquidPackets.Remove(toRemove);
                toRemove = null;
            }

            
			this.UpdateAnim();
            return;
        }
	}
	
}
