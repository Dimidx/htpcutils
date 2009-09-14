using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MediaManager.Library;

namespace MediaManager.Library
{
    public class Utils
    {
        #region MD5HashFile
        /// <summary>
        /// Computes the MD5 hash of a given file.
        /// </summary>
        /// <param name="filename">The path of the file.</param>
        /// <returns>The MD5 checksum in string format.</returns>
        /// <exception>Throws an FileNotFoundException if the given file doesn't exist (or is not accessible).</exception>
        public static string MD5HashFile(string filename)
        {
            if (File.Exists(filename))
            {

                System.Security.Cryptography.MD5CryptoServiceProvider md5_summer = new System.Security.Cryptography.MD5CryptoServiceProvider();

                string result = string.Empty;
                using (FileStream fs = File.OpenRead(filename))
                    result = BitConverter.ToString(md5_summer.ComputeHash(fs));
                return result;
            }
            else
            {
                return "";
            }

        }
        #endregion

        #region GetSourceHTML
        /// <summary>
        /// Récupère la source HTML d'une page Web
        /// </summary>
        /// <param name="url">URL de la page à récupérer</param>
        /// <returns></returns>
        public static string GetSourceHTML(string url)
        {
            string sourceHTML = string.Empty;

            try
            {
                WebClient m_webClient = new WebClient();

                #region Proxy
                if (Master.Settings.XML.Config.confProxy.UseProxy)
                {
                    WebProxy wProxy = new WebProxy(Master.Settings.XML.Config.confProxy.ProxyAdress, Convert.ToInt32(Master.Settings.XML.Config.confProxy.ProxyPort));
                    wProxy.Credentials = new NetworkCredential(Master.Settings.XML.Config.confProxy.ProxyUser, Master.Settings.XML.Config.confProxy.ProxyPassword, Master.Settings.XML.Config.confProxy.ProxyDomain);
                    m_webClient.Proxy = wProxy;
                }
                #endregion

                //m_webClient.Encoding = Encoding.UTF8;
                sourceHTML = m_webClient.DownloadString(url);

            }
            catch (Exception e)
            {
                //throw new Exception("An exception occured when you tried to download the file: " + e.Message);
                sourceHTML = "";
            }

            return sourceHTML;

        }

        #endregion

        /// <summary>
        /// Charge une image sans verroulller l'originale
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BitmapImage GetImageSource(string path,int largeur)
        {
            // Ouverture d'une stream vers le fichier original
            StreamReader reader = new StreamReader(path);

            // Préparation d'un tableau de Byte pour lire la stream
            Int32 length = Convert.ToInt32(reader.BaseStream.Length);
            Byte[] data = new Byte[length];

            // Lecture de la stream
            reader.BaseStream.Read(data, 0, length);

            // Création d'une nouvelle stream mémoire
            // afin de copier le contenu de la stream originale
            MemoryStream stream = new MemoryStream(data);

            // Création de l'image à parir de la stream en mémoire
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.IgnoreColorProfile | BitmapCreateOptions.DelayCreation;
            image.CacheOption = BitmapCacheOption.OnDemand;
            if (largeur != 0) image.DecodePixelWidth = largeur; //Miniature
            image.StreamSource = stream;
            image.EndInit();
            image.Freeze();
            reader.Close();

            // Libération des ressources
            reader.Dispose();
            reader = null;
            data = null;
            stream = null;

            return image;
        }

                public static BitmapImage GetImageSource(string url)
        {
            return GetImageSource(url, 0);
        }

                        

        #region GetStreamImage



        /// <summary>
        /// Télécharge une image dans un stream
        /// </summary>
        /// <param name="url">URL de l'image a télécharger</param>
        /// <returns></returns>
        public static BitmapImage GetStreamImage(string url)
        {
            try
            {
                WebClient client = new WebClient();

                #region Proxy
                if (Master.Settings.XML.Config.confProxy.UseProxy)
                {
                    WebProxy wProxy = new WebProxy(Master.Settings.XML.Config.confProxy.ProxyAdress, Convert.ToInt32(Master.Settings.XML.Config.confProxy.ProxyPort));
                    wProxy.Credentials = new NetworkCredential(Master.Settings.XML.Config.confProxy.ProxyUser, Master.Settings.XML.Config.confProxy.ProxyPassword, Master.Settings.XML.Config.confProxy.ProxyDomain);
                    client.Proxy = wProxy;
                }
                #endregion

                byte[] _result = client.DownloadData(new Uri(url));
                client.Dispose();
                client = null;
                MemoryStream ms = new MemoryStream(_result);

                // Création de l'image à parir de la stream en mémoire
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.IgnoreColorProfile | BitmapCreateOptions.DelayCreation;
                image.CacheOption = BitmapCacheOption.OnDemand;

                image.StreamSource = ms;
                image.EndInit();
                image.Freeze();

                // Libération des ressources
                _result = null;
                //ms.Dispose();
                ms = null;

                return image;


            }
            catch (WebException ex)
            {
                Console.WriteLine("Impossible de télécharger l'image " + url + Environment.NewLine + ex.Message);
                return null;
            }

        }
        #endregion

