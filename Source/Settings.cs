using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Extensions;

namespace MasterConverter
{
    public class Settings
    {
        //----- params -----
        
        public class MasterSettings
        {
            /// <summary> マスター：タグ定義行 </summary>
            public int tagRow = 2;
            /// <summary> マスター：型名定義名行 </summary>
            public int dataTypeRow = 3;
            /// <summary> マスター：フィールド名定義行 </summary>
            public int fieldNameRow = 4;
            /// <summary> マスター：レコード開始行 </summary>
            public int recordStartRow = 5;
        }

        public class ExportSettings
        {
            public string AESKey = string.Empty;
            public string AESIv = string.Empty;
            public bool lz4compress = true;
        }

        //----- field -----

        //----- property -----

        public MasterSettings Master { get; private set; }

        public ExportSettings Export { get; private set; }

        //----- method -----        

        public Settings()
        {
            var iniFilePath = "./settings.ini";

#if false
            Master = IniFile.Read<MasterSettings>("Master", iniFilePath);
            Export = IniFile.Read<ExportSettings>("Export", iniFilePath);
#else
            Master = SimpleIniFileParser.Read<MasterSettings>("Master", iniFilePath);
            Export = SimpleIniFileParser.Read<ExportSettings>("Export", iniFilePath);
#endif
        }
    }
}


public static class SimpleIniFileParser
{
    public static T Read<T>(string section, string filepath)
    {
        T ret = (T)Activator.CreateInstance(typeof(T));
        var lines = File.ReadAllLines(filepath);

        foreach (var n in typeof(T).GetFields())
        {
            try
            {
                if (n.FieldType == typeof(int))
                {
                    var x = lines.FirstOrDefault(s => { return s.StartsWith(n.Name); });
                    n.SetValue(ret, int.Parse(x.Split('=')[1].Trim()));
                }
                else if (n.FieldType == typeof(uint))
                {
                    var x = lines.FirstOrDefault(s => { return s.StartsWith(n.Name); });
                    n.SetValue(ret, uint.Parse(x.Split('=')[1].Trim()));
                }
                else if (n.FieldType == typeof(bool))
                {
                    var x = lines.FirstOrDefault(s => { return s.StartsWith(n.Name); });
                    n.SetValue(ret, bool.Parse(x.Split('=')[1].Trim()));
                }
                else
                {
                    var x = lines.FirstOrDefault(s => { return s.StartsWith(n.Name); });
                    n.SetValue(ret, x.Split('=')[1].Trim());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        };

        return ret;
    }
}