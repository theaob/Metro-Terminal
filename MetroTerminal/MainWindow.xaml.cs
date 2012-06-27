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
using MahApps.Metro.Controls;

namespace MetroTerminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        private Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 100; i++)
            {
                addToList(version.ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.ShowDialog();
            //MessageBox.Show("Settings!");*/
        }

        private void updateAvailable()
        {
            updateWindowButton.Visibility = System.Windows.Visibility.Visible;
        }

        private void addToList(String text)
        {
            //textBox1.Text += DateTime.Now.ToString("HH:mm:ss:fff", null) + " | " + text + "\r\n";
            //textBlock1.Inlines.Add(DateTime.Now.ToString("HH:mm:ss:fff", null) + " | " + text + "\r\n");
            //listBox1.Items.Insert(listBox1.Items.Count, DateTime.Now.ToString("HH:mm:ss:fff", null) + " | " + text);
        }
    }
}
