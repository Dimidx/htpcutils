using System;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;

namespace MediaManager.Library.NFO
{
    public class NfoFile : INotifyPropertyChanged

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


        public static NfoTV getNfoTV(String NfoPath)
        {
            NfoTV nf;
            Serializer s = new Serializer(NfoPath, new NfoTV());
            nf = (NfoTV)s.FromFile();
            return nf;
        }

        public static NfoMovie getNfoMovie(String NfoPath)
        {
            NfoMovie nf;
            Serializer s = new Serializer(NfoPath, new NfoMovie());
            nf = (NfoMovie)s.FromFile();
            return nf;
        }

        public static bool saveNfoTV(NfoTV nf, String NfoPath)
        {
            Serializer s = new Serializer(NfoPath, nf);
            return s.ToFile();
        }

        public static bool saveNfoMovie(NfoMovie nf, String NfoPath)
        {
            Serializer s = new Serializer(NfoPath, nf);
            return s.ToFile();
        }
    }
}
