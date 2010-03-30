using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using EmberAPI;
using System.Windows.Threading;
using System.Windows;
using System.Threading;

namespace CinePassion
{
    public class ScraperCP : Interfaces.EmberMovieScraperModule
    {
        private string _Name = "CinePassion";
        private string _AssemblyName;
        private frmSetupScraper _setup;
        private bool _ScraperEnabled = false;
        private bool _PostScraperEnabled = false;
        private Film _FilmScrape;
        #region Properties

        //Nom du scraper
        public string ModuleName
        {
            get { return _Name; }
        }

        //Version
        public string ModuleVersion
        {
            get { return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FilePrivatePart.ToString(); }
        }


        //Si c'est un scraper
        public bool IsScraper
        {
            get { return true; }
        }

        //Si c'est un post scraper
        public bool IsPostScraper
        {
            get { return true; }
        }

        //Si l'utilisateur à activé le Scraper
        public bool ScraperEnabled
        {
            get
            {
                return _ScraperEnabled;
            }

            set
            {
                _ScraperEnabled = value;
            }
        }

        //Si l'utilisateur à activé le PostScraper
        public bool PostScraperEnabled
        {
            get
            {
                return _PostScraperEnabled;
            }

            set
            {
                _PostScraperEnabled = value;
            }
        }

        #endregion


        public void Init(string sAssemblyName)
        {
            _AssemblyName = sAssemblyName;
        }

        public Containers.SettingsPanel InjectSetupPostScraper()
        {
            Containers.SettingsPanel Spanel = new Containers.SettingsPanel();
            return (Spanel);
        }

        public Containers.SettingsPanel InjectSetupScraper()
        {
            Containers.SettingsPanel Spanel = new Containers.SettingsPanel();
            
            _setup = new frmSetupScraper();
            _setup.cbEnabled.Checked = _ScraperEnabled;
            Spanel.Name = string.Concat(this._Name, "Scraper");
            Spanel.Text = _Name;
            Spanel.Prefix = "NativeMovieInfo_";
            Spanel.Order = 110;
            Spanel.Parent = "pnlMovieData";
            Spanel.Type = Master.eLang.GetString(36, "Movies");
            if (_ScraperEnabled)
            {
                Spanel.ImageIndex = 9;
            }
            else
            {
                Spanel.ImageIndex = 10;
            }
            Spanel.Panel = _setup.pnlSettings;

            _setup.SetupScraperChanged += Handle_SetupScraperChanged;
            
            //_setup.ModuleSettingsChanged += Handle_ModuleSettingsChanged;

            return Spanel;

            //Return (Spanel)

        }

        private void NewWindowHandler(EmberAPI.MediaContainers.Movie DBMovie)
        {
            //Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            //Thread newWindowThread = System.Windows.Threading.Dispatcher.CurrentDispatcher.Thread;

            //newWindowThread.SetApartmentState(ApartmentState.STA);
            //newWindowThread.IsBackground = false;
         
            //newWindowThread.Start();

            Thread workerThread = new Thread(new ParameterizedThreadStart(ShowMyWindow));
            workerThread.SetApartmentState(ApartmentState.STA);
            workerThread.Start(DBMovie);
            workerThread.Join();

        }

        private void ShowMyWindow(object DBMovie)
        {
            dlgResults tempWindow = new dlgResults((EmberAPI.MediaContainers.Movie)DBMovie);
            tempWindow.ShowDialog();
            
            if (tempWindow._ResultMovie != null)
            {
                _FilmScrape = tempWindow._ResultMovie;
            }

            //System.Windows.Threading.Dispatcher.Run();
            //while (System.Windows.Threading.Dispatcher.CurrentDispatcher.Thread.IsAlive)
	{
	         
	}
            
        }

        public Interfaces.ModuleResult Scraper(ref Structures.DBMovie DBMovie, ref Enums.ScrapeType ScrapeType, ref Structures.ScrapeOptions Options)
        {
            //dlgSearchResult test = new dlgSearchResult(DBMovie.Movie);
            //test.ShowDialog();
            //NewWindowHandler();
            NewWindowHandler(DBMovie.Movie);
            if (_FilmScrape != null)
            {
                DBMovie.Movie.Title = _FilmScrape.Title;
                DBMovie.Movie.OriginalTitle = _FilmScrape.OriginalTitle;
                DBMovie.Movie.Plot = _FilmScrape.Plot;
                DBMovie.Movie.Year = _FilmScrape.Year;
                DBMovie.Movie.Runtime = _FilmScrape.Runtime;
                DBMovie.Movie.ID = _FilmScrape.Id_IMDB;
            }
            
            Interfaces.ModuleResult test = new Interfaces.ModuleResult();
            test.breakChain = false;
            _FilmScrape = null;
            return test;
        }

        public Interfaces.ModuleResult SelectImageOfType(ref Structures.DBMovie DBMovie, Enums.ImageType _DLType, ref Containers.ImgResult pResults,  bool _isEdit, bool preload)
        {
            Interfaces.ModuleResult test = new Interfaces.ModuleResult();
            test.breakChain = true;

            return test;
        }


        public void SaveSetupPostScraper(bool DoDispose)
        {

        }

        public void SaveSetupScraper(bool DoDispose)
        {

            ModulesManager.Instance.SaveSettings();

            if (DoDispose)
            {
                _setup.SetupScraperChanged -= Handle_SetupScraperChanged;
                //_setup.ModuleSettingsChanged -= Handle_ModuleSettingsChanged;
                _setup.Dispose();
            }

        }


        public bool QueryPostScraperCapabilities(Enums.PostScraperCapabilities cap)
        {
            return true;
        }

        public Interfaces.ModuleResult PostScraper(ref Structures.DBMovie DBMovie, Enums.ScrapeType ScrapeType)
        {
            return new Interfaces.ModuleResult();
        }

        public Interfaces.ModuleResult GetMovieStudio(ref Structures.DBMovie DBMovie, ref System.Collections.Generic.List<string> sStudio)
        {
            return new Interfaces.ModuleResult();
        }

        public Interfaces.ModuleResult DownloadTrailer(ref Structures.DBMovie DBMovie, ref string sURL)
        {
            return new Interfaces.ModuleResult();
        }

        #region Events

        public event Interfaces.EmberMovieScraperModule.MovieScraperEventEventHandler MovieScraperEvent;

        public event Interfaces.EmberMovieScraperModule.ModuleSettingsChangedEventHandler ModuleSettingsChanged;
        private void Handle_ModuleSettingsChanged()
        {
            if (ModuleSettingsChanged != null)
            {
                ModuleSettingsChanged();
            }

        }

        private void Handle_PostModuleSettingsChanged()
        {
            if (ModuleSettingsChanged != null)
            {
                ModuleSettingsChanged();
            }
        }

        public event Interfaces.EmberMovieScraperModule.PostScraperSetupChangedEventHandler PostScraperSetupChanged;
        private void Handle_SetupPostScraperChanged(bool state, int difforder)
        {
            PostScraperEnabled = state;
            if (PostScraperSetupChanged != null)
            {
                PostScraperSetupChanged(string.Concat(this._Name, "PostScraper"), state, difforder);
            }
        }

        public event Interfaces.EmberMovieScraperModule.ScraperSetupChangedEventHandler ScraperSetupChanged;
        private void Handle_SetupScraperChanged(bool state, int difforder)
        {
            ScraperEnabled = state;
            if (ScraperSetupChanged != null)
            {

                ScraperSetupChanged(string.Concat(this._Name, "Scraper"), state, difforder);

            }
        }
        
        #endregion


    }
}
