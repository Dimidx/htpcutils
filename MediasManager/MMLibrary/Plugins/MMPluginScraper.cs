using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.ComponentModel;

using MediaManager.Library;

namespace MediaManager.Plugins
{
    public class MMPluginScraper
    {
        private static IMMPluginScraper instancePlugin;
        private string _CheminConfig;
        public MMPluginScraper(string _filename)
        {

            Assembly PluginAssembly;
            FileInfo PluginFile = new FileInfo(_filename);
            PluginAssembly = Assembly.LoadFrom(PluginFile.FullName);

            instancePlugin = PluginAssembly.CreateInstance("MediaManager.Plugins." + PluginFile.Name.Substring(0
                                                     , PluginFile.Name.Length - 4)) as IMMPluginScraper;

            _CheminConfig = PluginFile.FullName.Replace("dll", "xml");

        }

        public string URL { get { return instancePlugin.URL; } }
        public string Name { get { return instancePlugin.Name; } }
        public string Author { get { return instancePlugin.Author; } }
        public string Version { get { return instancePlugin.Version; } }
        public string Description { get { return instancePlugin.Description; } }
        //public List<MMPluginOption> Options { get { return instancePlugin.GetOptions(); } }

        public void LoadOptions()
        {
           
        }

        public void SaveOptions()
        {

            //if (File.Exists(_CheminConfig) != true ) File.Create(_CheminConfig);
            //TextWriter w = new StreamWriter(@_CheminConfig);
            //XmlSerializer xmlSerial = new XmlSerializer(typeof(List<MMPluginOption>));
            //xmlSerial.Serialize(w, Options);
            //w.Close();
            
            

        }



    }
}
