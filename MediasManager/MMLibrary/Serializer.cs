﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace MediaManager.Library
{
    public class Serializer
    {
        String path;
        Object classType;
        XmlSerializer xmlSerial;
        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

        public Serializer(String xmlPath, Object toClass)
        {
            try
            {
                path = xmlPath;
                ns.Add("", "");
                classType = toClass;
                xmlSerial = new XmlSerializer(toClass.GetType());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Charge un fichier NFO
        /// </summary>
        /// <returns></returns>
        public Object FromFile()
        {
            if (!File.Exists(path)) return null;
            try
            {
                TextReader r = new StreamReader(path);
                Object obj = xmlSerial.Deserialize(r);
                r.Close();
                return obj;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR in: " + path + e.Message);
                return null;
            }
        }

        public bool ToFile()
        {
            try
            {
                TextWriter w = new StreamWriter(@path);
                xmlSerial.Serialize(w, classType, ns);
                w.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR in: " + path);
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }
    }
}
