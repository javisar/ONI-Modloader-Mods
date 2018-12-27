using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class StirlingGenerator : KMonoBehaviour, ISaveLoadable, ISecondaryInput, ISecondaryOutput
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
		CellOffset rotatedOffset = this.building.GetRotatedOffset(this.inputPortInfo.offset);
		this.sInputCell = Grid.OffsetCell(cell, rotatedOffset);

		int cell2 = Grid.PosToCell(base.transform.GetPosition());
		CellOffset rotatedOffset2 = this.building.GetRotatedOffset(this.outputPortInfo.offset);
		this.sOutputCell = Grid.OffsetCell(cell2, rotatedOffset2);

		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.inputPortInfo.conduitType);

		this.itemSInput = new FlowUtilityNetwork.NetworkItem(this.inputPortInfo.conduitType, Endpoint.Sink, this.sInputCell, base.gameObject);
		networkManager.AddToNetworks(this.sInputCell, this.itemSInput, true);

		this.itemSOutput = new FlowUtilityNetwork.NetworkItem(this.outputPortInfo.conduitType, Endpoint.Source, this.sOutputCell, base.gameObject);
		networkManager.AddToNetworks(this.sOutputCell, this.itemSOutput, true);

		base.GetComponent<ConduitConsumer>().isConsuming = false;
		//this.OnFilterChanged(ElementLoader.FindElementByHash(this.filteredElem).tag);
		//this.filterable.onFilterChanged += this.OnFilterChanged;
		ConduitFlow flowManager = Conduit.GetFlowManager(this.inputPortInfo.conduitType);
		flowManager.AddConduitUpdater(this.OnConduitTick, ConduitFlowPriority.Default);
		//base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, ElementFilter.filterStatusItem, this);
	}

	[SerializeField]
	public ConduitPortInfo inputPortInfo;

	[SerializeField]
	public ConduitPortInfo outputPortInfo;

	[MyCmpReq]
	private Building building;

	//private Tag filteredTag = GameTags.Oxygen;

	private int inputCell = -1;

	private int outputCell = -1;

	private int sInputCell = -1;

	private int sOutputCell = -1;

	private FlowUtilityNetwork.NetworkItem itemInput;

	private FlowUtilityNetwork.NetworkItem itemOutput;

	private FlowUtilityNetwork.NetworkItem itemSInput;

	private FlowUtilityNetwork.NetworkItem itemSOutput;

	protected override void OnCleanUp()
	{
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.inputPortInfo.conduitType);
		networkManager.RemoveFromNetworks(this.sInputCell, this.itemSInput, true);
		ConduitFlow flowManager = Conduit.GetFlowManager(this.inputPortInfo.conduitType);
		flowManager.RemoveConduitUpdater(this.OnConduitTick);
		base.OnCleanUp();
	}


	private void OnConduitTick(float dt)
	{
		bool value = false;
		if (this.operational.IsOperational)
		{
			ConduitFlow flowManager = Conduit.GetFlowManager(this.inputPortInfo.conduitType);

			ConduitFlow.ConduitContents contentsI = flowManager.GetContents(this.inputCell);
			//int num = (contents.element != this.filteredElem) ? this.outputCell : this.filteredCell;
			ConduitFlow.ConduitContents contentsO = flowManager.GetContents(this.outputCell);
			ConduitFlow.ConduitContents contentsI2 = flowManager.GetContents(this.sInputCell);
			ConduitFlow.ConduitContents contentsO2 = flowManager.GetContents(this.sOutputCell);

			if (contentsI.mass > 0.111999989f
				&& contentsI2.mass > 0.888f
				&& contentsO.mass <= 0f)
			{
				if (contentsI.element != SimHashes.Hydrogen || contentsI2.element != SimHashes.Oxygen)
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
					float num2 = flowManager.AddElement(this.outputCell, SimHashes.Steam, 1f, 523.15f, contentsI.diseaseIdx, 0);
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

	 ConduitType ISecondaryInput.GetSecondaryConduitType()
	{
		return this.inputPortInfo.conduitType;
	}

	CellOffset ISecondaryInput.GetSecondaryConduitOffset()
	{
		return this.inputPortInfo.offset;
	}

	ConduitType ISecondaryOutput.GetSecondaryConduitType()
	{
		return this.outputPortInfo.conduitType;
	}

	CellOffset ISecondaryOutput.GetSecondaryConduitOffset()
	{
		return this.outputPortInfo.offset;
	}

}

