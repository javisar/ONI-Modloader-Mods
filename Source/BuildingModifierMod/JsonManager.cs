namespace BuildingModifierMod
{
    using Newtonsoft.Json;
    using System;
    using System.IO;

    public class JsonManager
    {
        public JsonSerializer Serializer =
        JsonSerializer.CreateDefault(new JsonSerializerSettings { Formatting = Formatting.Indented, ObjectCreationHandling = ObjectCreationHandling.Replace });

        public T Deserialize<T>(string path)
        {
            T result;

            using (StreamReader streamReader = new StreamReader(path))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
                {
                    result = this.Serializer.Deserialize<T>(jsonReader);

                    jsonReader.Close();
                }

                streamReader.Close();
            }

            return result;
        }
        
        public void Serialize<T>(T value, string path)
        {
            using (StreamWriter streamReader = new StreamWriter(path))
            {
                using (JsonTextWriter jsonReader = new JsonTextWriter(streamReader))
                {
                    this.Serializer.Serialize(jsonReader, value);

                    jsonReader.Close();
                }

                streamReader.Close();
            }
        }

        public bool TryLoadConfiguration<T>(string path, out T state)
        {
            try
            {
                state = Deserialize<T>(path);
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
                Serialize<T>(state, path);
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