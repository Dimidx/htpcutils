using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
            //Scraper.Scraper _scrap = new Scraper.Scraper();

            Plugin _plugin = new Plugin();
            if (_plugin.Load(@"D:\Mes Projets\MoviesManager\Exe\Plugins\MovieCovers_Plugin.dll"))
            {
            
            }
            




            //ScraperPlugin titi = (ScraperPlugin)Scraper.Scraper.CreateInstance(_plugins[0]);

            //string test = titi.PluginName;

        }
    }
}
