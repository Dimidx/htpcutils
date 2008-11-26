using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace NiceCovers_Library
{
    public class NiceCovers
    {

        public static Bitmap Fusion(string _FichierCover)
        {
            Bitmap bDvdBox = new Bitmap(NiceCovers_Library.Properties.Resources.dvdbox);
            Bitmap bCover = new Bitmap(@_FichierCover);
            Bitmap bVide = new Bitmap(NiceCovers_Library.Properties.Resources.Vide);
            Graphics g = Graphics.FromImage(bVide);

            g.DrawImage(bCover, 81, 24, 458, 655);
            g.DrawImage(bDvdBox, 0, 0, 571, 720);

            g.Save();

            return bVide;
        }

    }
}
