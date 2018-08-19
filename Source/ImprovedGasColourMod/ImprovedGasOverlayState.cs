namespace ImprovedGasColourMod
{
    public class ImprovedGasOverlayState
    {
        private float _gasPressureEnd = 2.5f;
        
        public bool Enabled { get; set; } = true;

        public float GasPressureEnd
        {
            get
            {
                return this._gasPressureEnd;
            }

            set
            {
                this._gasPressureEnd = this._gasPressureEnd <= 0 ? float.Epsilon : value;
            }
        }

        // not used anymore
        public float GasPressureStart { get; set; } = 0.1f;
        
        // gas overlay
        public float MinimumGasColorIntensity { get; set; } = 0.25f;

        // major fps drop when enabled
        public bool AdvancedGasOverlayDebugging { get; set; } = false;

        public bool ShowEarDrumPopMarker { get; set; } = true;
     
        
    }
}