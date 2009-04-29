using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MediaManager.Library.NFO;

namespace MediaManager
{
    public class Settings
    {
        public static String xmlPath;
        public static MediaManager.Config.XmlSettings XML = new MediaManager.Config.XmlSettings();

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
            XML = (Config.XmlSettings)s.FromFile();

            if (XML == null)
            {
                XML = new Config.XmlSettings();
                return false;
            }
            return true;
        }
    }
}
