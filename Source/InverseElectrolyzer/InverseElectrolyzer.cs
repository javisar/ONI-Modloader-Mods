using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class InverseElectrolyzer: KMonoBehaviour, ISaveLoadable, ISecondaryInput
{
	
	[MyCmpGet]
	private Operational operational;
	
	public Tag filterTag;	

	public List<Descriptor> GetDescriptors(BuildingDef def)
	{
		return null;
	}


	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.randomiseLoopedOffset = true;
		/*
		this.filterable = base.GetComponent<Filterable>();
		ConduitType conduitType = this.portInfo.conduitType;
		if (conduitType == ConduitType.Gas)// && this.filteredTag == GameTags.Water)
		{
			this.filteredTag = GameTags.Oxygen;
		}
		this.InitializeStatusItems();
		*/
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		//base.smi.StartSM();

		this.inputCell1 = this.building.GetUtilityInputCell();
		this.outputCell = this.building.GetUtilityOutputCell();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		CellOffset rotatedOffset = this.building.GetRotatedOffset(this.portInfo.offset);
		this.inputCell2 = Grid.OffsetCell(cell, rotatedOffset);

		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
		this.itemInput2 = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, this.inputCell2, base.gameObject);
		networkManager.AddToNetworks(this.inputCell2, this.itemInput2, true);
		base.GetComponent<ConduitConsumer>().isConsuming = false;
		//this.OnFilterChanged(ElementLoader.FindElementByHash(this.filteredElem).tag);
		//this.filterable.onFilterChanged += this.OnFilterChanged;
		ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);
		flowManager.AddConduitUpdater(this.OnConduitTick, ConduitFlowPriority.Default);
		//base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, ElementFilter.filterStatusItem, this);
	}

	[SerializeField]
	public ConduitPortInfo portInfo;

	[MyCmpReq]
	private Building building;

	//private Tag filteredTag = GameTags.Oxygen;

	private int inputCell1 = -1;

	private int inputCell2 = -1;

	private int outputCell = -1;

	private FlowUtilityNetwork.NetworkItem itemInput1;

	private FlowUtilityNetwork.NetworkItem itemInput2;

	private FlowUtilityNetwork.NetworkItem itemOutput;

	protected override void OnCleanUp()
	{
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
		networkManager.RemoveFromNetworks(this.inputCell2, this.itemInput2, true);
		ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);
		flowManager.RemoveConduitUpdater(this.OnConduitTick);
		base.OnCleanUp();
	}


	private void OnConduitTick(float dt)
	{
		bool value = false;
		if (this.operational.IsOperational)
		{
			ConduitFlow gasFlow = Conduit.GetFlowManager(this.portInfo.conduitType);

			ConduitFlow.ConduitContents contentsI1 = gasFlow.GetContents(this.inputCell1);
			ConduitFlow.ConduitContents contentsI2 = gasFlow.GetContents(this.inputCell2);

            //Debug.Log("contentsI1.mass: " + contentsI1.mass);
            //Debug.Log("contentsI2.mass: " + contentsI2.mass);

            //int num = (contents.element != this.filteredElem) ? this.outputCell : this.filteredCell;
            ConduitFlow.ConduitContents contentsO = gasFlow.GetContents(this.outputCell);
			
			if (contentsI1.element != SimHashes.Hydrogen || contentsI2.element != SimHashes.Oxygen)
			{
				base.Trigger((int)GameHashes.DoBuildingDamage, new BuildingHP.DamageSourceInfo
				{
					damage = 1,
					source = STRINGS.BUILDINGS.DAMAGESOURCES.BAD_INPUT_ELEMENT,
					popString = STRINGS.UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.WRONG_ELEMENT
				});
				gasFlow.RemoveElement(this.inputCell1, 0.111999989f);
				gasFlow.RemoveElement(this.inputCell2, 0.888f);
			}
			else
			{
                if (contentsI1.mass > 0.111999989f
                    && contentsI2.mass > 0.888f
                    && contentsO.mass <= 0f)
                {
                    value = true;
                    //float num2 = flowManager.AddElement(num, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount);
                    float outputTemperature = contentsI1.temperature * 0.111999989f + contentsI2.temperature * 0.888f;
                    //Debug.Log("outputTemperature: " + outputTemperature);
                    ConduitFlow liquidFlow = Conduit.GetFlowManager(ConduitType.Liquid);
                    float num2 = liquidFlow.AddElement(this.outputCell, SimHashes.Water, 1f, outputTemperature, contentsI1.diseaseIdx, 0);
                    if (num2 > 0f)
                    {
                        gasFlow.RemoveElement(this.inputCell1, 0.111999989f);
                        gasFlow.RemoveElement(this.inputCell2, 0.888f);
                    }
                }
                
			}
			
		}
		this.operational.SetActive(value, false);
	}

	private bool ShowInUtilityOverlay(HashedString mode, object data)
	{
		bool result = false;
		ElementFilter elementFilter = (ElementFilter)data;
		switch (elementFilter.portInfo.conduitType)
		{
			case ConduitType.Gas:
				result = OverlayModes.GasConduits.ID.Equals(mode);
				break;
			case ConduitType.Liquid:
				result = OverlayModes.LiquidConduits.ID.Equals(mode);
				break;
		}
		return result;
	}

	public ConduitType GetSecondaryConduitType()
	{
		return this.portInfo.conduitType;
	}

	public CellOffset GetSecondaryConduitOffset()
	{
		return this.portInfo.offset;
	}

}

