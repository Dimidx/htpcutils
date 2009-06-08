using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using MediaManager.Library;

namespace MediaManager.Configuration
{
    /*************
     * 
     * XMLSettings
     * 
     *************/

    [XmlRoot(
    ElementName = "settings",
    DataType = "Name")]
    public class XmlSettings
    {
        [XmlElement(ElementName = "main")]
        public Main Main = new Main();

        [XmlElement(ElementName = "config")]
        public Config Config = new Config();
    }

    /***************
     * 
     * Main Settings
     * 
     **************/
}

public class Main
{

}


public class Config
{
    [XmlElement(ElementName = "movies")]
    public ConfigMovie confMovie = new ConfigMovie();
}

public class ConfigMovie
{

    public String[] extensions = { "*.mkv", "*.mp4", "*.avi", "*.wmv", "*.rar", "*.ifo", "*.iso", "*.img" };
    [XmlIgnore]
    public char[] split = { ',', ';' };

    [XmlElement(ElementName = "folder")]

    public ObservableCollection<MovieFolder> MovieFolders;
}

public class MovieFolder
{
    private String _path = "";
    public String path
    {
        get { return _path; }
        set { _path = value; }
    }

    private bool _containsFolders = true;
    public bool containsFolders
    {
        get { return _containsFolders; }
        set { _containsFolders = value; }
    }

    public override string ToString()
    {
        return path;
    }
}
