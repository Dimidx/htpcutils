﻿using System;
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
                System.Windows.Media.Imaging.BitmapImage bitz = null;

                    try
                    {
                    String toConvert = (String)value;

                        bitz = new System.Windows.Media.Imaging.BitmapImage(new Uri(toConvert));
                    }
                    catch 
                    {
                        bitz = new System.Windows.Media.Imaging.BitmapImage();
                    }
                
                return bitz;

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