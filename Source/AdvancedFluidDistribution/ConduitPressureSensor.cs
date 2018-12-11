using KSerialization;
using UnityEngine;

namespace FluidPressureSensor
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class ConduitPressureSensor : ConduitThresholdSensor, IThresholdSwitch
    {
        private static LocString MASS = new LocString("Mass", "STRINGS.BUILDINGS.CONDUITPRESSURESENSOR.MASS");
        private static LocString MASS_THRESHOLD = new LocString("Mass Threshold", "STRINGS.BUILDINGS.MASS_THRESHOLD");
        private static LocString GRAMMS = new LocString("g.", "STRINGS.BUILDINGS.GRAMMS");
        private readonly static HashedString TINT_SYMBOL;

        public float rangeMax;
        public float rangeMin;

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
                return (float)flowManager.GetContents(cell).mass * 1000f;
            }
        }

        public float RangeMax { get { return rangeMax; } }

        public float RangeMin { get {return rangeMin; } }

        public LocString ThresholdValueName
        {
            get
            {
                return MASS;
            }
        }

        public LocString Title
        {
            get
            {
                return MASS_THRESHOLD;
            }
        }

        public ThresholdScreenLayoutType LayoutType { get { return ThresholdScreenLayoutType.SliderBar; } }

        public int IncrementScale { get { return 1; } }

        public NonLinearSlider.Range[] GetRanges { get { return NonLinearSlider.GetDefaultRange(this.RangeMax); } }

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
            return GRAMMS;
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