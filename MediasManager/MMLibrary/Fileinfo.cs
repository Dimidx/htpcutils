using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MediaManager.Library

{   
    /// <summary>
    /// Informations concernant le fichier vidéo
    /// </summary>
    [Serializable]
    [XmlRoot("fileinfo")]
    public class Fileinfo : INotifyPropertyChanged
    {
        private StreamDetails _StreamDetails;
        /// <summary>
        /// Informations du stream du fichier
        /// </summary>

        public StreamDetails StreamDetails
        {
            get { return _StreamDetails; }
            set { _StreamDetails = value; OnPropertyChanged("StreamDetails"); }
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

    #region StreamDetails
    [Serializable]
    [XmlRoot("streamdetails")]
    public class StreamDetails : INotifyPropertyChanged
    {

        private List<Video> _Video;
        private List<Audio> _Audio;
        private List<Subtitle> _Subtitle;

        /// <summary>
        /// Informations du stream video du fichier
        /// </summary>
        public List<Video> Video
        {
            get { return _Video; }
            set { _Video = value; OnPropertyChanged("Video"); }
        }

        /// <summary>
        /// Informations du stream audio du fichier
        /// </summary>
        public List<Audio> Audio
        {
            get { return _Audio; }
            set { _Audio = value; OnPropertyChanged("Audio"); }
        }

        /// <summary>
        /// Informations des Sous-Titres du fichier
        /// </summary>
        public List<Subtitle> Subtitle
        {
            get { return _Subtitle; }
            set { _Subtitle = value; OnPropertyChanged("Subtitle"); }
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

    #endregion

    #region Video
    [Serializable]
    [XmlRoot("video")]
    public class Video : INotifyPropertyChanged
    {
        private string _width;
        private string _height;
        private string _codec;
        private string _formatinfo;
        private string _duration;
        private string _bitrate;
        private string _bitratemode;
        private string _bitratemax;
        private string _codecid;
        private string _codecidinfo;
        private string _scantype;
        private string _aspectdisplayratio;

        /// <summary>
        /// Largeur
        /// </summary>
        [XmlElement("width")]
        public string Width
        {
            get { return _width; }
            set { _width = value; OnPropertyChanged("Width"); }
        }

        /// <summary>
        /// Hauteur
        /// </summary>
        [XmlElement("height")]
        public string Height
        {
            get { return _height; }
            set { _height = value; OnPropertyChanged("Height"); }
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
    #endregion

    [Serializable]
    [XmlRoot("audio")]
    public class Audio : INotifyPropertyChanged
    {

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

    [Serializable]
    [XmlRoot("subtitle")]
    public class Subtitle : INotifyPropertyChanged
    {

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
