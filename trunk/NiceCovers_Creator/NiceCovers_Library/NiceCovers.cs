using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;


namespace NiceCovers_Library
{
    public class NiceCovers
    {
        private static byte[] ConvertImageToByteArray(Bitmap img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            return ms.ToArray();
        }

        public static Bitmap Fusion(string _FichierCover)
        {
            Bitmap bDvdBox = new Bitmap(NiceCovers_Library.Properties.Resources.dvdbox);
            Bitmap bCover = new Bitmap(@_FichierCover);


            Bitmap bVide = new Bitmap(571, 720);

     
            //BitmapSource img = BitmapFrame.Create(new Uri(@"D:\Perso\Dev\HTPCUtils\NiceCovers_Creator\NiceCovers_Library\Resources\Vide.png"));
            //BitmapMetadata meta = (BitmapMetadata)img.Metadata;

            //meta.Title = "Tutu";
            //meta.SetQuery("/Text/Description", "test de moi");

            Stream ms = new MemoryStream();
            bVide.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            PngBitmapEncoder png = new PngBitmapEncoder();

            png.Metadata = new BitmapMetadata("png"); //error - Property
            png.Metadata.ApplicationName = "Moi";

            //png.Metadata.SetQuery("/tEXt/Software", "moi");

            png.Frames.Add(BitmapFrame.Create(ms));
            using (Stream stm = File.Create("c:/foo.png"))
            {
                png.Save(stm);
                bVide = new Bitmap(stm);
            }



            ms.Close();


            Graphics g = Graphics.FromImage(bVide);
            g.DrawImage(bCover, 81, 24, 458, 655);
            g.DrawImage(bDvdBox, 0, 0, 571, 720);
            g.Save();


            return bVide;
        }

        public static string FusionSave(string[] _ListeFichiers)
        {
            string _suffixe = "_NiceCovers";
            string _NomFichier = "";
            try
            {
                if (_ListeFichiers.Length != 0)
                {
                    foreach (string _FileCover in _ListeFichiers)
                    {
                        Bitmap _NiceCovers = NiceCovers.Fusion(_FileCover);
                        FileInfo _file = new FileInfo(_FileCover);
                        _NomFichier = _file.DirectoryName + "\\" + _file.Name.Replace(_file.Extension, "") + _suffixe + ".png";
                        _NiceCovers.Save(_NomFichier);
                    }
                    return _NomFichier;
                }
                return "";
            }
            catch (Exception)
            {

                return "";
            }
        }

        public class DonneeExif
        {
            //"UserComment" id="0x9286"
            //"Software" id="0x131"
            //"Artist" id="0x13B"

            public string Artist = "Danone-KiD";
            public string Software = "";
            public string UserComment = "";

            public DonneeExif GetExif(Bitmap _Bitmap)
            {
                DonneeExif _DonneesExif = new DonneeExif();

                _DonneesExif.Artist = GetExifString(_Bitmap, 0x13B);
                _DonneesExif.Software = GetExifString(_Bitmap, 0x131);
                _DonneesExif.UserComment = GetExifString(_Bitmap, 0x9286);

                return _DonneesExif;

            }

            public Bitmap SetExif(Bitmap _Bitmap, DonneeExif _DonneesExif)
            {
                //DonneeExif _DonneesExif = new DonneeExif();

                //_DonneesExif.Artist = GetExifString(_Bitmap, 0x13B);
                //_DonneesExif.Software = GetExifString(_Bitmap, 0x131);
                //_DonneesExif.UserComment = GetExifString(_Bitmap, 0x9286);


                //Encoding _Encoding = Encoding.UTF8;
                //Bitmap theImage = _Bitmap;
                PropertyItem[] propItems = _Bitmap.PropertyItems;
                Encoding _Encoding = Encoding.UTF8;
                //Image theImage = new Bitmap(m_currImageFileTemp);
                //ropertyItem propItem = null;
                //_Bitmap.GetPropertyItem(0x9286);
                try
                {
                    propItems[0].Id = 0x9286;
                    propItems[0].Type = 2;
                    propItems[0].Len = _DonneesExif.Artist.Length;
                    propItems[0].Value = _Encoding.GetBytes(_DonneesExif.UserComment + '\0');

                    _Bitmap.SetPropertyItem(propItems[0]);
                }
                catch (Exception e)
                {

                    string toto = e.Message;

                }

                //theImage.Save(m_currImageFile);




                return _Bitmap;

            }

            private string GetExifString(Bitmap _Bitmap, int _HexExif)
            {
                string _DataExif = "";

                try
                {
                    System.Text.ASCIIEncoding Value = new System.Text.ASCIIEncoding();
                    _DataExif = Value.GetString(_Bitmap.GetPropertyItem(_HexExif).Value);

                }
                catch (ArgumentException e)
                {

                }
                return _DataExif;
            }
        }
    }
}
