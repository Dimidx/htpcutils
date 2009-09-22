using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using MediaManager.Library;
using MediaManager.Plugins;

namespace MediaManager.Master
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
    private ConfigMovie _confMovie = new ConfigMovie();
    private ConfigProxy _confProxy = new ConfigProxy();


    [XmlElement(ElementName = "movies")]
    public ConfigMovie confMovie
    {
        get { return _confMovie; }
        set { _confMovie = value; }
    }

    [XmlElement(ElementName = "proxy")]
    public ConfigProxy confProxy
    {
        get { return _confProxy; }
        set { _confProxy = value; }
    }

}

public class ConfigProxy
{
    private bool _UseProxy = false;
    private string _ProxyAdress = "";
    private string _ProxyPort = "";
    private string _ProxyUser = "";
    private string _ProxyPassword = "";
    private string _ProxyDomain = "";

    public bool UseProxy
    {
        get { return _UseProxy; }
        set { _UseProxy = value; }
    }

    public string ProxyAdress
    {
        get { return _ProxyAdress; }
        set { _ProxyAdress = value; }
    }

    public string ProxyPort
    {
        get { return _ProxyPort; }
        set { _ProxyPort = value; }
    }
    
    public string ProxyUser
    {
        get { return _ProxyUser; }
        set { _ProxyUser = value; }
    }

    public string ProxyPassword
    {
        get { return _ProxyPassword; }
        set { _ProxyPassword = value; }
    }

    public string ProxyDomain
    {
        get { return _ProxyDomain; }
        set { _ProxyDomain = value; }
    }

}


public class ConfigMovie
{

    private ObservableCollection<MovieFolder> _MovieFolders;
    private string[] _Extensions = { "*.mkv", "*.mp4", "*.avi", "*.wmv", "*.rar", "*.ifo", "*.iso", "*.img" };

    [XmlElement(ElementName = "extensions")]
    public string[] Extensions
    {
        get { return _Extensions; }
        set { _Extensions = value; }
    }

    [XmlIgnore]
    public char[] split = { ',', ';' };

    [XmlElement(ElementName = "folder")]
    public ObservableCollection<MovieFolder> MovieFolders
    {
        get { return _MovieFolders; }
        set { _MovieFolders = value; }
    }
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
