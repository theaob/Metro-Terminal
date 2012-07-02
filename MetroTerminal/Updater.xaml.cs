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
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace MetroTerminal
{
    /// <summary>
    /// Interaction logic for Updater.xaml
    /// </summary>
    public partial class Updater : MetroWindow
    {
        public Updater()
        {
            InitializeComponent();
            colorize();
        }
        public static bool updateAble(Version thisVersion)
        {



        }
        private void colorize()
        {
            var a = new System.Uri(TerminalSettings.Default.colorUri);
            var b = new System.Uri(TerminalSettings.Default.backColorUri);
            var c = new ResourceDictionary();
            var d = new ResourceDictionary();
            c.Source = b;
            d.Source = a;
            this.Resources.MergedDictionaries[4] = c;
            this.Resources.MergedDictionaries[3] = d;
        }
    }
}
