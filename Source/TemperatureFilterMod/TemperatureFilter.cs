using KSerialization;
using STRINGS;
using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace TemperatureFilterMod
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class TemperatureFilter : KMonoBehaviour, ISaveLoadable, ISecondaryOutput, IThresholdSwitch
    {
        [SerializeField]
        [Serialize]
        protected float threshold = 0.0f;
        [SerializeField]
        [Serialize]
        protected bool activateAboveThreshold = true;
        public float rangeMin = 0.0f;
        public float rangeMax = 373.15f;

        private int inputCell = -1;
        private int outputCell = -1;
        private int filteredCell = -1;
        [SerializeField]
        public ConduitPortInfo portInfo;
        [MyCmpReq]
        private Operational operational;
        [MyCmpReq]
        private Building building;
        [MyCmpReq]
        private KSelectable selectable;
        private Guid needsConduitStatusItemGuid;
        private Guid conduitBlockedStatusItemGuid;
        private FlowUtilityNetwork.NetworkItem itemFilter;

        public float Threshold
        {
            get
            {
                return this.threshold;
            }
            set
            {
                this.threshold = value;
            }
        }

        public bool ActivateAboveThreshold
        {
            get
            {
                return this.activateAboveThreshold;
            }
            set
            {
                this.activateAboveThreshold = value;
            }
        }

        public float CurrentValue => Conduit.GetFlowManager(this.portInfo.conduitType).GetContents(inputCell).temperature;

        public float RangeMin => rangeMin;

        public float RangeMax => rangeMax;

        public LocString Title
        {
            get
            {
                return UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE;
            }
        }

        public LocString ThresholdValueName
        {
            get
            {
                return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE;
            }
        }

        public string AboveToolTip
        {
            get
            {
                return (string)UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_ABOVE;
            }
        }

        public string BelowToolTip
        {
            get
            {
                return (string)UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_BELOW;
            }
        }

        public ThresholdScreenLayoutType LayoutType
        {
            get
            {
                return ThresholdScreenLayoutType.SliderBar;
            }
        }

        public int IncrementScale
        {
            get
            {
                return 1;
            }
        }

        public NonLinearSlider.Range[] GetRanges
        {
            get
            {
                return new NonLinearSlider.Range[4]
                {
                    new NonLinearSlider.Range(25f, 260f),
                    new NonLinearSlider.Range(50f, 400f),
                    new NonLinearSlider.Range(12f, 1500f),
                    new NonLinearSlider.Range(13f, 10000f)
                };
            }
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            this.inputCell = this.building.GetUtilityInputCell();
            this.outputCell = this.building.GetUtilityOutputCell();
            this.filteredCell = Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), this.building.GetRotatedOffset(this.portInfo.offset));
            IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
            this.itemFilter = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Source, this.filteredCell, this.gameObject);
            networkManager.AddToNetworks(this.filteredCell, (object)this.itemFilter, true);
            this.GetComponent<ConduitConsumer>().isConsuming = false;
            Conduit.GetFlowManager(this.portInfo.conduitType).AddConduitUpdater(new Action<float>(this.OnConduitTick), ConduitFlowPriority.Default);
            this.UpdateConduitExistsStatus();
            this.UpdateConduitBlockedStatus();
            ScenePartitionerLayer layer = (ScenePartitionerLayer)null;
            switch (this.portInfo.conduitType)
            {
                case ConduitType.Gas:
                    layer = GameScenePartitioner.Instance.gasConduitsLayer;
                    break;
                case ConduitType.Liquid:
                    layer = GameScenePartitioner.Instance.liquidConduitsLayer;
                    break;
                case ConduitType.Solid:
                    layer = GameScenePartitioner.Instance.solidConduitsLayer;
                    break;
            }
            if (layer == null)
                return;
        }

        protected override void OnCleanUp()
        {
            Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.filteredCell, (object)this.itemFilter, true);
            Conduit.GetFlowManager(this.portInfo.conduitType).RemoveConduitUpdater(new Action<float>(this.OnConduitTick));
            base.OnCleanUp();
        }

        private void OnConduitTick(float dt)
        {
            bool flag = false;
            this.UpdateConduitBlockedStatus();
            if (this.operational.IsOperational)
            {
                ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);
                ConduitFlow.ConduitContents contents1 = flowManager.GetContents(this.inputCell);
                int num = -1;
                if (activateAboveThreshold)
                {
                    num = contents1.temperature < this.threshold ? this.outputCell : this.filteredCell;
                }
                else
                {
                    num = contents1.temperature > this.threshold ? this.outputCell : this.filteredCell;
                }
                ConduitFlow.ConduitContents contents2 = flowManager.GetContents(num);
                if ((double)contents1.mass > 0.0 && 
                    ((double)contents2.mass <= 0.0 || contents2.element.Equals(contents1.element) )
                   ) 
                {
                    flag = true;
                    float delta = flowManager.AddElement(num, contents1.element, contents1.mass, contents1.temperature, contents1.diseaseIdx, contents1.diseaseCount);
                    if ((double)delta > 0.0)
                        flowManager.RemoveElement(this.inputCell, delta);
                }
            }
            this.operational.SetActive(flag, false);
        }

        private void UpdateConduitExistsStatus()
        {
            bool flag1 = RequireOutputs.IsConnected(this.filteredCell, this.portInfo.conduitType);
            StatusItem status_item;
            switch (this.portInfo.conduitType)
            {
                case ConduitType.Gas:
                    status_item = Db.Get().BuildingStatusItems.NeedGasOut;
                    break;
                case ConduitType.Liquid:
                    status_item = Db.Get().BuildingStatusItems.NeedLiquidOut;
                    break;
                case ConduitType.Solid:
                    status_item = Db.Get().BuildingStatusItems.NeedSolidOut;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            bool flag2 = this.needsConduitStatusItemGuid != Guid.Empty;
            if (flag1 != flag2)
                return;
            this.needsConduitStatusItemGuid = this.selectable.ToggleStatusItem(status_item, this.needsConduitStatusItemGuid, !flag1, (object)null);
        }

        private void UpdateConduitBlockedStatus()
        {
            bool flag1 = Conduit.GetFlowManager(this.portInfo.conduitType).IsConduitEmpty(this.filteredCell);
            StatusItem blockedMultiples = Db.Get().BuildingStatusItems.ConduitBlockedMultiples;
            bool flag2 = this.conduitBlockedStatusItemGuid != Guid.Empty;
            if (flag1 != flag2)
                return;
            this.conduitBlockedStatusItemGuid = this.selectable.ToggleStatusItem(blockedMultiples, this.conduitBlockedStatusItemGuid, !flag1, (object)null);
        }

        private bool ShowInUtilityOverlay(HashedString mode, object data)
        {
            bool flag = false;
            switch (((TemperatureFilter)data).portInfo.conduitType)
            {
                case ConduitType.Gas:
                    flag = mode == OverlayModes.GasConduits.ID;
                    break;
                case ConduitType.Liquid:
                    flag = mode == OverlayModes.LiquidConduits.ID;
                    break;
            }
            return flag;
        }

        public ConduitType GetSecondaryConduitType()
        {
            return this.portInfo.conduitType;
        }

        public CellOffset GetSecondaryConduitOffset()
        {
            return this.portInfo.offset;
        }

        public int GetFilteredCell()
        {
            return this.filteredCell;
        }

        public float GetRangeMinInputField()
        {
            return GameUtil.GetConvertedTemperature(this.RangeMin, false);
        }

        public float GetRangeMaxInputField()
        {
            return GameUtil.GetConvertedTemperature(this.RangeMax, false);
        }

        public LocString ThresholdValueUnits()
        {
            LocString locString = (LocString)null;
            switch (GameUtil.temperatureUnit)
            {
                case GameUtil.TemperatureUnit.Celsius:
                    locString = UI.UNITSUFFIXES.TEMPERATURE.CELSIUS;
                    break;
                case GameUtil.TemperatureUnit.Fahrenheit:
                    locString = UI.UNITSUFFIXES.TEMPERATURE.FAHRENHEIT;
                    break;
                case GameUtil.TemperatureUnit.Kelvin:
                    locString = UI.UNITSUFFIXES.TEMPERATURE.KELVIN;
                    break;
            }
            return locString;
        }

        public string Format(float value, bool units)
        {
            return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, false);
        }

        public float ProcessedSliderValue(float input)
        {
            return Mathf.Round(input);
        }

        public float ProcessedInputValue(float input)
        {
            return GameUtil.GetTemperatureConvertedToKelvin(input);
        }
    }
}