        #region RemoveUnwantedChars
        /// <summary>
        /// Supprime les caractères html
        /// </summary>
        /// <param name="tInput"></param>
        /// <returns></returns>
        public static string RemoveUnwantedChars(string tInput)
        {
            tInput = Regex.Replace(tInput, "<[^<]*>", "");
            tInput = Regex.Replace(tInput, "Plus.*?...", "");
            tInput = tInput.Replace("\\s{2,}", " ");
            tInput = tInput.Replace("&nbsp;", "").Trim();
            return tInput;
        }
        #endregion

        #region GetStringBetweenTwoStrings
        /// <summary>
        /// Method used to extract the content of a string from a 2 strings.
        /// </summary>
        private static string GetStringBetweenTwoStrings(string src, string start, string end)
        {
            string retVal = string.Empty;

            int idxStart = src.IndexOf(start);

            if (idxStart != -1)
            {
                idxStart++;

                int idxEnd = src.IndexOf(end, idxStart);

                if (idxEnd != -1)
                {
                    retVal = src.Substring(idxStart, idxEnd - idxStart);
                }
            }

            return retVal;
        }
        #endregion

        #region Hash
        public static string Hash(string input)
        {
            byte[] bytes;
            uint m_crc = 0xffffffff;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            bytes = encoding.GetBytes(input.ToLower());
            foreach (byte myByte in bytes)
            {
                m_crc ^= ((uint)(myByte) << 24);
                for (int i = 0; i < 8; i++)
                {
                    if ((System.Convert.ToUInt32(m_crc) & 0x80000000) == 0x80000000)
                    {
                        m_crc = (m_crc << 1) ^ 0x04C11DB7;
                    }
                    else
                    {
                        m_crc <<= 1;
                    }
                }
            }
            return String.Format("{0:x8}", m_crc);
        }
        #endregion

        #region ChampModifiable

        /// <summary>
        /// Un champ d'une classe
        /// </summary>
        public class ChampModifiable : INotifyPropertyChanged
        {
            private bool _IsModifiable = false;
            private PropertyInfo _PropertyInfo;

            public bool IsModifiable
            {
                get { return _IsModifiable; }
                set { _IsModifiable = value; OnPropertyChanged("IsModifiable"); }
            }
            public PropertyInfo PropertyInfo
            {
                get { return _PropertyInfo; }
                set { _PropertyInfo = value; OnPropertyChanged("PropertyInfo"); OnPropertyChanged("NomChamp"); }
            }

            public string NomChamp
            {
                get { return _PropertyInfo.Name; }

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

        public static ObservableCollection<Utils.ChampModifiable> GetChampsModifiables(object _Classe)
        {
            ObservableCollection<Utils.ChampModifiable> _ListeChamps = new ObservableCollection<Utils.ChampModifiable>();
            Type _TypeClasse = _Classe.GetType();
            PropertyInfo[] _Properties = _TypeClasse.GetProperties();

            foreach (PropertyInfo t in _Properties)
            {
                Utils.ChampModifiable c = new Utils.ChampModifiable();
                c.PropertyInfo = t;

                if (t.GetValue(_Classe, null) != null)
                {
                    string _valeur = t.GetValue(_Classe, null).ToString();

                    Console.WriteLine(c.NomChamp + " - " + c.PropertyInfo.PropertyType + " - " + _valeur);

                    switch (c.PropertyInfo.PropertyType.ToString())
                    {
                        case "System.String":
                            if ((string)t.GetValue(_Classe, null) == "") _valeur = "";
                            break;

                        case "System.DateTime":
                            if (((DateTime)t.GetValue(_Classe, null)).Year == 1) _valeur = "";
                            break;

                        case "MediaManager.Library.PersonneCollection":
                            if (((PersonneCollection)t.GetValue(_Classe, null)).Count <= 0) _valeur = "";
                            break;

                        case "System.Int32":
                            if (((int)t.GetValue(_Classe, null)) == 0) _valeur = "";
                            break;

                        case "MediaManager.Library.Thumb":
                            if (((Thumb)t.GetValue(_Classe, null)).URLImage == "") _valeur = "";
                            break;

                        case "System.Collections.ObjectModel.ObservableCollection`1[MediaManager.Library.Thumb]":
                            if (((ObservableCollection<Thumb>)t.GetValue(_Classe, null)).Count <= 0) _valeur = "";
                            break;

                        case "System.String[]":
                            if (((string[])t.GetValue(_Classe, null)).Length <= 0) _valeur = "";
                            break;

                        case "System.Single":
                            if (((Single)t.GetValue(_Classe, null)) == 0) _valeur = "";
                            break;

                        default:
                            if (t.GetValue(_Classe, null).ToString() == "") _valeur = "";
                            break;
                    }

                    if (_valeur == "")
                    {
                        c.IsModifiable = true;
                    }
                    else
                    {
                        c.IsModifiable = false;
                    }
                }

                else
                {
                    c.IsModifiable = true;
                }

                if (t.CanWrite) _ListeChamps.Add(c);
            }
            return _ListeChamps;

        }
        #endregion

        #region RemoveAccents
        /// <summary>
        /// Supprime les caractères accentués
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string RemoveAccents(string inputString)
        {
            byte[] tabString = System.Text.Encoding.GetEncoding(1251).GetBytes(inputString);
            return System.Text.Encoding.ASCII.GetString(tabString);
        }
        #endregion


    }
}
