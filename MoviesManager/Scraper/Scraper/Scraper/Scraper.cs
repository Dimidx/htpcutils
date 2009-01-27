using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;


namespace Scraper
{
    public class Plugin
    {
        public string ClassName;
        public string Path;
        public string Author;
        public string Version;
        public string PluginName;


        private ScraperPlugin _plugInterface;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="_path"></param>
        public Plugin()
        {
            

        }


        public bool Load(string _path)
        {
            this.Path = _path;
            this.ClassName = Scraper.ExamineAssembly(_path);
            if (this.ClassName == null) return false;

            _plugInterface = (ScraperPlugin)Scraper.CreateInstance(this);
            if (_plugInterface == null) return false;

            this.Author = _plugInterface.Author;
            this.PluginName = _plugInterface.PluginName;
            this.Version = _plugInterface.Version;
            return true;
        }


        public Movie[] SearchMovie(string MovieName)
        {
            Movie[] _Result = _plugInterface.SearchMovie(MovieName);
            return _Result;
        }

        public Movie GetMovie(string ID)
        {
            Movie _Result = _plugInterface.GetMovie(ID);
            return _Result;
        }

        //string test = titi.PluginName;
    }

    public interface ScraperPlugin
    {
        string PluginName { get; }
        string Author { get; }
        string Version { get; }

        Movie[] SearchMovie(string MovieName);
        Movie GetMovie(string ID);

    }

    public class Scraper
    {

        public static object CreateInstance(Plugin Plugin)
        {
            Assembly objDLL;
            object objPlugin;
            try
            {
                // Load dll
                objDLL = Assembly.LoadFrom(Plugin.Path);
                
                //Cherche le nom de la classe
                //Plugin.ClassName = 
                
                // Create and return class instance
                objPlugin = objDLL.CreateInstance(Plugin.ClassName);

            }
            catch
            {
                return null;
            }
            return objPlugin;
        }
        /// <summary>
        /// Renvoie le nom de la classe de la dll
        /// </summary>
        /// <param name="_path"></param>
        /// <returns></returns>
        public static string ExamineAssembly(string _path)
        {
            Assembly objDLL;
            objDLL = Assembly.LoadFrom(_path);

            Type objInterface;

            //ailablePlugin Plugin;
            // Loop through each type in the DLL
            foreach (System.Type objType in objDLL.GetTypes())
            {
                // Only look at public types
                if ((objType.IsPublic == true))
                {
                    // Ignore abstract classes
                    if ((objType.Attributes & TypeAttributes.Abstract)
                                != TypeAttributes.Abstract)
                    {
                        // See if this type implements our interface
                        objInterface = objType.GetInterface("ScraperPlugin", true);

                        if (!(objInterface == null))
                        {
                            // It does
                            //Plugin _plug = new Plugin();
                            //_plug.Path = objDLL.Location;
                            //_plug.Name = objType.FullName;
                            //_plug.ClassName = objType.Name;
                            return objType.FullName;

                        }
                    }
                }
            }
            return null;
        }


    }


}
