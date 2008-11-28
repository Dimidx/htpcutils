using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;


namespace NiceCovers_Library
{
    /// <summary>
    /// Permet de convertir une affiche de film "normale" en NiceCovers
    /// </summary>
    public class NiceCovers
    {
        /// <summary>
        /// Fusionne une image avec la DvdBox
        /// </summary>
        /// <param name="_FichierCover">Le chemin complet de l'image de base</param>
        /// <returns>l'image fusionnée</returns>
        public static Bitmap Fusion(string _FichierCover)
        {

            Bitmap bDvdBox = new Bitmap(NiceCovers_Library.Properties.Resources.dvdbox);
            Bitmap bCover = new Bitmap(@_FichierCover);
            Bitmap bVide = new Bitmap(571, 720);

            if (VerifNiceCovers(bCover) == false)
            {
                Graphics g = Graphics.FromImage(bVide);
                g.DrawImage(bCover, 81, 24, 458, 655);
                g.DrawImage(bDvdBox, 0, 0, 571, 720);

                g.Save();
                return bVide;
            }
            else
            {
                return bCover;
            }

        }

        private static bool VerifNiceCovers(Bitmap _Bitmap)
        {
            Color _color1 = _Bitmap.GetPixel(60, 10);
            Color _Test1 = Color.FromArgb(133, 76, 76, 76);

            if (_color1 == _Test1)
            {
                return true;
            }
            else
            {
                return false;
            }





        }



        /// <summary>
        /// Lance la conversion de la liste de fichiers (un seul fichier peut être passé)
        /// </summary>
        /// <param name="_ListeFichiers"></param>
        /// <returns>Le dernier nom du fichier généré</returns>
        public static string FusionSave(string _FichierJpg)
        {
            string _suffixe = "_NiceCovers";
            string _NomFichier = "";
            try
            {
                if (File.Exists(_FichierJpg) == true)
                {
                    Bitmap _NiceCovers = NiceCovers.Fusion(_FichierJpg);
                    FileInfo _file = new FileInfo(_FichierJpg);
                    _NomFichier = _file.DirectoryName + "\\" + _file.Name.Replace(_file.Extension, "") + _suffixe + ".png";
                    _NiceCovers.Save(_NomFichier);
                    return _NomFichier;
                }
                else
                {
                    return "";
                }

            }
            catch (Exception e)
            {

                return "";
            }
        }
    }
}
