using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web;



namespace MusicFanartDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            string _urlFanartMusic = "http://www.meediofr.org/~dmdocuments/artist_music/backdrops/";
            string sPath = @args[0];
            List<string> ArtisteSansFanarts = new List<string>();
            int FanartsManquants = 0;
            int FanartsTelecharges = 0;
            int FanartsExistants = 0;

            DirectoryInfo dir = new DirectoryInfo(sPath);
            foreach (DirectoryInfo DirInitiale in dir.GetDirectories())
            {

                foreach (DirectoryInfo DirArtiste in DirInitiale.GetDirectories())
                {

                    string _artist = DirArtiste.Name;
                    string _artistpropre = DirArtiste.Name;
                    Console.WriteLine("-------------------------------------------------------------");
                    Console.WriteLine("Recherche du fanart pour " + _artist);

                    //Chemin Local du fanart
                    string CheminLocal = DirArtiste.FullName + "\\fanart.jpg";

                    //Si le fanart existe déjà on saute
                    if (File.Exists(CheminLocal) != true)
                    {

                        // convertit en minuscule et supprime les accents
                        _artist = _artist.ToLower();
                        // On supprime les accents
                        _artist = System.Text.RegularExpressions.Regex.Replace(_artist, @"(à|á|ä|â|ã|å)+", "a");
                        _artist = System.Text.RegularExpressions.Regex.Replace(_artist, @"(é|è|ê|ë)+", "e");
                        _artist = System.Text.RegularExpressions.Regex.Replace(_artist, @"(ì|í|ï|î)+", "i");
                        _artist = System.Text.RegularExpressions.Regex.Replace(_artist, @"(ò|ó|ö|ô)+", "o");
                        _artist = System.Text.RegularExpressions.Regex.Replace(_artist, @"(ü|û|ù|ú)+", "u");
                        _artist = System.Text.RegularExpressions.Regex.Replace(_artist, @"(ÿ|ý)+", "y");
                        _artist = System.Text.RegularExpressions.Regex.Replace(_artist, @"(ç)+", "c");
                        _artist = System.Text.RegularExpressions.Regex.Replace(_artist, @"(ñ)+", "n");
                        _artist = System.Text.RegularExpressions.Regex.Replace(_artist, @"(-|_|')+", " ");

                        _artist = System.Text.RegularExpressions.Regex.Replace(_artist, @"("""")+", "florent");

                        // permet de faire le remplacement uniquement si c'est le début de la chaine
                        if (_artist.IndexOf("the ") == 0) _artist = System.Text.RegularExpressions.Regex.Replace(_artist, @"(the )+", "");
                        if (_artist.IndexOf("les ") == 0) _artist = System.Text.RegularExpressions.Regex.Replace(_artist, @"(the )+", "");

                        //URL du Fanart
                        string URLFanart = _urlFanartMusic + _artist + ".jpg";

                        WebClient ClientWeb = new WebClient();
                        if (TestURL(URLFanart))
                        {
                            Console.WriteLine("Téléchargement de " + URLFanart);
                            ClientWeb.DownloadFile(URLFanart, CheminLocal);
                            FanartsTelecharges++;
                        }
                        else
                        {
                            URLFanart = "http://www.htbackdrops.com/data/media/1/" + _artist.Replace(" ", "_") + ".jpg";
                            if (TestURL(URLFanart))
                            {
                                Console.WriteLine("Téléchargement de " + URLFanart);
                                ClientWeb.DownloadFile(URLFanart, CheminLocal);
                                FanartsTelecharges++;
                            }
                            else
                            {
                                Console.WriteLine("Aucuns fanart de disponible pour " + _artistpropre);
                                ArtisteSansFanarts.Add(_artistpropre);
                                FanartsManquants++;
                            }

                        }

                    }
                    else
                    {
                        //Fanart déja présent
                        Console.WriteLine("Le fanart existe déjà !");
                        FanartsExistants++;
                    }
                }


            }
            Console.WriteLine("Terminé !");
            Console.WriteLine("Fanarts Téléchargés : "+ FanartsTelecharges.ToString());        
            Console.WriteLine("Fanarts Existants   : "+ FanartsExistants.ToString());    
            Console.WriteLine("Fanarts Manquants : "+ FanartsManquants.ToString());

            //Génère le fichier des artistes manquants
            if (File.Exists("ArtistesManquants.txt"))
            {
                File.Delete("ArtistesManquants.txt");
            }

            string ArtisteListe = "";
            foreach (string Artiste in ArtisteSansFanarts)
            {
                ArtisteListe += Artiste + System.Environment.NewLine;
            }
            File.AppendAllText("ArtistesManquants.txt", ArtisteListe);
            System.Threading.Thread.Sleep(1000);
        }

        #region " TestURL "
        public static Boolean TestURL(string url)
        {
            try
            {
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    myHttpWebResponse.Close();
                    return true;
                }
                else
                {
                    myHttpWebResponse.Close();
                    return false;
                }


            }
            catch
            {
                return false;
            }
        }
        #endregion


    }
}
