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
        #region Membres

        private string m_FilmId;
        /// <summary>
        /// Référence du film chez http://www.allocine.fr
        /// </summary>
        public string ID
        {
            get { return m_FilmId; }
            set { m_FilmId = value; OnPropertyChanged("ID"); }
        }

        private string m_FilmTitle;

        /// <summary>
        /// Titre Français
        /// </summary>
        public string Titre
        {
            get { return m_FilmTitle; }
            set { m_FilmTitle = value; OnPropertyChanged("Titre"); }
        }

        private string m_FilmAnnee;

        /// <summary>
        /// Année de production
        /// </summary>
        public string Annee
        {
            get { return m_FilmAnnee; }
            set { m_FilmAnnee = value; OnPropertyChanged("Annee"); }
        }

 
        private DateTime m_FilmDateSortie;

        /// <summary>
        /// Date de sortie en France
        /// </summary>
        public DateTime DateSortie
        {
            get { return m_FilmDateSortie; }
            set { m_FilmDateSortie = value; OnPropertyChanged("DateSortie"); }
        }

        private string m_FilmOriginalTitle;

        /// <summary>
        /// Titre Original
        /// </summary>
        public string TitreOriginal
        {
            get { return m_FilmOriginalTitle; }
            set { m_FilmOriginalTitle = value; OnPropertyChanged("TitreOriginal"); }
        }

        private PersonneCollection m_FilmRealisateurs;

        /// <summary>
        /// Liste des réalisateurs
        /// </summary>
        public PersonneCollection Realisateurs
        {
            get { return m_FilmRealisateurs; }
            set { m_FilmRealisateurs = value; OnPropertyChanged("Realisateurs"); }
        }

        private PersonneCollection m_FilmActors;
        /// <summary>
        /// Liste des acteurs
        /// </summary>
        public PersonneCollection Acteurs
        {
            get { return m_FilmActors; }
            set { m_FilmActors = value; OnPropertyChanged("Acteurs"); }
        }

        private string m_FilmSynopsis;
        /// <summary>
        /// Résumé
        /// </summary>
        public string Synopsis
        {
            get { return m_FilmSynopsis; }
            set { m_FilmSynopsis = value; }
        }

        private string[] m_FilmPays;
        /// <summary>
        /// Pays
        /// </summary>
        public string[] Pays
        {
            get { return m_FilmPays; }
            set { m_FilmPays = value; OnPropertyChanged("Pays"); }
        }

        private int m_FilmDureeMin;
        /// <summary>
        /// Durée en minutes
        /// </summary>
        public int DureeMin
        {
            get { return m_FilmDureeMin; }
            set { m_FilmDureeMin = value; }
        }

        private string m_FilmDureeChaine;
        /// <summary>
        /// Durée au format __h __min
        /// </summary>
        public string DureeChaine
        {
            get { return m_FilmDureeChaine; }
            set { m_FilmDureeChaine = value; }
        }

        private string m_FilmCritique;
        /// <summary>
        /// Critique presse
        /// </summary>
        public string Critique
        {
            get { return m_FilmCritique; }
            set { m_FilmCritique = value; }
        }

        private string m_FilmAvis;
        /// <summary>
        /// Avis
        /// </summary>
        public string Avis
        {
            get { return m_FilmAvis; }
            set { m_FilmAvis = value; }
        }

        private Thumb m_FilmCover;
        /// <summary>
        /// La jaquette du film
        /// </summary>
        public Thumb Cover
        {
            get { return m_FilmCover; }
            set { m_FilmCover = value; }
        }
 
        private string m_FilmURLBandeAnnonce;
        /// <summary>
        /// L'URL de la bande annonce
        /// </summary>
        private string URLBandeAnnonce
        {
            get { return m_FilmURLBandeAnnonce; }
            set { m_FilmURLBandeAnnonce = value; }
        }


        private string[] m_FilmGenres;
        /// <summary>
        /// Liste des genres
        /// </summary>
        public string[] Genres
        {
            get { return m_FilmGenres; }
            set { m_FilmGenres = value; }
        }

        private float m_FilmNotePress;
        /// <summary>
        /// Note de la presse (/10)
        /// </summary>
        public float NotePresse
        {
            get { return m_FilmNotePress; }
            set { m_FilmNotePress = value; }
        }

        private float m_FilmNoteSpectateurs;
        /// <summary>
        /// Note des spectateurs (/10)
        /// </summary>
        public float NoteSpectateurs
        {
            get { return m_FilmNoteSpectateurs; }
            set { m_FilmNoteSpectateurs = value; }
        }

        #endregion

        public Film()
        {
            this.m_FilmCover = new Thumb();
            this.m_FilmActors = new PersonneCollection();
            this.m_FilmRealisateurs = new PersonneCollection();
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
