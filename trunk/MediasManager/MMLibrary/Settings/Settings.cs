using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MediaManager.Library;
using MediaManager.Plugins;


namespace MediaManager.Master
{
    public class Settings
    {
        public static String xmlPath;
        public static XmlSettings XML = new XmlSettings();
        public static List<IMMPluginScraper> PluginsScraper = new List<IMMPluginScraper>();
        public static List<IMMPluginImportExport> PluginsImportExport = new List<IMMPluginImportExport>();

        public static bool Save()
        {
            try
            {
                Serializer s = new Serializer(xmlPath, XML);
                return s.ToFile();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Load()
        {

            Serializer s = new Serializer(xmlPath, XML);
            XML = (XmlSettings)s.FromFile();

            if (XML == null)
            {
                XML = new XmlSettings();
                return false;
            }
            return true;
        }
    }
}
