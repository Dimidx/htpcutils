using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using MediaManager;
using MediaManager.Library;
using System.Threading;
using System.Windows.Threading;
using System.Windows;

namespace MediaManager
{
    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        private SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

        public AsyncObservableCollection()
        {
        }

        public AsyncObservableCollection(IEnumerable<T> list)
            : base(list)
        {
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext)
            {
                // Execute the CollectionChanged event on the current thread
                RaiseCollectionChanged(e);
            }
            else
            {
                // Post the CollectionChanged event on the creator thread
                _synchronizationContext.Post(RaiseCollectionChanged, e);
            }
        }

        private void RaiseCollectionChanged(object param)
        {
            // We are in the creator thread, call the base implementation directly
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext)
            {
                // Execute the PropertyChanged event on the current thread
                RaisePropertyChanged(e);
            }
            else
            {
                // Post the PropertyChanged event on the creator thread
                _synchronizationContext.Post(RaisePropertyChanged, e);
            }
        }

        private void RaisePropertyChanged(object param)
        {
            // We are in the creator thread, call the base implementation directly
            base.OnPropertyChanged((PropertyChangedEventArgs)param);
        }
    }


    public class MovieManager
    {
        //La collection de film
        ObservableCollection<Movie> _movies = new ObservableCollection<Movie>();

        /// <summary>
        /// Pour pouvoir ajouter un film dans la collection depuis un autre Thread
        /// </summary>
        /// <param name="_movie"></param>
        /// <returns></returns>
        private object Add(object _movie)
        {
            _movies.Add((Movie)_movie);
            return null;
        }

        /// <summary>
        /// Pour pouvoir vider la collection depuis un autre Thread
        /// </summary>
        /// <returns></returns>
        private object Clear(object _null)
        {
            _movies.Clear(); ;
            return null;
        }

        /// <summary>
        /// La collection de films
        /// </summary>
        public ObservableCollection<Movie> Movies
        {
            get { return _movies; }
        }

        public void scanMovieDirs() //object sender)
        {

            ObservableCollection<MovieFolder> paths = Settings.XML.Config.confMovie.MovieFolders;
            //Récupère le thread
            Application app = System.Windows.Application.Current;
            if (app != null)
            {
                app.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(Clear),null);
            }

            //_movies.Clear();
            if (paths != null)
            {

                foreach (MovieFolder mf in paths)
                    
                {
                    
                    String directory = mf.path;
                    if (Directory.Exists(directory))
                    {
                    DirectoryInfo dir = new DirectoryInfo(directory);
                    if (mf.containsFolders)
                    {
                        foreach (DirectoryInfo dinf in dir.GetDirectories())
                        {
                            foreach (String ext in Settings.XML.Config.confMovie.extensions)
                            {
                                foreach (FileInfo fileInfo in dinf.GetFiles(ext))
                                {
                                    
                                    if (!fileInfo.Name.ToLower().Contains("sample") )
                                    {
                                        if (fileInfo != null)
                                        {
                                            if (app != null)
                                            {
                                                app.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(Add), new Movie(fileInfo, mf));
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (String ext in Settings.XML.Config.confMovie.extensions)
                        {
                            foreach (FileInfo fileInfo in dir.GetFiles(ext))
                            {
                                if (!fileInfo.Name.ToLower().Contains("sample"))
                                {
                                    if (fileInfo != null)
                                    {
                                        if (app != null)
                                        {
                                            app.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(Add), new Movie(fileInfo, mf));
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                }
            }
        }
    }
}
