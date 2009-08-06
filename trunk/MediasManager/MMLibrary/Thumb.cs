using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Net;
using System.Web;
using System.IO;
using System.Xml.Serialization;
using System.Threading;

namespace MediaManager.Library
{




    /// <summary>
    /// Représente une image avec sa miniature 
    /// </summary>
    [Serializable]
    public class Thumb : INotifyPropertyChanged
    {

        #region Private

        private string defaultCacheDir = System.Environment.CurrentDirectory + @"\Cache\Images\";
        private bool _IsLoading = false;
        private string _URLImage = "";
        private BitmapImage _Image;
        private BitmapImage _Miniature;
        private double _Hauteur;
        private double _Largeur;
        private string _FichierCache;
        private BackgroundWorker bw = new BackgroundWorker();

        #endregion

        #region Public
        /// <summary>
        /// image
        /// </summary>
        public BitmapImage Image
        {
            get
            {
                if (_Image == null & !_IsLoading & _URLImage != "")
                {
                    GetImage();
                    return null;

                }
                else
                {
                    return _Image;
                }


            }
            //set
            //{
            //    _Image = value;
            //    //if (_Image != null) _Image.Freeze();
            //    OnPropertyChanged("Image");
            //}
        }

        /// <summary>
        /// Miniature
        /// </summary>
        public BitmapImage Miniature
        {
            get
            {
                if (_Image == null & !_IsLoading & _URLImage != "")
                {
                    GetImage();
                    return null;
                }
                else
                {
                    return _Miniature;
                }


            }
            //set
            //{
            //    _Miniature = value;
            //    //if (_Miniature != null) _Miniature.Freeze();
            //    OnPropertyChanged("Miniature");
            //}
        }

        /// <summary>
        /// IsDownloading
        /// </summary>
        public bool IsLoading
        {
            get { return _IsLoading; }
            //set { _IsLoading = value; OnPropertyChanged("IsLoading"); }
        }

        /// <summary>
        /// Fichier Cache
        /// </summary>
        public string FichierCache
        {
            get { return _FichierCache; }
        }
        

        /// <summary>
        /// Hauteur de l'image
        /// </summary>
        public double Hauteur
        {
            get { return _Hauteur; }
        }

        /// <summary>
        /// Largeur de l'image
        /// </summary>
        public double Largeur
        {
            get { return _Largeur; }
        }

