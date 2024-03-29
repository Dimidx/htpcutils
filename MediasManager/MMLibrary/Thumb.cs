﻿using System;
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
        private BackgroundWorker bw;// = new BackgroundWorker();

        #endregion

        #region Public
        /// <summary>
        /// image
        /// </summary>
        public BitmapImage Image
        {
            get
            {
                if (_Image == null & !_IsLoading & !String.IsNullOrEmpty(_URLImage))
                {
                    Console.WriteLine("Chargement Normal : " + URLImage);
                    _IsLoading = true;
                    OnPropertyChanged("IsLoading");
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
                if (_Image == null & !_IsLoading & !String.IsNullOrEmpty(_URLImage))
                {
                    Console.WriteLine("Chargement Mini : " + URLImage);
                    _IsLoading = true;
                    OnPropertyChanged("IsLoading");
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

                _FichierCache = defaultCacheDir + _hash.Substring(0, 1) + @"\" + _hash.Substring(1, 1) + @"\" + _hash + ".jpg";
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
                    catch (Exception e)
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
                        Console.WriteLine("Save Thumb " + _FichierCache);
                        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                        encoder.QualityLevel = 100;
                        encoder.Frames.Add(BitmapFrame.Create(_Image));
                        encoder.Save(stream);
                        stream.Close();
                        encoder = null;
                        stream.Dispose();
                        //stream = null;
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


        public void GetImage(bool Force)
        {

            _IsLoading = true;
            OnPropertyChanged("IsLoading");
            if (bw != null)
            {
                if (bw.IsBusy)
                {
                    bw.CancelAsync();
                    while (bw.IsBusy)
                    {
                        Thread.Sleep(1);
                    }
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
            bw.Dispose();
            _IsLoading = false;
            OnPropertyChanged("IsLoading");
            Console.WriteLine("Chargement Terminée : " + URLImage);
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_URLImage == "")
            {

            }
            else
            {
    
                //L'image est locale et en cache
                if (IsLocal & IsCached)
                {
                    try
                    {
                        Console.WriteLine("Image Locale - Chargement depuis le cache : " + _FichierCache);
                        _Image = Utils.GetImageSource(_FichierCache);
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine("Invalid operation: " + ex.ToString());
                    }
                    goto Mini;
                }

                //L'image est locale mais n'est pas en cache
                if (IsLocal & !IsCached)
                {

                    try
                    {
                        Console.WriteLine("Image Locale - Enregistrement dans le cache : " + _FichierCache);
                        File.Delete(_FichierCache);
                        _Image = Utils.GetImageSource(_URLImage);
                        SaveImage();
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine("Invalid operation: " + ex.ToString());
                    }
                    goto Mini;
                }


                //L'image est distante et en cache
                if (IsCached & !IsLocal)
                {
                    Console.WriteLine("Image Distante - Chargement depuis le cache : " + _FichierCache);
                    try
                    {
                        _Image = Utils.GetImageSource(_FichierCache);
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine("Invalid operation: " + ex.ToString());
                    }
                    goto Mini;
                }

                //L'image est Distante et Pas en cache
                if (!IsCached & !IsLocal)
                {
                    Console.WriteLine("Image Distante - Enregistrement dans le cache : " + _FichierCache);
                    _Image = Utils.GetStreamImage(_URLImage);
                    SaveImage();
                    goto Mini;


                }

            Mini:
                if (_Image != null)
                {

                    try
                    {
                        _Largeur = Math.Round(_Image.Width, 0);
                        //_Hauteur = Math.Round(_Image.Height, 0);
                    }
                    catch 
                    {
                        Console.WriteLine("Erreur recup dimension image :" + _FichierCache);
                        return;
                    }


                }
                //Si le fichier est bien en cache on crée la miniature
                if (IsCached)
                {
                    try
                    {
                        //Création de la miniature
                        _Miniature = Utils.GetImageSource(_FichierCache, 250);
                    }
                    catch
                    {
                        Console.WriteLine("Invalid operation: ") ;//+ ex.ToString());
                    }

                }
            }


        }

        public Thumb()
        {
            //Console.WriteLine("Charge le thumb");
        }

        public Thumb(string _URL)
        {
            URLImage = _URL;
        }

        ~Thumb()
        {
            if (bw != null)
            {
                if (bw.IsBusy)
                {
                    bw.CancelAsync();
                    while (bw.IsBusy)
                    {
                        Console.WriteLine("Attente bw");
                        Thread.Sleep(10);
                    }
                }
                bw.Dispose();
            }
            try
            {
                _Image = null;
                _Miniature = null;
                _IsLoading = false;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Invalid operation: " + ex.ToString());
            }


            //Console.WriteLine("Decharge Thumb " + URLImage);
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
