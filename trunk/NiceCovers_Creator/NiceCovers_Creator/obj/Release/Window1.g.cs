﻿#pragma checksum "..\..\Window1.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "80A41B0410D6ED1D78CCDA056BBB6EC4"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :2.0.50727.3053
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace NiceCovers_Creator {
    
    
    /// <summary>
    /// Window1
    /// </summary>
    public partial class Window1 : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 106 "..\..\Window1.xaml"
        internal System.Windows.Media.Animation.BeginStoryboard Story_About_BeginStoryboard;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\Window1.xaml"
        internal System.Windows.Controls.Button BTN_Close;
        
        #line default
        #line hidden
        
        
        #line 119 "..\..\Window1.xaml"
        internal System.Windows.Controls.Border Border_Main;
        
        #line default
        #line hidden
        
        
        #line 120 "..\..\Window1.xaml"
        internal System.Windows.Controls.StackPanel stackPanel;
        
        #line default
        #line hidden
        
        
        #line 122 "..\..\Window1.xaml"
        internal System.Windows.Controls.Image NiceCover;
        
        #line default
        #line hidden
        
        
        #line 144 "..\..\Window1.xaml"
        internal System.Windows.Controls.Border Border_About;
        
        #line default
        #line hidden
        
        
        #line 157 "..\..\Window1.xaml"
        internal System.Windows.Controls.Label LIB_Version;
        
        #line default
        #line hidden
        
        
        #line 161 "..\..\Window1.xaml"
        internal System.Windows.Controls.Label LIB_Titre;
        
        #line default
        #line hidden
        
        
        #line 162 "..\..\Window1.xaml"
        internal System.Windows.Controls.Label LIB_Auteur;
        
        #line default
        #line hidden
        
        
        #line 164 "..\..\Window1.xaml"
        internal System.Windows.Controls.TextBlock LIB_Description;
        
        #line default
        #line hidden
        
        
        #line 171 "..\..\Window1.xaml"
        internal System.Windows.Controls.Button BTN_Charge;
        
        #line default
        #line hidden
        
        
        #line 178 "..\..\Window1.xaml"
        internal System.Windows.Controls.Button BTN_About;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/NiceCovers_Creator;component/window1.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Window1.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Story_About_BeginStoryboard = ((System.Windows.Media.Animation.BeginStoryboard)(target));
            return;
            case 2:
            this.BTN_Close = ((System.Windows.Controls.Button)(target));
            
            #line 116 "..\..\Window1.xaml"
            this.BTN_Close.Click += new System.Windows.RoutedEventHandler(this.BTN_Close_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Border_Main = ((System.Windows.Controls.Border)(target));
            return;
            case 4:
            this.stackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 5:
            this.NiceCover = ((System.Windows.Controls.Image)(target));
            return;
            case 6:
            this.Border_About = ((System.Windows.Controls.Border)(target));
            return;
            case 7:
            this.LIB_Version = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            
            #line 158 "..\..\Window1.xaml"
            ((System.Windows.Documents.Hyperlink)(target)).Click += new System.Windows.RoutedEventHandler(this.LinkClicked);
            
            #line default
            #line hidden
            return;
            case 9:
            this.LIB_Titre = ((System.Windows.Controls.Label)(target));
            return;
            case 10:
            this.LIB_Auteur = ((System.Windows.Controls.Label)(target));
            return;
            case 11:
            this.LIB_Description = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 12:
            this.BTN_Charge = ((System.Windows.Controls.Button)(target));
            
            #line 173 "..\..\Window1.xaml"
            this.BTN_Charge.Click += new System.Windows.RoutedEventHandler(this.BTN_Charge_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.BTN_About = ((System.Windows.Controls.Button)(target));
            
            #line 180 "..\..\Window1.xaml"
            this.BTN_About.Click += new System.Windows.RoutedEventHandler(this.BTN_About_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
