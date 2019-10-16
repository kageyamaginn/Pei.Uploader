using Pei.BolUploader.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Pei.BolUploader.Entities
{
    public static class BolItemExt
    {
        public static void Save(this BolItem item)
        {
            string dicPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            string filePath = Path.Combine(dicPath, String.Format("{0}.cache", item.BolNumber));
            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.Write(JsonConvert.SerializeObject(item));
            }

        }

        public static BolItem Get(string bolNumber)
        {
            string dicPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            string filePath = Path.Combine(dicPath, String.Format("{0}.cache", bolNumber));
            BolItem result = null;
            using (StreamReader sr = new StreamReader(filePath))
            {
                result = JsonConvert.DeserializeObject<BolItem>(sr.ReadToEnd());
            }
            return result;
        }


    }


}
