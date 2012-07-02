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
using Xceed.Wpf.Toolkit;

namespace MetroTerminal
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : MetroWindow
    {

        String backColorUri = TerminalSettings.Default.colorUri;
        String baseColorUri = TerminalSettings.Default.backColorUri;

        private BrushConverter bc = new BrushConverter();
        private ColorConverter cc = new ColorConverter();
        private FontFamilyConverter ffc = new FontFamilyConverter();

        public SettingsWindow()
        {
            InitializeComponent();
            loadColorFromSettings();
            buttonizer();
            buttonizerBack();
            terminalColorize();
        }
        private void terminalColorize()
        {
            listBoxTest.Background = (Brush)bc.ConvertFrom(TerminalSettings.Default.terminalBackColor);
            listBoxTest.Foreground = (Brush)bc.ConvertFrom(TerminalSettings.Default.terminalFontColor);
            //terminalForeButton.Background = listBoxTest.Foreground;
            //terminalBackPicker.Background = listBoxTest.Background;
            terminalBackPicker.SelectedColor = (Color) cc.ConvertFrom(TerminalSettings.Default.terminalBackColor);
            terminalForePicker.SelectedColor = (Color) cc.ConvertFrom(TerminalSettings.Default.terminalFontColor);
            //terminalBackPicker.Foreground = listBoxTest.Background;
            //terminalForePicker.Background = listBoxTest.Foreground;
            //terminalForePicker.Foreground = listBoxTest.Foreground;
        }
        private void loadColorFromSettings()
        {
            String color = TerminalSettings.Default.colorUri;
            color = color.Substring(color.LastIndexOf('/') + 1);
            color = color.Substring(0, color.IndexOf('.'));
            changeColor(color);

            color = TerminalSettings.Default.backColorUri;
            color = color.Substring(color.LastIndexOf('/') + 1);
            color = color.Substring(0, color.IndexOf('.'));
            color = color.Substring(4);
            changeBackColor(color);

        }
        private void buttonizer()
        {
            String color = TerminalSettings.Default.colorUri;
            color = color.Substring(color.LastIndexOf('/') + 1);
            color = color.Substring(0, color.IndexOf('.'));
            blueRect.Content = null;
            redRect.Content = null;
            orangeRect.Content = null;
            greenRect.Content = null;
            purpleRect.Content = null;
            if (color == "Blue")
            {
                blueRect.Content = "X";
            }
            else if (color == "Red")
            {
                redRect.Content = "X";
            }
            else if (color == "Green")
            {
                greenRect.Content = "X";
            }
            else if (color == "Orange")
            {
                orangeRect.Content = "X";
            }
            else if (color == "Purple")
            {
                purpleRect.Content = "X";
            }
        }
        private void buttonizerBack()
        {
            String color = TerminalSettings.Default.backColorUri;
            color = color.Substring(color.LastIndexOf('/') + 1);
            color = color.Substring(0, color.IndexOf('.'));
            darkRect.Content = null;
            darkRect.Foreground = Brushes.White;
            lightRect.Content = null;
            lightRect.Foreground = Brushes.Black;
            if (color == "BaseDark")
            {
                darkRect.Content = "X";
            }
            else if (color == "BaseLight")
            {
                lightRect.Content = "X";
            }
        }
        private void changeColor(String color)
        {
            backColorUri = "pack://application:,,,/MahApps.Metro;component/Styles/Accents/" + color + ".xaml";
            var a = new System.Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/"+color+".xaml");
            var d = new ResourceDictionary();
            d.Source = a;
            this.Resources.MergedDictionaries[3] = d;
            buttonizer();
        }
        private void changeBackColor(String shade)
        {
            baseColorUri = "pack://application:,,,/MahApps.Metro;component/Styles/Accents/Base" + shade + ".xaml";
            var a = new System.Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Base" + shade + ".xaml");
            var d = new ResourceDictionary();
            d.Source = a;
            this.Resources.MergedDictionaries[4] = d;
            buttonizerBack();
        }

        private void blueRect_Click(object sender, RoutedEventArgs e)
        {
            changeColor("Blue");
        }

        private void redRect_Click(object sender, RoutedEventArgs e)
        {
            changeColor("Red");
        }

        private void greenRect_Click(object sender, RoutedEventArgs e)
        {
            changeColor("Green");
        }

        private void purpleRect_Click(object sender, RoutedEventArgs e)
        {
            changeColor("Purple");
        }

        private void orangeRect_Click(object sender, RoutedEventArgs e)
        {
            changeColor("Orange");
        }

        private void lightRect_Click(object sender, RoutedEventArgs e)
        {
            changeBackColor("Light");
        }

        private void darkRect_Click(object sender, RoutedEventArgs e)
        {
            changeBackColor("Dark");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String color = this.Resources.MergedDictionaries[3].Source.ToString();
            color = color.Substring(color.LastIndexOf('/') + 1);
            color = color.Substring(0, color.IndexOf('.'));
            Brush code = Brushes.White;
            if(color == "Blue")
            {
                code = blueRect.Background;
            }
            else if(color == "Orange")
            {
                code = orangeRect.Background;
            }
            else if(color == "Red")
            {
                code = redRect.Background;
            }
            else if(color == "Green")
            {
                code = greenRect.Background;
            }
            else if(color == "Purple")
            {
                code = purpleRect.Background;
            }
            listBoxTest.Foreground = code;
            listBoxTest.Background = Brushes.Black;
            terminalBackPicker.SelectedColor = Brushes.Black.Color;
            terminalForePicker.SelectedColor = (Color)cc.ConvertFrom(code.ToString());
        }

        private void terminalForePicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            var mediaColor = cc.ConvertFrom(terminalForePicker.SelectedColor.ToString());
            listBoxTest.Foreground = (System.Windows.Media.Brush)bc.ConvertFromString(mediaColor.ToString());
        }

        private void terminalBackPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            var mediaColor = cc.ConvertFrom(terminalBackPicker.SelectedColor.ToString());
            listBoxTest.Background = (System.Windows.Media.Brush)bc.ConvertFromString(mediaColor.ToString());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult a = System.Windows.MessageBox.Show("Your settings will be overwritten!", "Are you sure?", MessageBoxButton.YesNoCancel);
            switch (a)
            {
                case MessageBoxResult.Yes:
                    {
                        TerminalSettings.Default.backColorUri = baseColorUri;
                        TerminalSettings.Default.colorUri = backColorUri;
                        TerminalSettings.Default.terminalBackColor = listBoxTest.Background.ToString();
                        TerminalSettings.Default.terminalFontColor = listBoxTest.Foreground.ToString();


                        TerminalSettings.Default.Save();

                        this.Close();
                        return;
                    }
                case MessageBoxResult.No:
                    {
                        this.Close();
                        return;
                    }
                case MessageBoxResult.Cancel:
                    {
                        return;
                    }
            }
            
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            fontFamilySelector.ItemsSource = Fonts.SystemFontFamilies;

           /* int index = 0;
            int count = 0;
            for (int i = 0; i <= Fonts.SystemFontFamilies.Count; i++)
            {
                if(String.Equals(fontFamilySelector.Items[i].ToString(), TerminalSettings.Default.terminalFontFamily, StringComparison.InvariantCultureIgnoreCase))
                {
                    index = i;
                }
                String x = fontFamilySelector.Items[i].ToString();
                count = i;
            }*/
            //Fonts.SystemFontFamilies[
            FontFamily settingFF = (FontFamily)ffc.ConvertFrom(TerminalSettings.Default.terminalFontFamily);
            //var s = Fonts.SystemFontFamilies.Ge

            //int index = fontFamilySelector.Items.IndexOf(TerminalSettings.Default.terminalFontFamily);

            //fontFamilySelector.Text = TerminalSettings.Default.terminalFontFamily;
            fontFamilySelector.SelectedIndex = fontFamilySelector.Items.IndexOf(settingFF);
                //TerminalSettings.Default.terminalFontFamily);
            //fontFamilySelector.SelectedValue = TerminalSettings.Default.terminalFontFamily;
            //fontFamilySelector.SelectedIndex = fontFamilySelector.Items.IndexOf(FontFamily.G
            for(int i = 6; i <= 72; i++)
            {
                if((i > 12 && i % 2 == 0 && i < 30) || i <=12 || (i % 12 == 0))
                fontSizeSelector.Items.Add(i);
            }
            fontSizeSelector.Text = TerminalSettings.Default.terminalFontSize.ToString();
                //TerminalSettings.Default.terminalFontFamily);

            instantScrollCheckBox.IsChecked = TerminalSettings.Default.instantScroll;
            opacitySlider.Value = TerminalSettings.Default.opacity;
            alwaysOnTopCheckBox.IsChecked = TerminalSettings.Default.alwaysOnTop;

            this.Opacity = opacitySlider.Value;

            showIconIfUpdate.IsChecked = TerminalSettings.Default.displayUpdateIcon;
            autoCheckForUpdates.IsChecked = TerminalSettings.Default.autoUpdate;
            autoInstallUpdate.IsChecked = TerminalSettings.Default.autoInstallNewUpdates;

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBoxResult r = System.Windows.MessageBox.Show("Are you sure?", "Save Settings", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            switch (r)
            {
                case MessageBoxResult.Yes:
                    {
                        try
                        {
                            TerminalSettings.Default.terminalFontFamily = fontFamilySelector.SelectedValue.ToString();
                            TerminalSettings.Default.terminalFontSize = double.Parse(fontSizeSelector.SelectedValue.ToString());
                            //TerminalSettings.Default.autoUpdate
                            TerminalSettings.Default.opacity = opacitySlider.Value;
                            TerminalSettings.Default.instantScroll = (instantScrollCheckBox.IsChecked == true);
                            TerminalSettings.Default.alwaysOnTop = (alwaysOnTopCheckBox.IsChecked == true);

                            TerminalSettings.Default.Save();
                        }
                        catch (FormatException)
                        {
                            System.Windows.MessageBox.Show("Please enter a valid font size!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        catch (OverflowException)
                        {
                            System.Windows.MessageBox.Show("Please enter a valid font size!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        this.Close();
                        return;
                    }
                case MessageBoxResult.No:
                    {
                        this.Close();
                        return;
                    }
                case MessageBoxResult.Cancel:
                    {
                        return;
                    }
            }




        }

        private void opacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Opacity = opacitySlider.Value;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://onurbaykal.com/Metro-Terminal/");
        }

        private void saveUpdateSettingButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult r = System.Windows.MessageBox.Show("Are you sure?", "Save Settings", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            switch (r)
            {
                case MessageBoxResult.Yes:
                    {
                        TerminalSettings.Default.displayUpdateIcon = (showIconIfUpdate.IsChecked == true);
                        TerminalSettings.Default.autoUpdate = (autoCheckForUpdates.IsChecked == true);
                        TerminalSettings.Default.autoInstallNewUpdates = (autoInstallUpdate.IsChecked == true);

                        TerminalSettings.Default.Save();
                        this.Close();
                        return;
                    }
                case MessageBoxResult.No:
                    {
                        this.Close();
                        return;
                    }
                case MessageBoxResult.Cancel:
                    {
                        return;
                    }
            }   
        }
    }
}
