using ONI_Common.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MaterialColor.IO
{
    public class ConfigWatcher : IRender1000ms, IDisposable
    {
        private bool _configuratorStateChanged;
        private bool _elementColorInfosChanged;

        public ConfigWatcher()
        {
            this.SubscribeToFileChangeNotifier();
        }

        void IRender1000ms.Render1000ms(float dt)
        {
            if (_elementColorInfosChanged || _configuratorStateChanged)
            {
                Painter.Refresh();
                _elementColorInfosChanged = _configuratorStateChanged = false;
            }
        }

        // TODO: too many parameters are needed for stopping file watch, fix oni-common
        // TODO: test if event handlers are cleared properly
        public void Dispose()
        {
            FileChangeNotifier.StopFileWatch
            (
                Paths.MaterialColorStateFileName,
                Paths.MaterialConfigPath,
                OnMaterialStateChanged
            );

            FileChangeNotifier.StopFileWatch
            (
                "*.json",
                Paths.ElementColorInfosDirectory,
                OnElementColorsInfosChanged
            );
        }

        private void OnMaterialStateChanged(object sender, FileSystemEventArgs e)
        {
            if (!State.TryReloadConfiguratorState())
            {
                return;
            }

            _configuratorStateChanged = true;

            const string message = "Configurator state changed.";

            Debug.Log(message);
        }

        private void OnElementColorsInfosChanged(object sender, FileSystemEventArgs e)
        {
            bool reloadColorInfosResult = false;

            try
            {
                reloadColorInfosResult = State.TryReloadElementColorInfos();
            }
            catch (Exception ex)
            {
                State.Logger.Log("ReloadElementColorInfos failed.");
                State.Logger.Log(ex);
            }

            if (reloadColorInfosResult)
            {
                _elementColorInfosChanged = true;

                const string message = "Element color infos changed.";

                State.Logger.Log(message);
                Debug.LogError(message);
            }
            else
            {
                State.Logger.Log("Reload element color infos failed");
            }
        }

        private void SubscribeToFileChangeNotifier()
        {
            const string jsonFilter = "*.json";

            try
            {
                FileChangeNotifier.StartFileWatch
                (
                    jsonFilter,
                    Paths.ElementColorInfosDirectory,
                    OnElementColorsInfosChanged
                );

                FileChangeNotifier.StartFileWatch
                (
                    Paths.MaterialColorStateFileName,
                    Paths.MaterialConfigPath,
                    OnMaterialStateChanged
                );
            }
            catch (Exception e)
            {
                State.Logger.Log("SubscribeToFileChangeNotifier failed");
                State.Logger.Log(e);
            }
        }
    }
}
