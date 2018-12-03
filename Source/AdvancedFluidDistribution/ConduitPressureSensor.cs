using KSerialization;
using UnityEngine;

namespace AdvancedFluidDistribution
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class ConduitPressureSensor : ConduitThresholdSensor, IThresholdSwitch
    {
        private float rangeMin = 0f;

        private float rangeMax = 1000f;

        private readonly static HashedString TINT_SYMBOL;

        public string AboveToolTip
        {
            get
            {
                return "Switch will be on if the pipe contents mass is above {0}";
            }
        }

        public string BelowToolTip
        {
            get
            {
                return "Switch will be off if the pipe contents mass is below {0}";
            }
        }

        public override float CurrentValue
        {
            get
            {
                int cell = Grid.PosToCell(this);
                ConduitFlow flowManager = Conduit.GetFlowManager(this.conduitType);
//                Logger.LogFormat(" Pressure Sensor {0} current value {1}", GetInstanceID(), flowManager.GetContents(cell).mass);
                return (float)flowManager.GetContents(cell).mass;
            }
        }

        public float RangeMax
        {
            get { return this.rangeMax; }
            set { this.rangeMax = value; }
        }

        public float RangeMin
        {
            get { return this.rangeMin; }
            set { this.rangeMin = value; }
        }

        public LocString ThresholdValueName
        {
            get
            {
                return "Mass";
            }
        }

        public LocString Title
        {
            get
            {
                return "Mass Threshold";
            }
        }

        static ConduitPressureSensor()
        {
            TINT_SYMBOL = "germs";
        }

        public ConduitPressureSensor()
        {
        }

        public string Format(float value, bool units)
        {
            return GameUtil.GetFormattedInt((float)((int)value), GameUtil.TimeSlice.None);
        }

        public float GetRangeMaxInputField()
        {
            return this.rangeMax;
        }

        public float GetRangeMinInputField()
        {
            return this.rangeMin;
        }

        public float ProcessedInputValue(float input)
        {
            return input;
        }

        public float ProcessedSliderValue(float input)
        {
            return input;
        }

        public LocString ThresholdValueUnits()
        {
            return "g.";
        }

        protected override void UpdateVisualState(bool force = false)
        {
            if (this.wasOn != this.switchedOn || force)
            {
                this.wasOn = this.switchedOn;
                if (!this.switchedOn)
                {
                    this.animController.Play(ConduitSensor.OFF_ANIMS, KAnim.PlayMode.Once);
                }
                else
                {
                    this.animController.Play(ConduitSensor.ON_ANIMS, KAnim.PlayMode.Loop);
                    int cell = Grid.PosToCell(this);
                    ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(this.conduitType).GetContents(cell);
                    Color32 color32 = Color.white;
                    this.animController.SetSymbolTint(TINT_SYMBOL, color32);
                }
            }
        }

        protected override void ConduitUpdate(float dt)
        {
            float currentValue = this.CurrentValue;
            if (this.activateAboveThreshold)
            {
                if (currentValue > this.threshold && !base.IsSwitchedOn || currentValue <= this.threshold && base.IsSwitchedOn)
                {
                    this.Toggle();
                }
            }
            else if (currentValue > this.threshold && base.IsSwitchedOn || currentValue <= this.threshold && !base.IsSwitchedOn)
            {
                this.Toggle();
            }
        }
    }
}