﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using MessagePack;
using YamlDotNet.Serialization;

namespace MasterConverter
{
    public static class RecordWriter
    {
        //----- params -----

        //----- field -----

        //----- property -----

        //----- method -----

        public static void ExportMessagePack(string exportPath, object[] values, bool lz4Compress, string aesKey)
        {
            var filePath = Path.ChangeExtension(exportPath, Constants.MessagePackMasterFileExtension);

            var json = JsonFx.Json.JsonWriter.Serialize(values);

            var bytes = lz4Compress ? LZ4MessagePackSerializer.FromJson(json) : MessagePackSerializer.FromJson(json);

            #if DEBUG

            Console.WriteLine("Json(LZ4MessagePack) :\n{0}\n", LZ4MessagePackSerializer.ToJson(bytes));

            #endif

            if (!string.IsNullOrEmpty(aesKey))
            {
                var aesManaged = AESExtension.CreateAesManaged(aesKey);

                bytes = bytes.Encrypt(aesManaged);
            }

            CreateFileDirectory(filePath);

            using (var writer = new BinaryWriter(new FileStream(filePath, FileMode.Create)))
            {
                writer.Write(bytes);
            }
        }

        public static void ExportYaml(string exportPath, object[] records)
        {
            var filePath = Path.ChangeExtension(exportPath, Constants.YamlMasterFileExtension);

            var serializer = new SerializerBuilder().Build();

            CreateFileDirectory(filePath);

            using (var writer = new StreamWriter(new FileStream(filePath, FileMode.Create)))
            {
                serializer.Serialize(writer, records);
            }
        }

        public static void ExportYamlRecords(string exportPath, string[] recordNames, object[] records)
        {
            var directory = PathUtility.Combine(Directory.GetParent(exportPath).FullName, Constants.RecordsFolderName);

            if (Directory.Exists(directory))
            {
                DirectoryUtility.Delete(directory);
            }

            Directory.CreateDirectory(directory);

            var serializer = new SerializerBuilder().Build();

            for (var i = 0; i < recordNames.Length; i++)
            {
                if (string.IsNullOrEmpty(recordNames[i])) { continue; }

                var filePath = PathUtility.Combine(directory, recordNames[i] + Constants.RecordFileExtension);

                using (var writer = new StreamWriter(new FileStream(filePath, FileMode.Create)))
                {
                    serializer.Serialize(writer, records[i]);
                }
            }
        }

        private static void CreateFileDirectory(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);

            if (Directory.Exists(directory)) { return; }
            
            Directory.CreateDirectory(directory);            
        }
    }
}
