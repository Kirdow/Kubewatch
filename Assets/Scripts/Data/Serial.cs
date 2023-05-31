using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.IO;
using System;
using UnityEngine;

namespace Kubewatch.Data
{
    public static class Serial
    {
        public static bool SerializeObject<T>(T obj, ref string data, string errorText = "Failed to serialize object")
        {
            try
            {
                var serializer = new SerializerBuilder().WithNamingConvention(PascalCaseNamingConvention.Instance).Build();
                
                using (var sw = new StringWriter())
                {
                    serializer.Serialize(sw, obj);
                    string fileData = sw.ToString();

                    data = fileData;
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(errorText);
                Debug.LogException(e);
            }

            return false;
        }

        public static bool DeserializeObject<T>(string data, ref T obj, string errorText = "Failed to deserialize object")
        {
            try
            {
                var deserializer = new DeserializerBuilder().WithNamingConvention(PascalCaseNamingConvention.Instance).Build();
                T result = deserializer.Deserialize<T>(data);

                obj = result;
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(errorText);
                Debug.LogException(e);
            }

            return false;
        }

        public static bool SaveFileString(string filePath, string data, string errorText = "Failed to save file")
        {
            if (data == null) return false;

            try
            {
                if (File.Exists(filePath))
                {
                    var old = $"{filePath}.old";
                    if (File.Exists(old)) File.Delete(old);
                    File.Move(filePath, old);
                }

                File.WriteAllText(filePath, data);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(errorText);
                Debug.LogException(e);
            }

            return false;
        }

        public static bool LoadFileString(string filePath, ref string data, string errorText = "Failed to load file")
        {
            if (!File.Exists(filePath)) return false;

            try
            {
                string fileData = File.ReadAllText(filePath);
                data = fileData;
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(errorText);
                Debug.LogException(e);
            }

            return false;
        }
    }
}