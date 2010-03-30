using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Drawing;
using System.Net;
using System.Web;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace CinePassion
{
    /// <summary>
    /// Représente un film sur CinePassion
    /// </summary>
    [Serializable]
    [System.Xml.Serialization.XmlRoot()]
    public class Film : INotifyPropertyChanged
    {

        #region Private
        
        private string _id;
        private string _id_allocine;
        private string _id_imdb;
        private string _last_change;
        private string _url;
        private string _title;
        private string _originaltitle;
        private string _year;
        private string _runtime;
        private string _plot;               //Synopsis
        private string _tagline;            //Accroche
        private string _informations;       //Budget, couleur...
        private DateTime _DateSortie;
        private string[] _trailers;
        private string[] _studios;
        private string[] _countries;
        private string[] _genres;
        private PersonneCollection _directors;
        private PersonneCollection _actors;
        private PersonneCollection _credits;
        private ObservableCollection<Thumb> _ListeCover;
        private ObservableCollection<Thumb> _ListeThumb;
        private ObservableCollection<Rating> _ratings;


        #endregion

        #region Public


        /// <summary>
        /// Référence du film chez http://www.allocine.fr
        /// </summary>
        public string Id_Allocine
        {
            get { return _id_allocine; }
            set { _id_allocine = value; OnPropertyChanged("Id_Allocine"); }
        }

        /// <summary>
        /// Référence du film chez http://www.imdb.com
        /// </summary>
        public string Id_IMDB
        {
            get { return _id_imdb; }
            set { _id_imdb = value; OnPropertyChanged("Id_IMDB"); }
        }

        /// <summary>
        /// Dernière mise à jour chez http://passion-xbmc.org
        /// </summary>
        public string LastChange
        {
            get { return _last_change; }
            set { _last_change = value; OnPropertyChanged("LastChange"); }
        }

        /// <summary>
        /// URL de la fiche chez http://passion-xbmc.org
        /// </summary>
        public string Url
        {
            get { return _url; }
            set { _url = value; OnPropertyChanged("Url"); }
        }

        /// <summary>
        /// Référence du film chez http://passion-xbmc.org
        /// </summary>
        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("Id"); }
        }

        /// <summary>
        /// Une Affiche est présente
        /// </summary>
        public bool HasCover
        {
            get { return Cover != null; }
        }

        /// <summary>
        /// Un fanart est présent
        /// </summary>
        public bool HasFanart
        {
            get { return Fanart != null; }
        }

        /// <summary>
        /// Un traile est présent
        /// </summary>
        public bool HasTrailer
        {
            get { return Trailers != null; }
        }

        /// <summary>
        /// Trailer
        /// </summary>
        public string[] Trailers
        {
            get { return _trailers; }
            set { _trailers = value; OnPropertyChanged("Trailers"); }
        }

        /// <summary>
        /// Titre Français
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }

        /// <summary>
        /// Année de production
        /// </summary>
        public string Year
        {
            get { return _year; }
            set { _year = value; OnPropertyChanged("Year"); }
        }

        /// <summary>
        /// Date de sortie en France
        /// </summary>
        public DateTime DateSortie
        {
            get { return _DateSortie; }
            set { _DateSortie = value; OnPropertyChanged("DateSortie"); }
        }

        /// <summary>
        /// Titre Original
        /// </summary>
        public string OriginalTitle
        {
            get { return _originaltitle; }
            set { _originaltitle = value; OnPropertyChanged("OriginalTitle"); }
        }

        /// <summary>
        /// Liste des réalisateurs
        /// </summary>
        public PersonneCollection Directors
        {
            get { return _directors; }
            set { _directors = value; OnPropertyChanged("Directors"); }
        }

        /// <summary>
        /// Liste des acteurs
        /// </summary>
        public PersonneCollection Actors
        {
            get { return _actors; }
            set { _actors = value; OnPropertyChanged("Actors"); }
        }

        /// <summary>
        /// Liste des credits (scénaristes...)
        /// </summary>
        public PersonneCollection Credits
        {
            get { return _credits; }
            set { _credits = value; OnPropertyChanged("Credits"); }
        }


         ///<summary>
         ///Résumé
         ///</summary>
        public string Plot
        {
            get { return _plot; }
            set { _plot = value; OnPropertyChanged("Plot"); }
        }


        /// <summary>
        /// Durée du film
        /// </summary>
        public string Runtime
        {
            get { return _runtime; }
            set { _runtime = value; OnPropertyChanged("Runtime"); }
        }

        /// <summary>
        /// Quelques mots sur le film (Accroche)
        /// </summary>
        public string Tagline
        {
            get { return _tagline; }
            set { _tagline = value; OnPropertyChanged("Tagline"); }
        }

        ///// <summary>
        ///// Certification
        ///// </summary>
        //public string Certification
        //{
        //    get { return _Certification; }
        //    set { _Certification = value; OnPropertyChanged("Certification"); }
        //}

        /// <summary>
        /// MPAA
        /// </summary>
        public string Informations
        {
            get { return _informations; }
            set { _informations = value; OnPropertyChanged("Informations"); }
        }

        ///// <summary>
        ///// Top250 d'IMDB
        ///// </summary>
        //public int Top250
        //{
        //    get { return _Top250; }
        //    set { _Top250 = value; OnPropertyChanged("Top250"); }
        //}

        /// <summary>
        /// La jaquette du film
        /// </summary>
        public Thumb Cover
        {
            get 
            {
                if (_ListeCover.Count > 0)
                {
                    return _ListeCover[0];
                }
                return null;
            }
        }

        /// <summary>
        /// Le fanart pour le film
        /// </summary>
        public Thumb Fanart
        {
            get 
            {
                if (_ListeFanart.Count > 0)
                {
                    return _ListeFanart[0];
                }
                return null;
            }
        }


        /// <summary>
        /// Liste d'affiches pour le film
        /// </summary>
        public ObservableCollection<Thumb> ListeCover
        {
            get { return _ListeCover; }
            set { _ListeCover = value; OnPropertyChanged("ListeCover"); OnPropertyChanged("Cover"); }
        }

        /// <summary>
        /// Liste de miniatures pour le film
        /// </summary>
        public ObservableCollection<Thumb> ListeThumbs
        {
            get { return _ListeThumb; }
            set { _ListeThumb = value; OnPropertyChanged("ListeThumbs"); }
        }

        private ObservableCollection<Thumb> _ListeFanart;
        /// <summary>
        /// Liste de fanart pour le film
        /// </summary>
        public ObservableCollection<Thumb> ListeFanart
        {
            get { return _ListeFanart; }
            set { _ListeFanart = value; OnPropertyChanged("ListeFanart"); OnPropertyChanged("Fanart"); }
        }

        /// <summary>
        /// Liste des genres
        /// </summary>
        public string[] Genres
        {
            get { return _genres; }
            set { _genres = value; OnPropertyChanged("Genres"); }
        }

        /// <summary>
        /// Liste des pays
        /// </summary>
        public string[] Countries
        {
            get { return _countries; }
            set { _countries = value; OnPropertyChanged("Countries"); }
        }

        /// <summary>
        /// Liste des studios
        /// </summary>
        public string[] Studios
        {
            get { return _studios; }
            set { _studios = value; OnPropertyChanged("Studios"); }
        }


        /// <summary>
        /// Notes
        /// </summary>
        public ObservableCollection<Rating> Ratings
        {
            get { return _ratings; }
            set { _ratings = value; OnPropertyChanged("Ratings"); }
        }

        #endregion

        public Film()
        {
            this._actors = new PersonneCollection();
            this._directors = new PersonneCollection();
            this._credits = new PersonneCollection();
            this._ListeCover = new ObservableCollection<Thumb>();
            this._ListeFanart = new ObservableCollection<Thumb>();
            this._ListeThumb = new ObservableCollection<Thumb>();
            this._ratings = new ObservableCollection<Rating>();
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
   /// <summary>
   /// Collection de Film
   /// </summary>
    public class FilmCollection : ObservableCollection<Film>
    {
      
    }


    public class FilmComparer : IComparer<Film>
    {

        public int Compare(Film _Film1, Film _Film2)
        {
            int i = -1;
            i = _Film1.Title.CompareTo(_Film2.Title);
            return i;
        }

    }

    

}
