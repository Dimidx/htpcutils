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

namespace Converters
{



    #region UriToImageConverter
    public class UriToImageConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null | value == "" )
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
                bi.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                bi.DecodePixelWidth = 300;
                //bi.DecodePixelHeight = 60;  
                bi.CacheOption = BitmapCacheOption.OnLoad;
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
            throw new Exception("The method or operation is not implemented.");
        }


    }
    #endregion

    #region PersonneCollectionToString
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

}
