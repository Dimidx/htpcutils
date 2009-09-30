using System;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;

namespace XBMC
{
    [XmlRoot(
    ElementName = "movie",
    DataType = "Name"
)]
    public class NfoMovie : INotifyPropertyChanged
    {
        private string title = "";
        private string rating = "";
        private string year = "";
        private string top250 = "";
        private string votes = "";
        private string outline = "";
        private string plot = "";
        private string tagline = "";
        private string runtime = "";
        private string mpaa = "";
        private string playcount = "";
        private string id = "";
        private string genre = "";
        private string credits = "";
        private string director = "";
        private string premiered = "";
        private string studio = "";
        private string trailer = "";
        private string alloid = "";
        private string certification = "";
        private string _OriginalTitle = "";
        private List<Actor> actors = new List<Actor>();

        private string[] _thumbs;// = new string[1];
        private string[] _thumbsSVN;// = new string[1];
        private string[] _fanart;// = new List<ThumbFanart>();

        [XmlElement(ElementName = "title")]
        public String Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged("Title"); }
        }

        [XmlElement(ElementName = "originaltitle")]
        public String OriginalTitle
        {
            get { return _OriginalTitle; }
            set { _OriginalTitle = value; OnPropertyChanged("OriginalTitle"); }
        }

        [XmlElement(ElementName = "alloid")]
        public String AlloId
        {
            get { return alloid; }
            set { alloid = value; OnPropertyChanged("AlloId"); }
        }

        [XmlElement(ElementName = "rating")]
        public String Rating
        {
            get { return rating; }
            set { rating = value; OnPropertyChanged("Rating"); }
        }

        [XmlElement(ElementName = "year")]
        public String Year
        {
            get { return year; }
            set { year = value; OnPropertyChanged("Year"); }
        }

        [XmlElement(ElementName = "top250")]
        public String Top250
        {
            get { return top250; }
            set { top250 = value; OnPropertyChanged("Top250"); }
        }

        [XmlElement(ElementName = "votes")]
        public String Votes
        {
            get { return votes; }
            set { votes = value; OnPropertyChanged("Votes"); }
        }

        [XmlElement(ElementName = "outline")]
        public String Outline
        {
            get { return outline; }
            set { outline = value; OnPropertyChanged("Outline"); }
        }

        [XmlElement(ElementName = "plot")]
        public String Plot
        {
            get { return plot; }
            set { plot = value; OnPropertyChanged("Plot"); }
        }

        [XmlElement(ElementName = "tagline")]
        public String Tagline
        {
            get { return tagline; }
            set { tagline = value; OnPropertyChanged("Tagline"); }
        }

        [XmlElement(ElementName = "runtime")]
        public String Runtime
        {
            get { return runtime; }
            set { runtime = value; OnPropertyChanged("Runtime"); }
        }

        [XmlArray(ElementName = "thumbs")]
        [XmlArrayItem(ElementName = "thumb")]
         //[XmlElement(ElementName = "thumb")]
        public string[] Thumb
        {
            get
            {
                return _thumbs;
            }
            set
            {
                _thumbs = value; OnPropertyChanged("Thumb");
            }
        }
        //Pour les version SVN
        [XmlElement(ElementName = "thumb")]
        public string[] ThumbSVN
        {
            get
            {
                return _thumbsSVN;
            }

            set
            {
                _thumbsSVN = value; OnPropertyChanged("ThumbSVN");
            }
        }

        [XmlArray(ElementName="fanart")]
        [XmlArrayItem(ElementName = "thumb")]
        //[XmlElement(ElementName = "fanart")]
        //[XmlIgnore]
        public string[] Fanart
        {
            get
            {
                return _fanart;
            }
            set
            {
                _fanart = value;
                OnPropertyChanged("Fanart");
            }
        }


        [XmlElement(ElementName = "certification")]
        public String Certification
        {
            get { return certification; }
            set { certification = value; OnPropertyChanged("Certification"); }
        }

        [XmlElement(ElementName = "mpaa")]
        public String Mpaa
        {
            get { return mpaa; }
            set { mpaa = value; OnPropertyChanged("Mpaa"); }
        }

        [XmlElement(ElementName = "playcount")]
        public String Playcount
        {
            get { return playcount; }
            set { playcount = value; OnPropertyChanged("Playcount"); }
        }

        [XmlElement(ElementName = "id")]
        public String Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }



        [XmlElement(ElementName = "genre")]
        public String Genre
        {
            get { return genre; }
            set { genre = value; OnPropertyChanged("Genre"); }
        }

        [XmlElement(ElementName = "credits")]
        public String Credits
        {
            get { return credits; }
            set { credits = value; OnPropertyChanged("Credits"); }
        }

        [XmlElement(ElementName = "director")]
        public String Director
        {
            get { return director; }
            set { director = value; OnPropertyChanged("Director"); }
        }

        [XmlElement(ElementName = "premiered")]
        public String Premiered
        {
            get { return premiered; }
            set { premiered = value; OnPropertyChanged("Premiered"); }
        }

        [XmlElement(ElementName = "studio")]
        public String Studio
        {
            get
            {
                return studio;

            }
            set { studio = value; OnPropertyChanged("Studio"); }
        }

        [XmlElement(ElementName = "trailer")]
        public String Trailer
        {
            get { return trailer; }
            set { trailer = value; OnPropertyChanged("Trailer"); }
        }

        [XmlElement(ElementName = "actor")]
        public List<Actor> Actor
        {
            get
            {
                //Actor[] act = new Actor[actors.Count];
                //actors.CopyTo(act);
                return actors;
            }
            set
            {
                if (value == null) return;
                //Actor[] newActors = (Actor[])value;
                actors.Clear();
                foreach (Actor newActor in value) actors.Add(newActor);
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}

//Classe pour les thumbs babylon
public class ThumbFanart : INotifyPropertyChanged
{
    private String _thumb;

    [XmlElement(ElementName = "thumb")]
    public String Thumb
    {
        get { return _thumb; }
        set { _thumb = value; OnPropertyChanged("Thumb"); }
    }

    public ThumbFanart(string _url)
    {
        _thumb = _url;
    }

    public ThumbFanart()
    {
        
    }


    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    #endregion


}

public class Actor : INotifyPropertyChanged
{
    private String name;
    private String role;
    private String thumb;

    [XmlElement(ElementName = "name")]
    public String Name
    {
        get { return name; }
        set { name = value; OnPropertyChanged("Name"); }
    }

    [XmlElement(ElementName = "role")]
    public String Role
    {
        get { return role; }
        set { role = value; OnPropertyChanged("Role"); }
    }

    [XmlElement(ElementName = "thumb")]
    public String Thumb
    {
        get { return thumb; }
        set { thumb = value; OnPropertyChanged("Thumb"); }
    }

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    #endregion
}