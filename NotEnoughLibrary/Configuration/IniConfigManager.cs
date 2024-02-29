	// Copyright (c) 2020 PshenoDev. See the MIT license for full information

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using NotEnoughLibrary.IO;

namespace NotEnoughLibrary.Configuration
{
    /// <summary>
    /// Класс, работающий с INI-конфигурациями.
    /// </summary>
    public class IniConfigManager<TLookup> : IDisposable
        where TLookup : struct, Enum
    {
        private readonly Dictionary<TLookup, string> keys;

        public virtual string Filename => @"config.ini";

        public string this[TLookup lookup] { get { return keys[lookup]; } set { Set(lookup, value); } }

        public IniConfigManager()
        {
            keys = new Dictionary<TLookup, string>();

            foreach (TLookup lookup in Enum.GetValues(typeof(TLookup)))
            {
                keys.Add(lookup, null);
            }

            InitializeDefaults();
        }

        public virtual void InitializeDefaults()
        {
            
        }

        public void Load()
        {
            if (string.IsNullOrEmpty(Filename)) return;

            using (var reader = new StreamReader(Storage.GetStream(Filename, FileMode.OpenOrCreate)))
            {
                string line = null;

                while ((line = reader.ReadLine()) != null)
                {
                    int equalsIndex = line.IndexOf('=');

                    if (line.Length == 0 || line[0] == '#' || equalsIndex < 0) continue;

                    string key = line.Substring(0, equalsIndex).Trim();
                    string value = line.Substring(equalsIndex + 1).Trim();

                    if (!Enum.TryParse(key, out TLookup lookup)) continue;

                    Set(lookup, value);
                }
            }
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(Filename)) return;

            using (var writer = new StreamWriter(Storage.GetStream(Filename, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                writer.WriteLine("[{0}]", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Path.GetFileNameWithoutExtension(Filename)));

                foreach (var key in keys)
                {
                    writer.WriteLine(@"{0} = {1}", key.Key, key.Value.ToString());
                }
            }
        }

        public void Set(TLookup lookup, object value)
        {
            keys[lookup] = value.ToString();
        }

        public void Dispose()
        {
            Save();
        }
    }
}
