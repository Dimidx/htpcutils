using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using NiceCovers_Library;


namespace NiceCovers_Cmd
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length != 0)
            {
                string _fichier = args[0];
                string _result = "";
                if (File.Exists(_fichier))
                {
                    FileInfo _fichierJpg = new FileInfo(_fichier);
                    
                    Console.WriteLine("Convertion de " + _fichierJpg.Name + " vers "+_fichierJpg.Name.Replace(_fichierJpg.Extension, "") + "_NiceCovers.png");
                    _result = NiceCovers.FusionSave(_fichier);
                    if (_result == "")
                    {
                        Console.WriteLine("--Erreur--");
                    }
                }
                else
                {
                    if (Directory.Exists(_fichier))
                    {
                        foreach (string _sfic in Directory.GetFiles(_fichier))
                        {
                            FileInfo _fic = new FileInfo(_sfic);
                            if ((_fic.Extension.ToLower() == ".jpg") || (_fic.Extension.ToLower() == ".png"))
                            {
                                Console.WriteLine("Convertion de " + _fic.Name + " vers " + _fic.Name.Replace(_fic.Extension, "") + "_NiceCovers.png");
                                _result = NiceCovers.FusionSave(_sfic);
                                if (_result == "")
                                {
                                    Console.WriteLine("--Erreur--");
                                }

                            }

                        }
                       
                    }
                    else
                    {
                        Console.WriteLine("Erreur : Le fichier " + _fichier + " n'existe pas !");
                    }


                }
                
            }
            else
            {
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("Syntaxe : NiceCovers_Cmd NomDuFichier");
                Console.WriteLine("Syntaxe : NiceCovers_Cmd NomDuDossier");
                Console.WriteLine("------------------------------------------------------");


            }

            

        }
    }
}
