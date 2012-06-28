using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls;
using System.IO.Ports;
using System.IO;

namespace MetroTerminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private BackgroundWorker fileDumpWorker = new BackgroundWorker();
        private BackgroundWorker fileDumpByteArrayWorker = new BackgroundWorker();
        private BackgroundWorker manualSendWorker = new BackgroundWorker();
        private BackgroundWorker receiveWorker = new BackgroundWorker();

        private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        private int manualRepeatCount = 1;

        private BrushConverter bc = new BrushConverter();
        private FontFamilyConverter ffc = new FontFamilyConverter();
        private FontSizeConverter fsc = new FontSizeConverter();

        private SerialPort port = new SerialPort();
        private SerialPort portReceive = new SerialPort();

        private RowDefinition tabRow;

        private String addEOL = "";

        private Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        private LinkedList<String> fileLines = new LinkedList<string>();
        private LinkedList<byte[]> byteArrayLines = new LinkedList<byte[]>();

        public MainWindow()
        {
            InitializeComponent();
            addToList("Application (v" + version.ToString() + ") opened");
            AppWindow.Title += " " +version.ToString(); 
            // THESE LINES MUST BE DELETED

            // DELETE UNTIL HERE
            colorizeAll();
            loadPortsIntoBox(portComboBox);
            loadBaudRates();
            loadSettings();
            prepareThreads();
        }
        void prepareThreads()
        {
            fileDumpWorker.WorkerReportsProgress = true;
            fileDumpWorker.WorkerSupportsCancellation = true;
            fileDumpWorker.DoWork += new DoWorkEventHandler(fileDumpWorker_DoWork);
            fileDumpWorker.ProgressChanged += new ProgressChangedEventHandler(fileDumpWorker_ProgressChanged);
            fileDumpWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(fileDumpWorker_RunWorkerCompleted);

            fileDumpByteArrayWorker.WorkerReportsProgress = true;
            fileDumpByteArrayWorker.WorkerSupportsCancellation = true;
            fileDumpByteArrayWorker.DoWork += new DoWorkEventHandler(fileDumpByteArrayWorker_DoWork);
            fileDumpByteArrayWorker.ProgressChanged += new ProgressChangedEventHandler(fileDumpByteArrayWorker_ProgressChanged);
            fileDumpByteArrayWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(fileDumpByteArrayWorker_RunWorkerCompleted);

            manualSendWorker.WorkerReportsProgress = true;
            manualSendWorker.WorkerSupportsCancellation = true;
            manualSendWorker.DoWork += new DoWorkEventHandler(manualSendWorker_DoWork);
            manualSendWorker.ProgressChanged += new ProgressChangedEventHandler(manualSendWorker_ProgressChanged);
            manualSendWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(manualSendWorker_RunWorkerCompleted);

            receiveWorker.WorkerSupportsCancellation = true;
            receiveWorker.DoWork += new DoWorkEventHandler(receiveWorker_DoWork);
        }
        void fileDumpByteArrayWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            fileDumpWorker_RunWorkerCompleted(sender, e);
        }
        void fileDumpByteArrayWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            dumpFileProgressBar.Value = (int)(((double)e.ProgressPercentage / fileLines.Count) * 100);
        }
        void fileDumpByteArrayWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int progress = 0;
            int delay = 0;
            if (e.Argument != null)
            {
                delay = (int)e.Argument;
            }
            stopwatch.Reset();
            stopwatch.Start();
            foreach (byte[] sendThis in byteArrayLines)
            {
                if (fileDumpByteArrayWorker.CancellationPending == true)
                {
                    e.Cancel = true;
                    return;
                }
                while (true)
                {
                    if (fileDumpByteArrayWorker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }
                    if (stopwatch.ElapsedMilliseconds == delay)
                    {
                        //port.Write(sendThis);
                        port.Write(sendThis, 0, sendThis.Length);
                        addToListSecure("Sent : " + ++progress);
                        fileDumpByteArrayWorker.ReportProgress(progress);
                        stopwatch.Reset();
                        stopwatch.Start();
                        break;
                    }
                }
            }
        }
        private void colorizeAll()
        {
            colorizeTerminalBox();
        }
        private void addToList(String text)
        {
            terminalTextBox.Items.Add(DateTime.Now.ToString("HH:mm:ss:fff", null) + " | " + text);
        }
        private void addToListSecure(String text)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                addToList(text);
            }), System.Windows.Threading.DispatcherPriority.Normal,null);
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
        private void savePortSettings(int baudRate, int baudRateIndex, int stopBitIndex, int parityIndex, int dataBitIndex)
        {
            bool save = false;
            if (stopBitIndex != TerminalSettings.Default.stopBitsIndex)
            {
                TerminalSettings.Default.stopBitsIndex = stopBitIndex;
                save = true;
            }
            if (parityIndex != TerminalSettings.Default.parityIndex)
            {
                save = true;
                TerminalSettings.Default.parityIndex = parityIndex;
            }
            if (dataBitIndex != TerminalSettings.Default.dataBitsIndex)
            {
                save = true;
                TerminalSettings.Default.dataBitsIndex = dataBitIndex;
            }
            if (baudRateIndex != TerminalSettings.Default.lastSelectedBaudRateIndex)
            {
                save = true;
                TerminalSettings.Default.lastSelectedBaudRateIndex = baudRateIndex; 
            }
            if (!TerminalSettings.Default.baudRates.Contains(baudRate.ToString()))
            {
                save = true;
                TerminalSettings.Default.baudRates.Add(baudRate.ToString());
                TerminalSettings.Default.lastSelectedBaudRateIndex = TerminalSettings.Default.baudRates.Count -1; 
            }
            if (save)
            {
                TerminalSettings.Default.Save();
            }
        }
        private void comPortTabEnableDisable()
        {
            portComboBox.IsEnabled = !portComboBox.IsEnabled;
            portLabel.IsEnabled = !portLabel.IsEnabled;
            baudRateLabel.IsEnabled = !baudRateLabel.IsEnabled;
            baudRateComboBox.IsEnabled = !baudRateComboBox.IsEnabled;
            parityGroupBox.IsEnabled = !parityGroupBox.IsEnabled;
            stopBitsGroupBox.IsEnabled = !stopBitsGroupBox.IsEnabled;
            dataBitsGroupBox.IsEnabled = !dataBitsGroupBox.IsEnabled;
            scanPortsButton.IsEnabled = !scanPortsButton.IsEnabled;
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
        private static byte[] StringToByteArray(string hex)
        {
            try
            {
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                return encoding.GetBytes(hex);
            }
            catch (ArgumentOutOfRangeException)
            {
                showErrorMessage("Error in hex conversion!");
            }
            catch (ArgumentNullException)
            {
                showErrorMessage("Error in hex conversion!");

            }
            catch (ArgumentException)
            {
                showErrorMessage("Error in hex conversion!");
            }
            return null;
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
            if (fileDumpWorker.IsBusy || receiveWorker.IsBusy || manualSendWorker.IsBusy)
            {
                showErrorMessage("Post is busy!");
                return;
            }
            if (port.IsOpen)
            {
                port.Close();
                openPortButton.Content = "Open Port";
                comPortTabEnableDisable();
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

                addToList(port.PortName + " is open!");

                openPortButton.Content = "Close Port";

                tabControl1.SelectedIndex = 1;

                savePortSettings(port.BaudRate, baudRateComboBox.SelectedIndex, stopBitsComboBox.SelectedIndex, parityComboBox.SelectedIndex,dataBitsComboBox.SelectedIndex);

                comPortTabEnableDisable();
            }
            catch (ArgumentOutOfRangeException)
            {
                showErrorMessage("Please select valid options for " + portComboBox.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                showMessage(ex.GetType().ToString(), "Error");
            }
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (redirectIfPortClosed())
            {
                return;
            }

            Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();
            openFile.FileName = "";
            openFile.ShowDialog();

            if (openFile.FileName.Length < 2)
            {
                return;
            }

            String fileName = openFile.FileName;
            String file = fileName.Substring(fileName.LastIndexOf("\\") + 1);

            selectFileButton.Content = file;

            addToList(file + " selected");

            fileLines.Clear();
            byteArrayLines.Clear();

            try
            {
                TextReader tr = new StreamReader(fileName);

                String line = "";

                while ((line = tr.ReadLine()) != null)
                {
                    fileLines.AddLast(line);
                    byteArrayLines.AddLast(StringToByteArray(line));
                }

                tr.Close();
            }
            catch (System.IO.IOException)
            {
                showErrorMessage("Couldn't read selected file!");
            }
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

            if (fileDumpWorker.IsBusy == true)
            {
                if (sendAsString.IsChecked == true)
                {
                    fileDumpWorker.CancelAsync();
                }
                else if (sendAsByte.IsChecked == true)
                {
                    fileDumpByteArrayWorker.CancelAsync();
                }
                sendManualGroupBox.IsEnabled = true;
                dumpButton.Content = "Send";
                return;
            }
            else
            {
                if (redirectIfPortClosed())
                {
                    return;
                }
                if (fileLines.Count == 0)
                {
                    Button_Click_4(sender, e);
                }
                int delay = 0;
                if (!(delayTextBox.Text == "Delay" || delayTextBox.Text == "0"))
                {
                    if (!IsItAPositiveNumber(delayTextBox.Text, out delay))
                    {
                        showErrorMessage("Please enter a valid delay value!");
                        return;
                    }
                }
                dumpFileProgressBar.Value = 0;
                if (sendAsString.IsChecked == true)
                {
                    if (eofN.IsChecked == true && eofR.IsChecked == true)
                    {
                        addEOL = "\r\n";
                    }
                    else if (eofN.IsChecked == true && eofR.IsChecked == false)
                    {
                        addEOL = "\n";
                    }
                    else if (eofN.IsChecked == false && eofR.IsChecked == true)
                    {
                        addEOL = "\r";
                    }
                    else
                    {
                        addEOL = "";
                    }
                    fileDumpWorker.RunWorkerAsync(delay);
                }
                else if (sendAsByte.IsChecked == true)
                {
                    if (manualEofN.IsChecked == true && manualEofR.IsChecked == true)
                    {
                        addEOL = "\r\n";
                    }
                    else if (manualEofN.IsChecked == true && manualEofR.IsChecked == false)
                    {
                        addEOL = "\n";
                    }
                    else if (manualEofN.IsChecked == false && manualEofR.IsChecked == true)
                    {
                        addEOL = "\r";
                    }
                    else
                    {
                        addEOL = "";
                    }
                    fileDumpByteArrayWorker.RunWorkerAsync(delay);
                }
                dumpButton.Content = "Cancel";
                sendManualGroupBox.IsEnabled = false;
            }

        }
        private byte[] getCharArray(String data)
        {
            Queue<byte> ll = new Queue<byte>();
            String[] every = data.Split(new char[] { ' ' },StringSplitOptions.RemoveEmptyEntries);
            foreach (String a in every) 
            {
                //int charDecimal = 0;
                try
                {
                    ll.Enqueue(byte.Parse(a));
                    //ll.Push(byte.Parse(a));
                }
                catch (System.OverflowException)
                {
                }
                catch (System.FormatException)
                {
                }
            }
            byte[] array = new byte[ll.Count];
            int llCount = ll.Count;
            for (int i = 0; i < llCount; i++ )
            {
                array[i] = ll.Dequeue();
            }
            //44 55 87 255 256 99 0 25
            return array;
        }
        private void showMessage(String message, String title)
        {
            MessageBox.Show(message, title);
        }
        private static void showErrorMessage(String message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        void fileDumpWorker_DoWork(object s, DoWorkEventArgs args)
        {
            int progress = 1;
            int delay = 0;
            if (args.Argument != null)
            {
                delay = (int)args.Argument;
            }
            stopwatch.Reset();
            stopwatch.Start();
            foreach (String sendThis in fileLines)
            {
                if (fileDumpWorker.CancellationPending == true)
                {
                    args.Cancel = true;
                    return;
                }
                while (true)
                {
                    if (fileDumpWorker.CancellationPending == true)
                    {
                        args.Cancel = true;
                        break;
                    }
                    if (stopwatch.ElapsedMilliseconds >= delay)
                    {
                        port.Write(sendThis + addEOL);
                        addToListSecure("Sent : " + sendThis);
                        fileDumpWorker.ReportProgress(progress++);
                        stopwatch.Reset();
                        stopwatch.Start();
                        break;
                    }
                }
            }
        }
        void manualSendWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            dumpFileProgressBar.Value = (int)(((double)e.ProgressPercentage / manualRepeatCount) * 100);
        }
        void manualSendWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument.GetType().ToString() == "System.String")
            {
                String sendThis = e.Argument.ToString();
                int progress = 1;
                for (int i = 0; i < manualRepeatCount; i++)
                {
                    if (manualSendWorker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        return;
                    }
                    port.Write(sendThis);
                    addToListSecure(sendThis.Trim());
                    manualSendWorker.ReportProgress(progress++);
                }
            }
            else
            {
                byte[] sendThis = (byte[])e.Argument;
                string toSend = "";
                foreach (byte a in sendThis)
                {
                    toSend += a.ToString();
                }
                int progress = 1;
                for (int i = 0; i < manualRepeatCount; i++)
                {
                    if (manualSendWorker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        return;
                    }
                    port.Write(sendThis, 0, sendThis.Length);
                    addToListSecure(toSend);
                    manualSendWorker.ReportProgress(progress++);
                }
            }

        }
        void fileDumpWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            dumpFileProgressBar.Value = (int) (((double)e.ProgressPercentage/fileLines.Count)*100);
        }
        void manualSendWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            manualSendButton.Content = "Send";
            dumpFileGroupBox.IsEnabled = true;
        }
        void fileDumpWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            sendManualGroupBox.IsEnabled = true;
            dumpButton.Content = "Send";
        }
        void receiveWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void updateWindowButton_Click(object sender, RoutedEventArgs e)
        {
            if (tabRow == null)
            {
                tabControl1.Visibility = System.Windows.Visibility.Hidden;
                tabRow = grid1.RowDefinitions[1];
                grid1.RowDefinitions.RemoveAt(1);
                //updateWindowButton.Content.ToString();
                rectangle1.Visibility = System.Windows.Visibility.Collapsed;
                rectangle2.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                tabControl1.Visibility = System.Windows.Visibility.Visible;
                grid1.RowDefinitions.Add(tabRow);
                tabRow = null;
                rectangle1.Visibility = System.Windows.Visibility.Visible;
                rectangle2.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            terminalTextBox.Items.Clear();
            //terminalTextBox.Clear();
        }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();

            dialog.Filter = "Text File|*.txt|Log File|*.log|All Files|*.*";
            dialog.FileName = "";
            dialog.ShowDialog();

            if (dialog.FileName.Length < 2)
            {
                return;
            }

            try
            {
                TextWriter tw = new StreamWriter(dialog.FileName);

                foreach (var item in terminalTextBox.Items)
                {
                    tw.WriteLine(item.ToString());
                }
                //tw.Write(terminalTextBox.Text);
                tw.Close();

                addToList("Log generated");
            }
            catch (System.IO.IOException)
            {
                showErrorMessage("Couldn't write to file");
            }
        }
        private void manualSendASCII_Checked(object sender, RoutedEventArgs e)
        {
            manualEofN.IsEnabled = false;
            manualEofR.IsEnabled = false;
        }
        private void manualSendASCII_Unchecked(object sender, RoutedEventArgs e)
        {
            manualEofN.IsEnabled = true;
            manualEofR.IsEnabled = true;
        }
        private void delayTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (delayTextBox.Text == "Delay")
            {
                delayTextBox.Text = "";
            }
        }
        private void delayTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(delayTextBox.Text) || String.IsNullOrEmpty(delayTextBox.Text))
            {
                delayTextBox.Text = "Delay";
            }
        }
        private void manualDataTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (manualDataTextBox.Text == "Enter Data Here")
            {
                manualDataTextBox.Text = "";
            }
        }
        private void manualDataTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(manualDataTextBox.Text) || String.IsNullOrEmpty(manualDataTextBox.Text))
            {
                manualDataTextBox.Text = "Enter Data Here";
            }
        }
        private void manualRepeat_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(manualRepeat.Text) || String.IsNullOrEmpty(manualRepeat.Text))
            {
                manualRepeatCount = 1;
                manualRepeat.Text = "Repeat";
            }
            else if (!IsItAPositiveNumber(manualRepeat.Text, out manualRepeatCount))
            {
                manualRepeatCount = 1;
                manualRepeat.Text = "Repeat";
            }
        }
        private void manualRepeat_GotFocus(object sender, RoutedEventArgs e)
        {
            if (manualRepeat.Text == "Repeat")
            {
                manualRepeat.Text = "";
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            newPortGroupBox.Visibility = System.Windows.Visibility.Visible;
        }
        private void receiveOtherPort_Unchecked(object sender, RoutedEventArgs e)
        {
            if (portReceive.IsOpen)
            {
                receiveOtherPort.IsChecked = true;
                showMessage("You need to close the port first!", "Data Receive");
                return;
            }
            newPortGroupBox.Visibility = System.Windows.Visibility.Hidden;
        }
        private bool redirectIfPortClosed()
        {
            if (!port.IsOpen)
            {
                tabControl1.SelectedIndex = 0;
                return true;
            }
            else
            {
                return false;
            }
        }
        private void manualSendButton_Click(object sender, RoutedEventArgs e)
        {
            if (redirectIfPortClosed())
            {
                return;
            }
            String sendThis = manualDataTextBox.Text;
            if (sendThis.Length < 1 || sendThis == null || sendThis == "Enter Data Here")
            {
                return;
            }

            if (manualSendASCII.IsChecked == true)
            {
                byte[] asciiChars = getCharArray(manualDataTextBox.Text);
                if (asciiChars.Length < 1)
                {
                    return;
                }
                if (!manualSendWorker.IsBusy)
                {
                    dumpFileProgressBar.Value = 0;
                    manualSendWorker.RunWorkerAsync(asciiChars);
                    manualSendButton.Content = "Cancel";
                    dumpFileGroupBox.IsEnabled = false;

                }
                else
                {
                    manualSendWorker.CancelAsync();
                    manualSendButton.Content = "Send";
                    dumpFileGroupBox.IsEnabled = true;
                }
            }
            else
            {

                if (manualEofR.IsChecked == true)
                {
                    sendThis += "\r";
                }

                if (manualEofN.IsChecked == true)
                {
                    sendThis += "\n";
                }

                if (!manualSendWorker.IsBusy)
                {
                    dumpFileProgressBar.Value = 0;
                    manualSendWorker.RunWorkerAsync(sendThis);
                    manualSendButton.Content = "Cancel";
                    dumpFileGroupBox.IsEnabled = false;

                }
                else
                {
                    manualSendWorker.CancelAsync();
                    manualSendButton.Content = "Send";
                    dumpFileGroupBox.IsEnabled = true;
                }
            }
        }
    }
}
