using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class InverseElectrolyzerAlt: KMonoBehaviour, ISaveLoadable, ISecondaryInput
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

		this.inputCell = this.building.GetUtilityInputCell();
		this.outputCell = this.building.GetUtilityOutputCell();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		CellOffset rotatedOffset = this.building.GetRotatedOffset(this.portInfo.offset);
		this.sInputCell = Grid.OffsetCell(cell, rotatedOffset);
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
		this.itemSInput = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, this.sInputCell, base.gameObject);
		networkManager.AddToNetworks(this.sInputCell, this.itemSInput, true);
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

	private int inputCell = -1;

	private int outputCell = -1;

	private int sInputCell = -1;

	private FlowUtilityNetwork.NetworkItem itemInput;

	private FlowUtilityNetwork.NetworkItem itemOutput;

	private FlowUtilityNetwork.NetworkItem itemSInput;

	protected override void OnCleanUp()
	{
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
		networkManager.RemoveFromNetworks(this.sInputCell, this.itemSInput, true);
		ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);
		flowManager.RemoveConduitUpdater(this.OnConduitTick);
		base.OnCleanUp();
	}


	private void OnConduitTick(float dt)
	{
		bool value = false;
		if (this.operational.IsOperational)
		{
			ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);

			ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
			//int num = (contents.element != this.filteredElem) ? this.outputCell : this.filteredCell;
			ConduitFlow.ConduitContents contentsO = flowManager.GetContents(this.outputCell);
			ConduitFlow.ConduitContents contents2 = flowManager.GetContents(this.sInputCell);
			if (contents.mass > 0.111999989f
				&& contents2.mass > 0.888f
				&& contentsO.mass <= 0f)
			{
				if (contents.element != SimHashes.Hydrogen || contents2.element != SimHashes.Oxygen)
				{
					base.Trigger(-794517298, new BuildingHP.DamageSourceInfo
					{
						damage = 1,
						source = STRINGS.BUILDINGS.DAMAGESOURCES.BAD_INPUT_ELEMENT,
						popString = STRINGS.UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.WRONG_ELEMENT
					});
					flowManager.RemoveElement(this.inputCell, 0.111999989f);
					flowManager.RemoveElement(this.sInputCell, 0.888f);
				}
				else
				{
					value = true;
					//float num2 = flowManager.AddElement(num, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount);
					float num2 = flowManager.AddElement(this.outputCell, SimHashes.Steam, 1f, 523.15f, contents.diseaseIdx, 0);
					if (num2 > 0f)
					{
						flowManager.RemoveElement(this.inputCell, 0.111999989f);
						flowManager.RemoveElement(this.sInputCell, 0.888f);
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

