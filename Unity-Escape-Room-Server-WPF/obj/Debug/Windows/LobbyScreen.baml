   M S B A M L   `   `   `  ����W  SUnity-Escape-Room-Server-WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Unity_Escape_Room_Server_WPFUnity_Escape_Room_Server_WPF  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	   .    (Unity_Escape_Room_Server_WPF.LobbyScreen   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   <local*clr-namespace:Unity_Escape_Room_Server_WPF     5   	   9System&clr-namespace:System;assembly=mscorlib     5   	   -   5          �Name$  LobbyWindow��5   ?    � Title$ LobbyScreen��5	   	   $	��450��6   $	��800��6*    � WindowStyle$
 None=�66    � WindowState$ 	Maximized=�6�    � 
ResizeMode$ NoResize=�6�   $��Stretch=�6�   $��Stretch=�6�   .��5
      � .+�5   
   �� $��Weirder Stuff��6   $��188,12,186,361.6q�6(   $��48(�6B   $��Center=�6P   $
��Bold"�6t    J� RenderTransformOrigin$ 0.439,0.539&�6�   $��Stretch=�6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ TeamDataGrid��5      $��38,89,32,30.6q�6)      GridLinesVisibility$ 
Horizontal=�6@   $��0q�6a   ���	#00F0F0F0$��Stretch=�6�   $��Stretch=�6�   $��0q�6�      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$	 
Team Name:��6B   
   FontSize$
 24Q�6V      
Foreground
 �   �   Width5  /System.Windows.Controls.DataGridLengthConverter$	 450 6w   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   �� *
TeamName+6w     5      $	 Time:��6>   $
 24Q�6M   $	 150 6[    6%   �� *Time+6[     5      $	 Score:��6?   $
 24Q�6O   $ * 6]    6%   �� *Score+6]    � 	Resources 5      =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� 
TargetType �� Property �� Value��%~   '        �� 5      "  6   .�5      ��  � ��$ 20(�61   �� 5       � �� �	#FFFF060663   5      5      5      54      55      