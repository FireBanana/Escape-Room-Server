﻿#pragma checksum "..\..\..\Windows\RoomScoreboard.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "E051DCBD90B286C1920CA9F6A7436F95E17C3213CDE2C21C07113D9F2D5092DB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
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
using System.Windows.Shell;
using Unity_Escape_Room_Server_WPF.Windows;


namespace Unity_Escape_Room_Server_WPF.Windows {
    
    
    /// <summary>
    /// RoomScoreboard
    /// </summary>
    public partial class RoomScoreboard : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\..\Windows\RoomScoreboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Unity_Escape_Room_Server_WPF.Windows.RoomScoreboard ScoreboardWindow;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\Windows\RoomScoreboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label ScoreText;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\Windows\RoomScoreboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label RemainingTimeText;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\Windows\RoomScoreboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label TeamName;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\Windows\RoomScoreboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox HintBotText;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Unity-Escape-Room-Server-WPF;component/windows/roomscoreboard.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Windows\RoomScoreboard.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ScoreboardWindow = ((Unity_Escape_Room_Server_WPF.Windows.RoomScoreboard)(target));
            
            #line 8 "..\..\..\Windows\RoomScoreboard.xaml"
            this.ScoreboardWindow.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.OnDoubleClick);
            
            #line default
            #line hidden
            
            #line 8 "..\..\..\Windows\RoomScoreboard.xaml"
            this.ScoreboardWindow.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.OnMouseDown);
            
            #line default
            #line hidden
            
            #line 8 "..\..\..\Windows\RoomScoreboard.xaml"
            this.ScoreboardWindow.Closed += new System.EventHandler(this.OnWindowClosed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ScoreText = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.RemainingTimeText = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.TeamName = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.HintBotText = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

