using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CinePassion
{

    public class Rating : INotifyPropertyChanged
    {
        public enum eType
        {
            AlloCine = 1,
            IMDB = 2,
            CinePassion = 3
        }

        private int _Votes;
        private float _Note;
        private eType _Type;


        /// <summary>
        /// Type de note
        /// </summary>
        public eType Type
        {
            get { return _Type; }
            set { _Type = value; OnPropertyChanged("Type"); }
        }
        /// <summary>
        /// Nombre de Votes
        /// </summary>
        public int Votes
        {
            get { return _Votes; }
            set { _Votes = value; OnPropertyChanged("Votes"); }
        }

        /// <summary>
        /// Note
        /// </summary>
        public float Note
        {
            get { return _Note; }
            set { _Note = value; OnPropertyChanged("Note"); }
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
