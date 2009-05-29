using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using MediaManager.Library;
using MediaManager.Plugins;

namespace MediaManager
{
    /// <summary>
    /// Logique d'interaction pour PluginOptions.xaml
    /// </summary>
    public partial class PluginOptions : Window
    {


        private MMPluginScraper MonPlug;

        public PluginOptions()
        {
            InitializeComponent();
            MonPlug = new MMPluginScraper(@"C:\Documents and Settings\Administrateur\Mes documents\Dev\HTPCUtils\MediasManager\MediasManager\bin\Debug\Scraper\Movies\Allocine.dll");
            MonPlug.SaveOptions();

            List<MMPluginOption> plugin = new List<MMPluginOption>();

            MMPluginOption p = new MMPluginOption();
            p.Caption = "Test d'option";
            p.Name = "test";
            p.HelpText = "Une aide";
            p.DataType = MMPluginOption.EnumDataType.String;
            plugin.Add(p);

            p = new MMPluginOption();
            p.Caption = "Test d'option bool";
            p.Name = "test";
            p.HelpText = "Une aide";
            p.DataType = MMPluginOption.EnumDataType.List;
            p.Choices.Add("Choix 1");
            p.Choices.Add("Choix 2");
            p.Choices.Add("Choix 3");
            p.Choices.Add("Choix 4");
            p.Choices.Add("Choix 5");
            plugin.Add(p);

            p = new MMPluginOption();
            p.Caption = "Test d'option list";
            p.Name = "test";
            p.HelpText = "Une aide";
            p.DataType = MMPluginOption.EnumDataType.Boolean;
            plugin.Add(p);

            grid.ShowGridLines = true;

            int _NumLigne = 0;

            foreach (MMPluginOption option in plugin)
            {
                RowDefinition ligne = new RowDefinition();
                ligne.Height = new GridLength(35);
                grid.RowDefinitions.Add(ligne);

                //Aide
                ToolTip _ToolTip = new ToolTip();
                _ToolTip.Content = option.HelpText;

                //Caption
                TextBlock _TextBlock = new TextBlock();
                _TextBlock.Text = option.Caption;
                _TextBlock.ToolTip = _ToolTip;
                _TextBlock.Margin = new Thickness(5,5,5,5);
                _TextBlock.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetColumn(_TextBlock, 0);
                Grid.SetRow(_TextBlock, _NumLigne);
                grid.Children.Add(_TextBlock);

                switch (option.DataType)
                {
                    case MMPluginOption.EnumDataType.String:
                        //L'option en elle meme
                        TextBox t = new TextBox();
                        t.Text = option.Caption;
                        t.ToolTip = _ToolTip;
                        t.Margin = new Thickness(5, 5, 5, 5);
                        t.VerticalAlignment = VerticalAlignment.Center;
                        Grid.SetColumn(t, 1);
                        Grid.SetRow(t, _NumLigne);
                        grid.Children.Add(t);

                        break;

                    case MMPluginOption.EnumDataType.Boolean:
                        //L'option en elle meme
                        CheckBox _CheckBox = new CheckBox();
                        _CheckBox.ToolTip = _ToolTip;
                        _CheckBox.Margin = new Thickness(5, 5, 5, 5);
                        _CheckBox.VerticalAlignment = VerticalAlignment.Center;
                        Grid.SetColumn(_CheckBox, 1);
                        Grid.SetRow(_CheckBox, _NumLigne);
                        grid.Children.Add(_CheckBox);
                        break;

                    case MMPluginOption.EnumDataType.List:

                        ComboBox _ComboBox = new ComboBox();
                        _ComboBox.ToolTip = _ToolTip;
                        _ComboBox.Margin = new Thickness(5, 5, 5, 5);
                        _ComboBox.VerticalAlignment = VerticalAlignment.Center;
                        if (option.Choices != null)
                        {
                            _ComboBox.ItemsSource = option.Choices;
                        }

                        Grid.SetColumn(_ComboBox, 1);
                        Grid.SetRow(_ComboBox, _NumLigne);
                        grid.Children.Add(_ComboBox);
                        break;

                    default:
                        break;

                }
                _NumLigne++;


            }


            grid.RowDefinitions.Add(new RowDefinition());



        }
    }
}
