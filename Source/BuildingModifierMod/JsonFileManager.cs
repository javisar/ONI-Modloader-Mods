namespace BuildingModifierMod
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    //using Logger = ONI_Common.IO.Logger;

    public class JsonFileManager
    {
        //private readonly Logger _logger;
        private readonly JsonManager _jsonManager;


		public JsonManager GetJsonManager()
		{
			return _jsonManager;
		}

		public JsonFileManager(JsonManager jsonManager)
        {
            //this._logger = logger;

            this._jsonManager = jsonManager;
        }
        

        public bool TryLoadConfiguration<T>(string path, out T state)
        {
            try
            {
                state = _jsonManager.Deserialize<T>(path);
                return true;
            }
            catch (Exception ex)
            {
                const string Message = "Can't load configurator state.";

               
                Debug.Log(Message);
                Debug.LogException(ex);

                state = (T)Activator.CreateInstance(typeof(T));

                return false;
            }
        }

        public bool TrySaveConfiguration<T>(string path, T state)
        {
            try
            {
                _jsonManager.Serialize<T>(state,path);
                return true;
            }
            catch (Exception ex)
            {
                const string Message = "Can't save configurator state.";

                Debug.Log(Message);
                Debug.LogException(ex);

                return false;
            }
        }
    }
}