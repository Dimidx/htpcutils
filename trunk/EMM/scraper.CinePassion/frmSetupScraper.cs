using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EmberAPI;

namespace CinePassion
{
    public partial class frmSetupScraper : Form
    {
        public delegate void ScraperSetupChangedHandler( bool State, int difforder);
        public event ScraperSetupChangedHandler SetupScraperChanged;
    
        public frmSetupScraper()
        {
            InitializeComponent();
        }

        private void cbEnabled_CheckedChanged(object sender, EventArgs e)
        {
                if (SetupScraperChanged != null)
               {
                   SetupScraperChanged(cbEnabled.Checked, 0);
               }
        }

        //private void cbEnabled_CheckedChanged(object sender, System.EventArgs e)
        //{
        //    if (SetupScraperChanged != null)
        //    {
        //        SetupScraperChanged(cbEnabled.Checked, 0);
        //    }

        //}
    }
}
