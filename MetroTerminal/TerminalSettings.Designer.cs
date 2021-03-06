﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MetroTerminal {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    public sealed partial class TerminalSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static TerminalSettings defaultInstance = ((TerminalSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new TerminalSettings())));
        
        public static TerminalSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Black")]
        public string terminalBackColor {
            get {
                return ((string)(this["terminalBackColor"]));
            }
            set {
                this["terminalBackColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("#FF41B1E1")]
        public string terminalFontColor {
            get {
                return ((string)(this["terminalFontColor"]));
            }
            set {
                this["terminalFontColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Segoe UI")]
        public string terminalFontFamily {
            get {
                return ((string)(this["terminalFontFamily"]));
            }
            set {
                this["terminalFontFamily"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public double terminalFontSize {
            get {
                return ((double)(this["terminalFontSize"]));
            }
            set {
                this["terminalFontSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>115200</string>
  <string>57600</string>
  <string>38400</string>
  <string>19200</string>
  <string>9600</string>
  <string>1200</string>
  <string>300</string>
  <string>921600</string>
  <string>460800</string>
  <string>230400</string>
  <string>4800</string>
  <string>2400</string>
  <string>150</string>
  <string>110</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection baudRates {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["baudRates"]));
            }
            set {
                this["baudRates"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int lastSelectedBaudRateIndex {
            get {
                return ((int)(this["lastSelectedBaudRateIndex"]));
            }
            set {
                this["lastSelectedBaudRateIndex"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int dataBitsIndex {
            get {
                return ((int)(this["dataBitsIndex"]));
            }
            set {
                this["dataBitsIndex"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int stopBitsIndex {
            get {
                return ((int)(this["stopBitsIndex"]));
            }
            set {
                this["stopBitsIndex"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int parityIndex {
            get {
                return ((int)(this["parityIndex"]));
            }
            set {
                this["parityIndex"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml")]
        public string colorUri {
            get {
                return ((string)(this["colorUri"]));
            }
            set {
                this["colorUri"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml")]
        public string backColorUri {
            get {
                return ((string)(this["backColorUri"]));
            }
            set {
                this["backColorUri"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool alwaysOnTop {
            get {
                return ((bool)(this["alwaysOnTop"]));
            }
            set {
                this["alwaysOnTop"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool autoUpdate {
            get {
                return ((bool)(this["autoUpdate"]));
            }
            set {
                this["autoUpdate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public double opacity {
            get {
                return ((double)(this["opacity"]));
            }
            set {
                this["opacity"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool instantScroll {
            get {
                return ((bool)(this["instantScroll"]));
            }
            set {
                this["instantScroll"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool displayUpdateIcon {
            get {
                return ((bool)(this["displayUpdateIcon"]));
            }
            set {
                this["displayUpdateIcon"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool autoInstallNewUpdates {
            get {
                return ((bool)(this["autoInstallNewUpdates"]));
            }
            set {
                this["autoInstallNewUpdates"] = value;
            }
        }
    }
}
