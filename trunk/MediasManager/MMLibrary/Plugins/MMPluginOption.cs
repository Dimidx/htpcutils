using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MediaManager.Plugins
{

    [Serializable]
    [System.Xml.Serialization.XmlRoot()]
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


            public MMPluginOption()
            {
                _Choices = new List<string>();
            }


            public enum EnumDataType { Boolean = 1, String = 2, List = 3 }
        

    }
}
