using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MediaManager.Library
{
    /// <summary>
    /// Représente une personne sur le site http://www.allocine.fr
    /// </summary>
    public class Personne : INotifyPropertyChanged
    {
        private string m_PersonneID;
        /// <summary>
        /// ID de la personne
        /// </summary>
        public string ID
        {
            get { return m_PersonneID; }
            set { m_PersonneID = value; OnPropertyChanged("ID"); }
        }

        private string m_Nom;
        /// <summary>
        /// Prénom et Nom
        /// </summary>
        public string Nom
        {
            get { return m_Nom; }
            set { m_Nom = value; OnPropertyChanged("Nom"); }
        }

        private string m_Role;
        /// <summary>
        /// Rôle
        /// </summary>
        public string Role
        {
            get { return m_Role; }
            set { m_Role = value; OnPropertyChanged("Role"); }
        }

        private Thumb m_Photo;
        /// <summary>
        /// Photo
        /// </summary>
        public Thumb Photo
        {
            get { return m_Photo; }
            set { m_Photo = value; OnPropertyChanged("Photo"); }
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
    /// Collection de Personnes
    /// </summary>
    public class PersonneCollection : ObservableCollection<Personne>
    {

        //
    }
}
