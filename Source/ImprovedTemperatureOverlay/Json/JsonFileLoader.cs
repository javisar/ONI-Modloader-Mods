namespace ImprovedTemperatureOverlay.Json
{
    //using MaterialColor.Data;
    using ONI_Common.Data;
    using ONI_Common.Json;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Logger = ONI_Common.IO.Logger;

    public class JsonFileLoader
    {
        private readonly Logger _logger;

        private ConfiguratorStateManager _configuratorStateManager;
    
        public JsonFileLoader(JsonManager jsonManager, Logger logger = null)
        {
            this._logger = logger;

            this.InitializeManagers(jsonManager);
        }


        public bool TryLoadTemperatureState(out ImprovedTemperatureOverlayState state)
        {
            try
            {
                state = this._configuratorStateManager.LoadTemperatureState();
                return true;
            }
            catch (Exception e)
            {
                this._logger.Log(e);
                this._logger.Log("Can't load overlay temperature state");

                state = new ImprovedTemperatureOverlayState();

                return false;
            }
        }
		
        private void InitializeManagers(JsonManager manager)
        {
            this._configuratorStateManager = new ConfiguratorStateManager(manager, this._logger);            
        }
        
    }
}