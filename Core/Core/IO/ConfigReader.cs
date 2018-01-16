namespace Core.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;


    public class ConfigReader
    {
        private Dictionary<string, string> configValues;

        public void ReadFile(string _path) {
            this.configValues = new Dictionary<string, string>();
            string[] mContent = File.ReadAllLines(_path);
            foreach(string sValue in mContent) {
                if (sValue.Contains("=")) {
                    string[] mValue = sValue.Split('=');
                    if (mValue.Length == 2) {
                        configValues.Add(mValue[0], mValue[1]);
                    }
                }
            }
        }

        public string GetValue(string key)
        {
            if (configValues.ContainsKey(key))
                return configValues[key];
            else
                return "NULL";
        }
    }
}
