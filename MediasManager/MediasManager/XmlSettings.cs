using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using MediaManager.Library.NFO;

namespace MediaManager.Config
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
        public MainS Main = new MainS();

        [XmlElement(ElementName = "config")]
        public ConfigS Config = new ConfigS();
    }

    /***************
     * 
     * Main Settings
     * 
     **************/
}

public class MainS
{
    public int top = 165;
    public int left = 282;
    public int width  = 852;
    public int height = 576;
    public bool fullscreen = false;

    [XmlElement(ElementName = "movielistview")]
    public MainMovies movies = new MainMovies();

    [XmlElement(ElementName = "movienfoeditor")]
    public NfoConfigMovie NfoConfMovie = new NfoConfigMovie();
}

public class NfoConfigMovie
{
    public int top = 200;
    public int left = 200;
}

public class MainMovies
{
    public int col1Width = 240;
    public int col2Width = 280;
    public int col3Width = 60;
    public int col4Width = 45;
    public int col5Width = 45;
    public int col6Width = 45;
    public int col7Width = 80;
    public int selectedFilterSize = 4;
    public int selectedFilterNfo = 7;
}

public class ConfigS
{
    [XmlElement(ElementName = "movies")]
    public ConfigMovie confMovie = new ConfigMovie();
}

public class ConfigMovie
{
    public int top = 200;
    public int left = 200;
    public String lastFolder;
    public int maxDownloading = 5;
    public int maxRetry = 1;
    public bool saveAsMovie = true;
    public bool saveFanartJpg = true;
    public bool savePosterJpg = true;
    public bool saveFolderJpg = true;
    public bool useFolderForSearch = true;
    public bool cleanFilename = true;
    public bool skipSample = true;
    public bool overwriteNfo = false;
    public String[] extensions = { "*.mkv", "*.mp4", "*.avi", "*.wmv", "*.rar", "*.ifo", "*.iso", "*.img" };
    [XmlIgnore]
    public char[] split = { ',', ';' };

    [XmlElement(ElementName = "folder")]
    public MovieFolder[] MovieFolders;
}

public class MovieFolder
{
    public String path = "";
    public bool containsFolders = true;
    public bool monitorFolder = false;
    public bool autoScrape = false;
    public override string ToString()
    {
        return path;
    }
}
