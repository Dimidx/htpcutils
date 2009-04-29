using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// <summary>
    /// Sauve les fichiers avec le nom de la video
    /// </summary>
    public bool saveAsMovie = true;

    /// <summary>
    /// Sauve le fanart sous la forme fanart.jpg
    /// </summary>
    public bool saveFanartJpg = true;
    
    /// <summary>
    /// Sauve le poster en folder.jpg
    /// </summary>
    public bool saveFolderJpg = true;

    /// <summary>
    /// Sauve le fanart sous la forme moviename-fanart.jpg
    /// </summary>
    public bool saveMovieNameFanart = true;

    /// <summary>
    /// Sauve le fanart sous la forme moviename.tbn
    /// </summary>
    public bool saveMovieNameTbn = true;

    /// <summary>
    /// Sauve le fanart sous la forme movie.tbn
    /// </summary>
    public bool saveMovieTbn = true;

    



    public bool useFolderForSearch = true;
    public bool cleanFilename = true;
    public bool skipSample = true;
    public bool overwriteNfo = false;
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



    public bool monitorFolder = false;
    public bool autoScrape = false;
    public override string ToString()
    {
        return path;
    }
}
