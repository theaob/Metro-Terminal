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

        private SerialPort port = new SerialPort();

        private Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        public MainWindow()
        {
            InitializeComponent();
            // THESE LINES MUST BE DELETED
            for (int i = 0; i < 100; i++)
            {
                addToList(version.ToString());
            }
            updateAvailable();
            //
            colorizeAll();
            loadPortsIntoBox(portComboBox);
            loadBaudRates();
            loadSettings();
        }
        private void colorizeAll()
        {
            colorizeTerminalBox();
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
        private void loadSettings()
        {
            dataBitsComboBox.SelectedIndex = TerminalSettings.Default.dataBitsIndex;
            parityComboBox.SelectedIndex = TerminalSettings.Default.parityIndex;
            stopBitsComboBox.SelectedIndex = TerminalSettings.Default.stopBitsIndex;
        }
        private bool IsItAPositiveNumber(String numberString, out int number)
        {
            try
            {
                number = int.Parse(numberString, null);
                if (number <= 0)
                {
                    throw new FormatException();
                }
            }
            catch (FormatException)
            {
                number = 0;
                return false;
            }
            catch (OverflowException)
            {
                number = 0;
                return false;
            }
            catch (ArgumentNullException)
            {
                number = 0;
                return false;
            }
            return true;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.ShowDialog();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            loadPortsIntoBox(portComboBox);
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (port.IsOpen)
            {
                port.Close();
                return;
            }
            try
            {
                int baudRate = 9600;
                if (IsItAPositiveNumber(baudRateComboBox.Text, out baudRate))
                {
                    port.BaudRate = baudRate;
                }
                else
                {
                    showMessage("Please enter a valid baud rate", "Error");
                }

                switch (parityComboBox.SelectedItem.ToString())
                {
                    case "None":
                        {
                            port.Parity = Parity.None;
                            break;
                        }
                    case "Odd":
                        {
                            port.Parity = Parity.Odd;
                            break;
                        }
                    case "Even":
                        {
                            port.Parity = Parity.Even;
                            break;
                        }
                    case "Mark":
                        {
                            port.Parity = Parity.Mark;
                            break;
                        }
                    case "Space":
                        {
                            port.Parity = Parity.Space;
                            break;
                        }
                }

                switch (dataBitsComboBox.SelectedIndex)
                {
                    case 0:
                        {
                            port.DataBits = 8;
                            break;
                        }
                    case 1:
                        {
                            port.DataBits = 7;
                            break;
                        }
                    case 2:
                        {
                            port.DataBits = 6;
                            break;
                        }
                    case 3:
                        {
                            port.DataBits = 5;
                            break;
                        }
                }

                switch (stopBitsComboBox.SelectedIndex)
                {
                    case 0:
                        {
                            port.StopBits = StopBits.None;
                            break;
                        }
                    case 1: { port.StopBits = StopBits.One; break; }
                    case 2: { port.StopBits = StopBits.OnePointFive; break; }
                    case 3: { port.StopBits = StopBits.Two; break; }
                }

                port.PortName = portComboBox.SelectedItem.ToString();

                port.Open();
            }
            catch (ArgumentOutOfRangeException)
            {
                showErrorMessage("Please select valid options for " + port.PortName);
            }
            catch (Exception ex)
            {
                showMessage(ex.GetType().ToString(), "Error");
            }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            terminalTextBox.Text = "";
        }
        private void showMessage(String message, String title)
        {
            MessageBox.Show(message, title);
        }
        private void showErrorMessage(String message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }



    }
}
