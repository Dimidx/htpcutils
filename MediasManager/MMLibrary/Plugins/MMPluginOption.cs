using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace MediaManager.Plugins
{

    [Serializable]
    [System.Xml.Serialization.XmlRoot(ElementName="Option")]
    public class MMPluginOption
    {

            private EnumDataType _DataType;
            /// <summary>
            /// Le type d'option
            /// </summary>
            public EnumDataType DataType
            {
                get { return _DataType; }
                set
                {
                    _DataType = value;
                }
            }

            private string _HelpText;
            /// <summary>
            /// Une aide pour l'option
            /// </summary>
            public string HelpText
            {
                get { return _HelpText; }
                set
                {
                    _HelpText = value;
                }
            }

            private string _Name;
            /// <summary>
            /// le nom de l'option
            /// </summary>
            public string Name
            {
                get { return _Name; }
                set
                {
                    _Name = value;
                }
            }

            private bool _IsMandatory;
            /// <summary>
            /// True si l'option est obligatoire
            /// </summary>
            public bool IsMandatory
            {
                get { return _IsMandatory; }
                set
                {
                    _IsMandatory = value;
                }
            }

            private string _GroupCaption;
            /// <summary>
            /// Permet de grouper les options dans des sections
            /// </summary>
            public string GroupCaption
            {
                get { return _GroupCaption; }
                set
                {
                    _GroupCaption = value;
                }
            }

            private string _Caption;
            /// <summary>
            /// Libellé de l'option
            /// </summary>
            public string Caption
            {
                get { return _Caption; }
                set
                {
                    _Caption = value;
                }
            }

            private List<string> _Choices;
            /// <summary>
            /// Choix multiples
            /// </summary>
            public List<string> Choices
            {
                get { return _Choices; }
                set
                {
                    _Choices = value;
                }
            }

            private  object _Value;
            /// <summary>
            /// La valeur
            /// </summary>
            public object Value
            {
                get { return _Value; }
                set
                {
                    _Value = value;
                }
            }

            private object _DefaultValue;
            /// <summary>
            /// La valeur par defaut
            /// </summary>
            public object DefaultValue
            {
                get { return _DefaultValue; }
                set
                {
                    _DefaultValue = value;
                }
            }


            public MMPluginOption()
            {
                _Choices = new List<string>();
            }


            public enum EnumDataType { Boolean = 1, String = 2, List = 3 }
        

    }

    public class MMPluginOptionCollection : List<MMPluginOption>
    {
        private IMMPlugin _Plugin;
        private List<MMPluginOption> _PluginOptions = new List<MMPluginOption>(); //Liste des options ecrites dans le plugin
        private List<MMPluginOption> _XMLOptions = new List<MMPluginOption>(); //Liste des options ecrites dans le fichier XML
 

        public MMPluginOptionCollection(IMMPlugin Plugin)
        {
            _Plugin = Plugin;

            //Charge les options du fichier XML
            LoadOptions();

            //Demande au plugin ces options
            _PluginOptions = _Plugin.LoadOptions();
            if (_PluginOptions != null)
            {
                foreach (var item in _PluginOptions)
                {
                    object _val = GetValue(item.Name, _XMLOptions);
                    item.Value = _val;
                    this.Add(item);
                }
            }
            //Affecte les options aux plugins
            _Plugin.Options = this;
            Console.WriteLine("Options chargée");
            SaveOptions();

        }
        public object GetValue(string OptionName)
        {
            return GetValue(OptionName, this);
        }


        public object GetValue(string OptionName,List<MMPluginOption> ListOptions)
        {
            object Valeur = new object();

            foreach (MMPluginOption item in ListOptions)
            {
                if (item.Name.Equals(OptionName, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (item.Value != null)
                    {
                        Valeur = item.Value;
                    }
                    if (Valeur == null)
                    {
                        Valeur = item.DefaultValue;
                    }
                    if (Valeur == null)
                    {
                        Valeur = "";
                    }
                    
                }
            }

            return Valeur;
        }


        public bool SaveOptions()
        {
            string _CheminConfig = System.Environment.CurrentDirectory + @"\Plugins\" + _Plugin.Name + ".xml";
            TextWriter w = new StreamWriter(@_CheminConfig);
            XmlSerializer xmlSerial = new XmlSerializer(typeof(List<MMPluginOption>));
            xmlSerial.Serialize(w, this);
            w.Close();
            return true;
        }

        private void LoadOptions()
        {
            string _CheminConfig = System.Environment.CurrentDirectory + @"\Plugins\" + _Plugin.Name + ".xml";

            if (File.Exists(_CheminConfig))
            {
                try
                {
                    TextReader r = new StreamReader(_CheminConfig);
                    XmlSerializer xmlSerial = new XmlSerializer(typeof(List<MMPluginOption>));
                    _XMLOptions = (List<MMPluginOption>)xmlSerial.Deserialize(r);
                    r.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR in: " + _CheminConfig + e.Message);
                    //return null;
                }
            }
        }


    }
}
