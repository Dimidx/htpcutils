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
        private String title = "";
        private String rating = "";
        private String year = "";
        private String top250 = "";
        private String votes = "";
        private String outline = "";
        private String plot = "";
        private String tagline = "";
        private String runtime = "";
        private String thumb = "";
        private String mpaa = "";
        private String playcount = "";
        private String id = "";
        private String genre = "";
        private String credits = "";
        private String director = "";
        private String premiered = "";
        private String studio = "";
        private String trailer = "";
        private String alloid = "";
        private string certification = "";
        private string _OriginalTitle = "";
        private List<Actor> actors  = new List<Actor>();

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

        [XmlElement(ElementName = "thumb")]
        public String Thumb
        {
            get { return thumb; }
            set { thumb = value; OnPropertyChanged("Thumb"); }
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