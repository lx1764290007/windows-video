using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace Storage
{
    internal class LocalStorage
    {

        static string path = "./Data.json";
        static string folder = System.Environment.CurrentDirectory + "\\Data.json";
        public static string GetPath()
        {
            return folder;
        }
        public static string  LoadFile()
        {
            
            string json;
            try
            {
                json = File.ReadAllText(path, Encoding.UTF8);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                Environment.Exit(1);
                throw ex;
            }
            return json;
        }
        public static JObject GetObject()
        {
            string text = LoadFile();
            JObject source = JObject.Parse(text);
            return source;
        }
        public static void SaveJson(ArrayList arrayList) 
        {
            JObject list = new JObject(new JProperty("source", new JArray(arrayList)));
            string convertString = Convert.ToString(list);
            File.WriteAllText(folder, convertString);
        }
    }
}
