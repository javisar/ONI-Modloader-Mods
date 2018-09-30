namespace ONI_Common.IO
{
    using System;
    using System.IO;

    public class Logger
    {
        private readonly string _filePath;

        public Logger(string filePath)
        {
            this._filePath = filePath;
        }

        public void Log(string message)
        {            
            
            using (StreamWriter writer = new StreamWriter(this._filePath, true))
            {
                DateTime now = System.DateTime.Now;

                Debug.Log($"[{now.ToShortDateString()}, {now.TimeOfDay}] {message}");   // Also dump to main log
                writer.WriteLine($"[{now.ToShortDateString()}, {now.TimeOfDay}] {message}\r\n");
                writer.Close();
            }
        }

        public void Log(Exception exception)
        {
            this.Log($"{exception?.Message}\n{exception?.StackTrace}");
        }

        // public void LogProperties(object target)
        // {
        // var builder = new StringBuilder();
        // foreach (var property in target.GetType().GetProperties())
        // {
        // builder.Append(property.Name);
        // builder.Append(":");
        // builder.Append(property.GetValue(target));
        // builder.Append("\n");
        // }
        // Log(builder.ToString());
    }
}