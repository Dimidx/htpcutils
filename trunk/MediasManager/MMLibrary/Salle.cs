using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MediaManager.Library
{
    /// <summary>
    /// Représente une salle de cinéma sur le site http://www.allocine.fr
    /// </summary>
    public class Salle : INotifyPropertyChanged
    {
        #region Membres

        private string m_SalleId;

        /// <summary>
        /// ID de la salle
        /// </summary>
        public string SalleId
        {
            get { return m_SalleId; }
            set { m_SalleId = value; OnPropertyChanged("SalleId"); }
        }

        private string m_Ville;

        /// <summary>
        /// Ville de la salle
        /// </summary>
        public string Ville
        {
            get { return m_Ville; }
            set { m_Ville = value; OnPropertyChanged("Ville"); }
        }

        private string m_NomSalle;

        /// <summary>
        /// Nom de la salle
        /// </summary>
        public string NomSalle
        {
            get { return m_NomSalle; }
            set { m_NomSalle = value; OnPropertyChanged("NomSalle"); }
        }

        private string m_CP;

        /// <summary>
        /// Code Postal de la salle
        /// </summary>
        public string CP
        {
            get { return m_CP; }
            set { m_CP = value; OnPropertyChanged("CP"); }
        }


        private string m_Lien;
        /// <summary>
        /// URL de la salle
        /// </summary>
        public string Lien
        {
            get { return m_Lien; }
            set { m_Lien = value; OnPropertyChanged("Lien"); }
        }


        #endregion

        public Salle()
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
    /// <summary>
    /// Collection de salles de cinéma
    /// </summary>
    public class Salles : ObservableCollection<Salle>
    {
        //public ObservableCollection<Salles> ListeSalles { get; set; }
        public Salles()
        {
            //ListeSalles = new ObservableCollection<Salles>();
        }

    }

    public class SalleComparer : IComparer<Salle>
    {

        public int Compare(Salle _Salle1, Salle _Salle2)
        {
            int i = -1;
            i = _Salle1.NomSalle.CompareTo(_Salle2.NomSalle);
            return i;
        }

    }
}