        /// <summary>
        /// URL du fichier image
        /// </summary>
        public string URLImage
        {
            get { return _URLImage; }
            set
            {
                _URLImage = value;
                string _hash = Utils.Hash(_URLImage);
                if (!Directory.Exists(defaultCacheDir + _hash.Substring(0, 1) + @"\")) Directory.CreateDirectory(defaultCacheDir + _hash.Substring(0, 1) + @"\");
                if (!Directory.Exists(defaultCacheDir + _hash.Substring(0, 1) + @"\" + _hash.Substring(1, 1) + @"\")) Directory.CreateDirectory(defaultCacheDir + _hash.Substring(0, 1) + @"\" + _hash.Substring(1, 1) + @"\");

                _FichierCache = defaultCacheDir + _hash.Substring(0, 1) + @"\" + _hash.Substring(1, 1) + @"\" + _hash+ ".jpg";
                OnPropertyChanged("URLImage");
            }
        }

        /// <summary>
        /// Si l'image est en cache
        /// </summary>
        public bool IsCached
        {
            get { return File.Exists(_FichierCache); }
        }

        /// <summary>
        /// Si l'image est locale
        /// </summary>
        public bool IsLocal
        {
            get
            {
                if (_URLImage != "")
                {
                    try
                    {
                        Uri _uri = new Uri(_URLImage, UriKind.RelativeOrAbsolute);
                        string _protocole = _uri.GetLeftPart(UriPartial.Scheme);
                        return _protocole.Contains("file://");
                    }
                    catch (Exception e )
                    {
                        Console.WriteLine("IsLocal" + Environment.NewLine + e.Message);
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
        }



        #endregion

        private void SaveImage()
        {
            if (!IsCached & !IsLocal)
            {
                try
                {
                    using (FileStream stream = new FileStream(_FichierCache, FileMode.Create))
                    {
                        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(_Image));
                        encoder.Save(stream);
                        stream.Close();
                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine("Impossible de mettre en cache le fichier " + _FichierCache + Environment.NewLine + e.Message);
                }
            }
            if (!IsCached & IsLocal)
            {
                try
                {
                    File.Copy(_URLImage, _FichierCache);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Impossible de mettre en cache le fichier " + _FichierCache + Environment.NewLine + e.Message);
                }
            }

        }

        public void GetImage()
        {
            GetImage(false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Force">Supprime le cache</param>

        public void GetImage(bool Force)
        {

            _IsLoading = true;
            OnPropertyChanged("IsLoading");
            if (bw.IsBusy)
            {
                bw.CancelAsync();
                while (bw.IsBusy)
                {
                    Thread.Sleep(1);
                }
            }
            if (Force)
            {
                try
                {
                    File.Delete(_FichierCache);
                }
                catch (Exception e)
                {

                    Console.WriteLine("Impossible de supprimer le fichier " + _FichierCache + " du cache." + Environment.NewLine + e.Message);
                }
            }

            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync();

        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            OnPropertyChanged("Image");
            OnPropertyChanged("Miniature");
            OnPropertyChanged("Largeur");
            OnPropertyChanged("Hauteur");
            Console.WriteLine("Chargement Terminée : " + URLImage);
            bw.Dispose();
            _IsLoading = false;
            OnPropertyChanged("IsLoading");
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_URLImage == "")
            {

            }
            else
            {
                _Image = new BitmapImage();
                _Image.BeginInit();
                _Image.CreateOptions = BitmapCreateOptions.IgnoreColorProfile | BitmapCreateOptions.DelayCreation;
                _Image.CacheOption = BitmapCacheOption.OnLoad;

                //L'image est locale et en cache
                if (IsLocal & IsCached)
                {
                    _Image.UriSource = new Uri(_FichierCache, UriKind.RelativeOrAbsolute);
                    _Image.EndInit();
                    _Image.Freeze();
                    goto Mini;
                }

                //L'image est locale mais n'est pas en cache
                if (IsLocal & !IsCached)
                {
                    File.Delete(_FichierCache);
                    _Image.UriSource = new Uri(_URLImage, UriKind.RelativeOrAbsolute);
                    _Image.EndInit();
                    _Image.Freeze();
                    SaveImage();
                    goto Mini;
                }


                //L'image est distante et en cache
                if (IsCached & !IsLocal)
                {
                    _Image.UriSource = new Uri(_FichierCache, UriKind.RelativeOrAbsolute);
                    _Image.EndInit();
                    _Image.Freeze();
                    //SaveImage();
                    goto Mini;
                }
                if (!IsCached & !IsLocal)
                {
                        MemoryStream ms = Utils.GetStreamImage(this._URLImage);
                        if (ms != null)
                        {
                            _Image.StreamSource = ms;
                            _Image.EndInit();
                            _Image.Freeze();
                            ms = null;
                            SaveImage();
                        }
                        else
                        {
                            
                            _URLImage = "pack://application:,,,/Images/Erreur_250.png";
                            _Image.UriSource = new Uri(_URLImage, UriKind.RelativeOrAbsolute);
                            _Image.EndInit();
                            _Image.Freeze();
                            ms = null;
                            //Création de la miniature
                            _Miniature = new BitmapImage();
                            _Miniature.BeginInit();
                            _Miniature.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                            _Miniature.CacheOption = BitmapCacheOption.OnLoad;
                            _Miniature.DecodePixelWidth = 200;
                            _Miniature.UriSource = new Uri(_URLImage, UriKind.RelativeOrAbsolute);
                            _Miniature.EndInit();
                            _Miniature.Freeze();
                        }
                }

            Mini:
                if (_Image != null)
                {
                    _Largeur = Math.Round(_Image.Width,0);
                    _Hauteur = Math.Round(_Image.Height,0);
                }
                if (IsCached)
                {
                    //Création de la miniature
                    _Miniature = new BitmapImage();
                    _Miniature.BeginInit();
                    _Miniature.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                    _Miniature.CacheOption = BitmapCacheOption.OnLoad;
                    _Miniature.DecodePixelWidth = 200;
                    _Miniature.UriSource = new Uri(_FichierCache, UriKind.RelativeOrAbsolute);
                    _Miniature.EndInit();
                    _Miniature.Freeze();

                }
            }


        }

        public Thumb()
        {
            //Console.WriteLine("Charge le thumb");
        }

        public Thumb(string _URL)
        {
            Console.WriteLine("Charge le thumb" + _URL);
            URLImage = _URL;
        }

        ~Thumb()
        {

            if (bw.IsBusy)
            {
                bw.CancelAsync();
                while (bw.IsBusy)
                {
                    Thread.Sleep(1);
                }
            }
            bw.Dispose();
            ////Image.Freeze();
            ////Miniature.Freeze();

            //this.Image = null;
            //this.Miniature = null;
            this._Image = null;
            this._Miniature = null;
            this._IsLoading = false;
            Console.WriteLine("Decharge Thumb " + URLImage);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string PropertyName)
        {
            OnPropertyChanged(this, PropertyName);
        }
        protected void OnPropertyChanged(object sender, string PropertyName)
        {
            OnPropertyChanged(sender, new PropertyChangedEventArgs(PropertyName));
        }
        protected void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(sender, e);
        }


        #endregion

    }
}
