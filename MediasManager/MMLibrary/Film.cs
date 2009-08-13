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

namespace MediaManager.Library
{
    /// <summary>
    /// Représente un film sur http://www.allocine.fr
    /// </summary>
    [Serializable]
    [System.Xml.Serialization.XmlRoot()]
    public class Film : INotifyPropertyChanged
    {

        #region Private
        
        private string _AlloId;
        private string _Id;
        private string _Trailer;
        private string _Titre;
        private string _Annee;
        private string _Studio;
        private DateTime _DateSortie;
        private string _TitreOriginal;
        private PersonneCollection _Realisateurs;
        private PersonneCollection _Acteurs;
        private ObservableCollection<Thumb> _ListeCover;
        private ObservableCollection<Thumb> _ListeThumb;
        #endregion

        #region Public


        /// <summary>
        /// Référence du film chez http://www.allocine.fr
        /// </summary>
        public string AlloID
        {
            get { return _AlloId; }
            set { _AlloId = value; OnPropertyChanged("AlloID"); }
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
            get { return !String.IsNullOrEmpty(Trailer); }
        }

        /// <summary>
        /// Studio
        /// </summary>
        public string Studio
        {
            get { return _Studio; }
            set { _Studio = value; OnPropertyChanged("Studio"); }
        }

        /// <summary>
        /// Trailer
        /// </summary>
        public string Trailer
        {
            get { return _Trailer; }
            set { _Trailer = value; OnPropertyChanged("Trailer"); }
        }

        /// <summary>
        /// Référence du film
        /// </summary>
        public string ID
        {
            get { return _Id; }
            set { _Id = value; OnPropertyChanged("ID"); }
        }

        /// <summary>
        /// Titre Français
        /// </summary>
        public string Titre
        {
            get { return _Titre; }
            set { _Titre = value; OnPropertyChanged("Titre"); }
        }

        /// <summary>
        /// Année de production
        /// </summary>
        public string Annee
        {
            get { return _Annee; }
            set { _Annee = value; OnPropertyChanged("Annee"); }
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
        public string TitreOriginal
        {
            get { return _TitreOriginal; }
            set { _TitreOriginal = value; OnPropertyChanged("TitreOriginal"); }
        }

        /// <summary>
        /// Liste des réalisateurs
        /// </summary>
        public PersonneCollection Realisateurs
        {
            get { return _Realisateurs; }
            set { _Realisateurs = value; OnPropertyChanged("Realisateurs"); }
        }

        /// <summary>
        /// Liste des acteurs
        /// </summary>
        public PersonneCollection Acteurs
        {
            get { return _Acteurs; }
            set { _Acteurs = value; OnPropertyChanged("Acteurs"); }
        }

        private string _Synopsis;
        /// <summary>
        /// Résumé
        /// </summary>
        public string Synopsis
        {
            get { return _Synopsis; }
            set { _Synopsis = value; OnPropertyChanged("Synopsis"); }
        }

        private string _Duree;
        /// <summary>
        /// Durée du film
        /// </summary>
        public string Duree
        {
            get { return _Duree; }
            set { _Duree = value; OnPropertyChanged("Duree"); }
        }

        private bool _Vu;
        /// <summary>
        /// Film déjà vu ou pas
        /// </summary>
        public bool Vu
        {
            get { return _Vu; }
            set { 
                _Vu = value;
                
                if (_Vu == true)
                {
                    if (_NombreLectures == 0)
                    {
                        _NombreLectures = 1;
                        OnPropertyChanged("NombreLectures");
                    }
                }
                else
                {
                    _NombreLectures = 0;
                    OnPropertyChanged("NombreLectures");
                }
                OnPropertyChanged("Vu");

            }
        }

        private int _NombreLectures;
        /// <summary>
        /// Nombre de fois ou le film a été lu
        /// </summary>
        public int NombreLectures
        {
            get { return _NombreLectures; }
            set { _NombreLectures = value; OnPropertyChanged("NombreLectures"); }
        }

        private string _Resume;
        /// <summary>
        /// Court résumé du film
        /// </summary>
        public string Resume
        {
            get { return _Resume; }
            set { _Resume = value; OnPropertyChanged("Resume"); }
        }

        private string _Accroche;
        /// <summary>
        /// Quelques mots sur le film
        /// </summary>
        public string Accroche
        {
            get { return _Accroche; }
            set { _Accroche = value; OnPropertyChanged("Accroche"); }
        }

        private string _Certification;
        /// <summary>
        /// Certification
        /// </summary>
        public string Certification
        {
            get { return _Certification; }
            set { _Certification = value; OnPropertyChanged("Certification"); }
        }

        private string _MPAA;
        /// <summary>
        /// MPAA
        /// </summary>
        public string MPAA
        {
            get { return _MPAA; }
            set { _MPAA = value; OnPropertyChanged("MPAA"); }
        }

        private int _Top250;
        /// <summary>
        /// Top250 d'IMDB
        /// </summary>
        public int Top250
        {
            get { return _Top250; }
            set { _Top250 = value; OnPropertyChanged("Top250"); }
        }

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

        private string _URLBandeAnnonce;
        /// <summary>
        /// L'URL de la bande annonce
        /// </summary>
        private string URLBandeAnnonce
        {
            get { return _URLBandeAnnonce; }
            set { _URLBandeAnnonce = value; OnPropertyChanged("URLBandeAnnonce"); }
        }


        private string[] _Genres;
        /// <summary>
        /// Liste des genres
        /// </summary>
        public string[] Genres
        {
            get { return _Genres; }
            set { _Genres = value; OnPropertyChanged("Genres"); }
        }

        private float _Note;
        /// <summary>
        /// Note (/10)
        /// </summary>
        public float Note
        {
            get { return _Note; }
            set { _Note = value; OnPropertyChanged("Note"); }
        }

        private float _Votes;
        /// <summary>
        /// Nombre de Votes
        /// </summary>
        public float Votes
        {
            get { return _Votes; }
            set { _Votes = value; OnPropertyChanged("Votes"); }
        }

        #endregion

        public Film()
        {
            this._Acteurs = new PersonneCollection();
            this._Realisateurs = new PersonneCollection();
            this._ListeCover = new ObservableCollection<Thumb>();
            this._ListeFanart = new ObservableCollection<Thumb>();
            this._ListeThumb = new ObservableCollection<Thumb>();
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
    public class Films : ObservableCollection<Film>
    {
        
        //
    }


    public class FilmComparer : IComparer<Film>
    {

        public int Compare(Film _Film1, Film _Film2)
        {
            int i = -1;
            i = _Film1.Titre.CompareTo(_Film2.Titre);
            return i;
        }

    }

    

}
