using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Text;

namespace XBMC
{
    /// <summary>
    /// Artiste dans la Base de données
    /// </summary>
    public class MusicArtist : INotifyPropertyChanged
    {
        #region Private

        private string _Artist;
        private int _IdArtist;
        private BitmapImage _Thumb;
        private BitmapImage _Fanart;
        #endregion

        #region Public
        /// <summary>
        /// Nom de l'artiste
        /// </summary>
        public string Artist
        {
            get { return _Artist; }
            set { _Artist = value; OnPropertyChanged("Artist"); }
        }

        /// <summary>
        /// ID de l'artiste dans la BD
        /// </summary>
        public int IdArtist
        {
            get { return _IdArtist; }
            set { _IdArtist = value; OnPropertyChanged("IdArtist"); }
        }

        /// <summary>
        /// Photo de l'artiste
        /// </summary>
        public BitmapImage Thumb
        {
            get { return _Thumb; }
            set { _Thumb = value; _Thumb.Freeze(); OnPropertyChanged("Thumb"); }
        }

        /// <summary>
        /// Fanart de l'artiste
        /// </summary>
        public BitmapImage Fanart
        {
            get { return _Fanart; }
            set { _Fanart = value; _Fanart.Freeze(); OnPropertyChanged("Fanart"); }
        } 
        #endregion

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

    /// <summary>
    /// Album dans la Base de données
    /// </summary>
    public class MusicAlbum : INotifyPropertyChanged
    {
        #region Private

        private string _Album;
        private int _IdAlbum;
        private MusicArtist _Artist;
        private int _Year;
        private string _Genre;
        private BitmapImage _Thumb;

        #endregion

        #region Public
        /// <summary>
        /// Nom de l'album
        /// </summary>
        public string Album
        {
            get { return _Album; }
            set { _Album = value; OnPropertyChanged("Album"); }
        }

        /// <summary>
        /// ID de l'album dans la BD
        /// </summary>
        public int IdAlbum
        {
            get { return _IdAlbum; }
            set { _IdAlbum = value; OnPropertyChanged("IdAlbum"); }
        }

        /// <summary>
        /// Artiste
        /// </summary>
        public MusicArtist Artist
        {
            get { return _Artist; }
            set { _Artist = value; OnPropertyChanged("Artist"); }
        }

        /// <summary>
        /// Genre
        /// </summary>
        public string Genre
        {
            get { return _Genre; }
            set { _Genre = value; OnPropertyChanged("Genre"); }
        }

        /// <summary>
        /// Année
        /// </summary>
        public int Year
        {
            get { return _Year; }
            set { _Year = value; OnPropertyChanged("Year"); }
        }

        /// <summary>
        /// Pochette de l'album
        /// </summary>
        public BitmapImage Thumb
        {
            get { return _Thumb; }
            set { _Thumb = value; _Thumb.Freeze(); OnPropertyChanged("Thumb"); }
        }

        #endregion

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

    /// <summary>
    /// Titre dans la Base de données
    /// </summary>
    public class MusicSong : INotifyPropertyChanged
    {
        #region Private

        private string _Title;
        private int _IdSong;
        private MusicArtist _Artist;
        private MusicAlbum _Album;
        private int _Year;
        private int _Track;
        private int _Duration;
        private string _Genre;
        private string _Filename;
        private string _Path;
        private BitmapImage _Thumb;

        #endregion

        #region Public
        /// <summary>
        /// Titre de la piste
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; OnPropertyChanged("Title"); }
        }

        /// <summary>
        /// ID de la piste dans la BD
        /// </summary>
        public int IdSong
        {
            get { return _IdSong; }
            set { _IdSong = value; OnPropertyChanged("IdSong"); }
        }

        /// <summary>
        /// Artiste
        /// </summary>
        public MusicArtist Artist
        {
            get { return _Artist; }
            set { _Artist = value; OnPropertyChanged("Artist"); }
        }

        /// <summary>
        /// Album
        /// </summary>
        public MusicAlbum Album
        {
            get { return _Album; }
            set { _Album = value; OnPropertyChanged("Album"); }
        }

        /// <summary>
        /// Genre
        /// </summary>
        public string Genre
        {
            get { return _Genre; }
            set { _Genre = value; OnPropertyChanged("Genre"); }
        }

        /// <summary>
        /// Année
        /// </summary>
        public int Year
        {
            get { return _Year; }
            set { _Year = value; OnPropertyChanged("Year"); }
        }

        /// <summary>
        /// N° Piste
        /// </summary>
        public int Track
        {
            get { return _Track; }
            set { _Track = value; OnPropertyChanged("Track"); }
        }

        /// <summary>
        /// Durée
        /// </summary>
        public int Duration
        {
            get { return _Duration; }
            set { _Duration = value; OnPropertyChanged("Duration"); }
        }

        /// <summary>
        /// Nom du fichier
        /// </summary>
        public string Filename
        {
            get { return _Filename; }
            set { _Filename = value; OnPropertyChanged("Filename"); }
        }

        /// <summary>
        /// Chemin
        /// </summary>
        public string Path
        {
            get { return _Path; }
            set { _Path = value; OnPropertyChanged("Path"); }
        }

        /// <summary>
        /// Pochette de la piste
        /// </summary>
        public BitmapImage Thumb
        {
            get { return _Thumb; }
            set { _Thumb = value; _Thumb.Freeze(); OnPropertyChanged("Thumb"); }
        }

        #endregion

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
