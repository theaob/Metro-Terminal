   M S B A M L   `   `   `  ����T  PMahApps.Metro, Version=0.7.1.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	    MetroTerminal   MetroTerminal.Updater   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	     � Title$  Updater��5   	   $	��300��6   $	��300��6&    � 
ResizeMode$ NoResize=�62    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5	      $H Bpack://application:,,,/MahApps.Metro;component/Styles/Colours.xaml?�6%   �� 5
      $F @pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml?�6%   �� 5      $I Cpack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml?�6%   �� 5      $M Gpack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml?�6%   �� 5      $R Lpack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml?�6%   %    5      .��5      �  � RowDefinitions 5   
   �� 5      $	��40* �6   �� 5      $	��47* �6   �� 5      $
��174* �6    � ColumnDefinitions 5   
   �� 5      �� 5      .+�5   
   �� $��0��6   $��0��6!   $��23��6.   $��	10,0,10,0q�6:   .��6N   Check For Update6`   �� 5   
   $��1��6   $��0��6!   $��23��6.   $��	10,0,10,0q�6:   .��6M   Install Update6]   �� 5   
   $��0��6   $��1��6$   $
��Wrap=�61   $��10,0,0,0q�6E   $��Your Version: ��6W   6o   �� 5   
   $��1��6   $��1��6$   $
��Wrap=�61   $��10,0,0,0q�6E   $��Online Version: ��6W   6q   �� 5   
   -   5        $��changeLogTextBox��5   a   $��0��6   $��2��6"   $��2��66   $��10,-10,10,10q�6C   $�� ��6Y   6{   5       5!      