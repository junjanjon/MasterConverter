﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            Master = IniFile.Read<MasterSettings>("Master", iniFilePath);
            Export = IniFile.Read<ExportSettings>("Export", iniFilePath);
        }
    }
}
