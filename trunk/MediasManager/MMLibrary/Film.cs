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
        private string _Titre;
        private string _Annee;
        private string _Studio;
        private DateTime _DateSortie;
        private string _TitreOriginal;
        private PersonneCollection _Realisateurs;
        private PersonneCollection _Acteurs;
        private ObservableCollection<Thumb> _ListeCover;
        
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
        /// Studio
        /// </summary>
        public string Studio
        {
            get { return _Studio; }
            set { _Studio = value; OnPropertyChanged("Studio"); }
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

        private string[] _Pays;
        /// <summary>
        /// Pays
        /// </summary>
        public string[] Pays
        {
            get { return _Pays; }
            set { _Pays = value; OnPropertyChanged("Pays"); }
        }

        private int _DureeMin;
        /// <summary>
        /// Durée en minutes
        /// </summary>
        public int DureeMin
        {
            get { return _DureeMin; }
            set { _DureeMin = value; OnPropertyChanged("DureeMin"); }
        }

        private string _DureeChaine;
        /// <summary>
        /// Durée au format __h __min
        /// </summary>
        public string DureeChaine
        {
            get { return _DureeChaine; }
            set { _DureeChaine = value; OnPropertyChanged("DureeChaine"); }
        }

        private string _Critique;
        /// <summary>
        /// Critique presse
        /// </summary>
        public string Critique
        {
            get { return _Critique; }
            set { _Critique = value; OnPropertyChanged("Critique"); }
        }

        private string _Avis;
        /// <summary>
        /// Avis
        /// </summary>
        public string Avis
        {
            get { return _Avis; }
            set { _Avis = value; OnPropertyChanged("Avis"); }
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

        private float _NotePresse;
        /// <summary>
        /// Note de la presse (/10)
        /// </summary>
        public float NotePresse
        {
            get { return _NotePresse; }
            set { _NotePresse = value; OnPropertyChanged("NotePresse"); }
        }

        private float _NoteSpectateurs;
        /// <summary>
        /// Note des spectateurs (/10)
        /// </summary>
        public float NoteSpectateurs
        {
            get { return _NoteSpectateurs; }
            set { _NoteSpectateurs = value; OnPropertyChanged("NoteSpectateurs"); }
        }

        #endregion

        public Film()
        {
            this._Acteurs = new PersonneCollection();
            this._Realisateurs = new PersonneCollection();
            this._ListeCover = new ObservableCollection<Thumb>();
            this._ListeFanart = new ObservableCollection<Thumb>();

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
