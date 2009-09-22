using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.IO;
using System.Threading;
using System.Collections.ObjectModel;
using MediaManager;
using MediaManager.Library;
using System.Globalization;
using System.Xml.Linq;
using System.Linq;

namespace MediaManager.Converters
{

    #region DateTimeConverter
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value,
                           Type targetType,
                           object parameter,
                           CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            return date.ToString("d");
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            string strValue = value.ToString();
            DateTime resultDateTime;
            if (DateTime.TryParse(strValue, out resultDateTime))
            {
                return resultDateTime;
            }
            return value;
        }
    }
    #endregion

    #region UriToImageConverter
    public class UriToImageConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null | value == "")
            {
                return null;
            }

            if (value is string)
            {
                value = new Uri((string)value);
            }

            if (value is Uri)
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.CreateOptions = BitmapCreateOptions.IgnoreColorProfile | BitmapCreateOptions.DelayCreation;
                bi.DecodePixelWidth = 300;
                //bi.DecodePixelHeight = 60;  
                bi.CacheOption = BitmapCacheOption.OnDemand;
                bi.UriSource = (Uri)value;
                bi.EndInit();
                return bi;



            }

            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }


    #endregion

    #region ImageToImageSource
    /// <summary>
    /// Converter use to convert a System.Drawing.Image to an BitmapImage, when you need an ImageSource
    /// </summary>
    public class ToImage : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BitmapImage ImgSource = null;

            System.Drawing.Image image = value as System.Drawing.Image;

            if (image != null)
            {
                ImgSource = new BitmapImage();
                ImgSource.BeginInit();

                ImgSource.StreamSource = new MemoryStream(ConvertImageToByteArray(image));

                ImgSource.EndInit();
            }

            return ImgSource;
        }

        /// <summary>
        /// Convert a System.Drawing.Image to a byte array
        /// </summary>
        /// <param name="img">The System.Drawing.Image to convert in a byte array</param>
        /// <returns>A byte array</returns>
        private byte[] ConvertImageToByteArray(System.Drawing.Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            return ms.ToArray();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
    #endregion

    #region StringToImageSource
    /// <summary>
    /// Convertie un chemin string en ImageSource
    /// </summary>
    public class StringToImageSource : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            String toConvert = (String)value;
            BitmapImage bi3 = new BitmapImage();

            try
            {
                if (toConvert != null)
                {
                    bi3.BeginInit();
                    bi3.UriSource = new Uri(toConvert, UriKind.RelativeOrAbsolute);
                    bi3.EndInit();
                }
            }
            catch (Exception)
            {
                bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri("Images/defaultVideoBigPoster.png", UriKind.Relative);
                bi3.EndInit();
                //throw;
            }
            return bi3;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
    #endregion

    #region AvisToImage
    /// <summary>
    /// Convertie un chemin string en ImageSource
    /// </summary>
    public class AvisToImage : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string toConvert = "--aucun--";
            if (!String.IsNullOrEmpty((string)value)) { toConvert = (string)value; } else { toConvert = "--aucun--"; }
            toConvert = Utils.RemoveAccents(toConvert).Trim();

            BitmapImage bi3 = new BitmapImage();
            string _fimage = "";
            //Charge le fichier XML des avis
            XDocument _RatingsXML = XDocument.Load("./Images/Ratings/Ratings.xml");
            if (_RatingsXML.Nodes() != null)
            {
                //Cherche l'avis dans le XML
                var xAvis = from xDef in _RatingsXML.Descendants("name")
                            where toConvert.Contains(xDef.Attribute("searchstring").Value) == true
                              select (string)xDef.Element("icon");
                foreach (string name in xAvis)
                {
                    _fimage = name;

                }

            }
            if (String.IsNullOrEmpty(_fimage))
            {
                _fimage = "./Images/blank.png";
            }
            else
            {
                _fimage = "./Images/Ratings/" + _fimage;
            }

            bi3.BeginInit();
            bi3.UriSource = new Uri(_fimage, UriKind.Relative);
            bi3.EndInit();
            bi3.Freeze();
            return bi3;



        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
    #endregion

    #region TableauToString
    /// <summary>
    /// Convertie un Tableau de chaine en une chaine séparée par des ", "
    /// </summary>
    public class TableauToString : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string _TableauSepare = "";
            if (value == null) return "";
            string[] _TableauOrigine = value as string[];
            if (_TableauOrigine.Length != 0)
            {
                foreach (string item in _TableauOrigine)
                {
                    _TableauSepare += item + ", ";
                }
                if (_TableauSepare.Length > 0) _TableauSepare = _TableauSepare.Substring(0, _TableauSepare.Length - 2);
            }
            return _TableauSepare;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
         

            if ( string.IsNullOrEmpty(value.ToString())) return null;

            string[] _TableauTemp = value.ToString().Split(',');
            string[] _TableauFinal = new string[_TableauTemp.Length];
            //Nettoyage des espaces
            int i = 0;
            if (_TableauTemp.Length != 0)
            {
                foreach (string item in _TableauTemp)
                {
                    _TableauFinal[i] = item.Trim();
                    i++;
                }
            }
            return _TableauFinal;

            
        }


    }
    #endregion

    #region PersonneToString
    /// <summary>
    /// Convertie un Collection de chaine en une chaine séparée par des ", "
    /// </summary>
    public class PersonneToString : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string _TableauSepare = string.Empty;

            PersonneCollection _TableauOrigine = value as PersonneCollection;
            if (_TableauOrigine.Count != 0)
            {
                foreach (Personne item in _TableauOrigine)
                {
                    _TableauSepare += item.Nom + ", ";
                }
                if (_TableauSepare.Length > 0) _TableauSepare = _TableauSepare.Substring(0, _TableauSepare.Length - 2);
            }
            return _TableauSepare;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }


    }
    #endregion

    #region StudioToImage
    /// <summary>
    /// Convertie un chemin string en ImageSource
    /// </summary>
    public class StudioToImage : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string toConvert = "--aucun--";
            if (!String.IsNullOrEmpty((string)value)) { toConvert = (string)value; } else { toConvert = "--aucun--"; }
            toConvert = Utils.RemoveAccents(toConvert).Trim().ToLower();

            BitmapImage bi3 = new BitmapImage();
            string _fimage = "";
            //Charge le fichier XML des studios
            XDocument _StudiosXML = XDocument.Load("./Images/Studios/Studios.xml");
            if (_StudiosXML.Nodes() != null)
            {
                //Cherche le studio dans le XML
                var xStudio = from xDef in _StudiosXML.Descendants("name")
                              where xDef.Attribute("searchstring").Value.Contains(toConvert) == true
                              select (string)xDef.Element("icon");
                foreach (string name in xStudio)
                {
                    _fimage = name;

                }
                
                //Si pas d'image trouvé on prend celle par defaut
                if (_fimage == "")
                {
                    var xDefaut = from xDef in _StudiosXML.Descendants("default")
                                  select (string)xDef.Element("icon");
                    foreach (string name in xDefaut)
                    {
                        _fimage = name;

                    }
                
                }


            
            }

            _fimage = "./Images/Studios/" + _fimage;

            bi3.BeginInit();
            bi3.UriSource = new Uri(_fimage, UriKind.Relative);
            bi3.EndInit();
            bi3.Freeze();
            return bi3;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
    #endregion

}
