using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MovieCovers_Plugin;
using Scraper;

namespace TestScraper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            liste_resultats.Items.Clear();

            MovieCovers_Plugin.main _plugin = new MovieCovers_Plugin.main();
            Movie[] _ListeResultats = _plugin.SearchMovie(sai_recherche.Text);
            if (_ListeResultats.Length != 0)
            {
                foreach(Movie _movie in _ListeResultats)
                {
                    liste_resultats.Items.Add(_movie);
                }
            }




        }
    }
}
