namespace ONI_Common.IO
{
    using System;
    using System.IO;

    public class Logger
    {
        private readonly string _filePath;
        private StreamWriter writer;

        public Logger(string filePath)
        {
            this._filePath = filePath;

            FileInfo fileInfo = new FileInfo(this._filePath);
            if (!fileInfo.Exists)
                Directory.CreateDirectory(fileInfo.Directory.FullName);

            this.writer = new StreamWriter(this._filePath, true);
        }

        public void Log(string message)
        {
            //FileStream fileStream = new FileStream(this._filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);

            //using (StreamWriter writer = new StreamWriter(this._filePath, true))
            {
                DateTime now = System.DateTime.Now;

                Debug.Log($"[{now.ToShortDateString()}, {now.TimeOfDay}] {message}");   // Also dump to main log
                writer.WriteLine($"[{now.ToShortDateString()}, {now.TimeOfDay}] {message}\r\n");
                //writer.Close();
                writer.Flush();
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