using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XBMC
{
    [XmlRoot(
    ElementName = "episodedetails",
    DataType = "Name"
)]
    public class NfoTV
    {
        private String title;
        private String season;
        private String episode;
        private String plot;
        private String thumb;
        private Boolean watched;
        private String credits;
        private String director;
        private String aired;

        [XmlElement(ElementName = "title")]
        public String Title
        {
            get { return title; }
            set { title = value; }
        }

        [XmlElement(ElementName = "season")]
        public String Season
        {
            get { return season; }
            set { season = value; }
        }

        [XmlElement(ElementName = "episode")]
        public String Episode
        {
            get { return episode; }
            set { episode = value; }
        }

        [XmlElement(ElementName = "plot")]
        public String Plot
        {
            get { return plot; }
            set { plot = value; }
        }

        [XmlElement(ElementName = "thumb")]
        public String Thumb
        {
            get { return thumb; }
            set { thumb = value; }
        }

        [XmlElement(ElementName = "watched")]
        public Boolean Watched
        {
            get { return watched; }
            set { watched = value; }
        }

        [XmlElement(ElementName = "credits")]
        public String Credits
        {
            get { return credits; }
            set { credits = value; }
        }

        [XmlElement(ElementName = "director")]
        public String Director
        {
            get { return director; }
            set { director = value; }
        }

        [XmlElement(ElementName = "aired")]
        public String Aired
        {
            get { return aired; }
            set { aired = value; }
        }
    }
}
