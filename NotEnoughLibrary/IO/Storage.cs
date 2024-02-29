// Copyright (c) 2020 PshenoDev. See the MIT license for full information

using System.IO;

namespace NotEnoughLibrary.IO
{
    public class Storage
    {
        public const string CONFIGURATION_FOLDER_NAME = @"Configurations";
        public const string DATA_FOLDER_NAME = @"Data";
        public const string LOG_FOLDER_NAME = @"Logs";
        public const string SHADERS_FOLDER_NAME = @"Shaders";

        public static Stream GetStream(string filename, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read)
        {
            string directoryName = Directory.GetCurrentDirectory();

            if (filename.EndsWith(".ini"))
            {
                directoryName = CONFIGURATION_FOLDER_NAME;
            }

            if (filename.EndsWith(".obj"))
            {
                directoryName = Path.Combine(DATA_FOLDER_NAME, "Models");
            }

            if (filename.EndsWith(".jpg"))
            {
                directoryName = Path.Combine(DATA_FOLDER_NAME, "Textures");
            }

            if (filename.EndsWith(".log"))
            {
                directoryName = LOG_FOLDER_NAME;
            }

            if (filename.EndsWith(".frag") || filename.EndsWith(".vert"))
            {
                directoryName = SHADERS_FOLDER_NAME;
            }

            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            return new FileStream(Path.Combine(directoryName, filename), mode, access);
        }

        public static string ReadToEnd(string filename, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read)
        {
            using (var reader = new StreamReader(GetStream(filename, mode, access)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}