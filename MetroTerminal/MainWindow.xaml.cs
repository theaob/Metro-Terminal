using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls;
using System.IO.Ports;

namespace MetroTerminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private BrushConverter bc = new BrushConverter();
        private FontFamilyConverter ffc = new FontFamilyConverter();
        private FontSizeConverter fsc = new FontSizeConverter();

        private Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 100; i++)
            {
                addToList(version.ToString());
            }
            colorizeAll();
            loadPortsIntoBox(portComboBox);
            loadBaudRates();
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

            terminalTextBox.Text += DateTime.Now.ToString("HH:mm:ss:fff", null) + " | " + text + "\r\n";
            //textBlock1.Inlines.Add(DateTime.Now.ToString("HH:mm:ss:fff", null) + " | " + text + "\r\n");
            //listBox1.Items.Insert(listBox1.Items.Count, DateTime.Now.ToString("HH:mm:ss:fff", null) + " | " + text);
        }
        private void colorizeAll()
        {
            colorizeTerminalBox();
        }
        private void colorizeTerminalBox()
        {
            terminalTextBox.Background = (Brush)bc.ConvertFrom(TerminalSettings.Default.terminalBackColor);
            terminalTextBox.Foreground = (Brush)bc.ConvertFrom(TerminalSettings.Default.terminalFontColor);
            terminalTextBox.FontFamily = (FontFamily) ffc.ConvertFrom(TerminalSettings.Default.terminalFontFamily);
            terminalTextBox.FontSize = TerminalSettings.Default.terminalFontSize;
        }



        private void loadPortsIntoBox(ComboBox box)
        {
            SerialPort sp = new SerialPort();

            box.Items.Clear();
            foreach (String portName in SerialPort.GetPortNames())
            {
                try
                {
                    sp.PortName = portName;
                    sp.Open();
                    box.Items.Add(portName);
                    sp.Close();
                }
                catch (UnauthorizedAccessException)
                {
                }
            }
            box.SelectedIndex = 0;
        }
        private void loadBaudRates()
        {
            foreach (String rate in TerminalSettings.Default.baudRates)
            {
                baudRateComboBox.Items.Add(rate);
            }
            baudRateComboBox.SelectedIndex = TerminalSettings.Default.lastSelectedBaudRateIndex;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            loadPortsIntoBox(portComboBox);
        }
    }
}
