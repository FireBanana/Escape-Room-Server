   M S B A M L   `   `   `  ����W  SUnity-Escape-Room-Server-WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:Unity_Escape_Room_Server_WPF.Windows$Unity_Escape_Room_Server_WPF.Windows  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	   5    /Unity_Escape_Room_Server_WPF.Windows.TeamWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Dlocal2clr-namespace:Unity_Escape_Room_Server_WPF.Windows     5   	   9System&clr-namespace:System;assembly=mscorlib     5   	   -   5          � Title$  
TeamWindow��5	   	   $	��490��6   $	��800��6)   .��5
      � .+�5   
   �� -   5         ��Name$ TeamNameText��5      $��Team Name: Default��6&   $��Center=�6C   $��0,10,0,0q�6`   $	��Top=�6r   $��22(�6�   $
��Bold"�6�   $	��600��6�   $��Center=�6�   �� 5   
   $��Time Remaining:��6   $
��Left=�6*   $��	55,54,0,0q�6E   $	��Top=�6X   $	��156��6p   $��18(�6|   $
��Bold"�6�   �� 5   
   -   5        $ TimeRemainingText��5      $��00:00��6+   $
��Left=�6;   $��	55,68,0,0q�6V   $	��Top=�6i    J� RenderTransformOrigin$ 0.349,-0.172&�6�   $��55��6�   $	��145��6�   $��36(�6�   �� 5   
   $��Score:��6   $
��Left=�6!   $��
257,54,0,0q�6<   $	��Top=�6P   $��70��6h   $��18(�6s   $
��Bold"�6�   �� 5   
   -   5        $ 	ScoreText��5      $��0��6#   $
��Left=�6/   $��
253,68,0,0q�6J   $	��Top=�6^   $ 0.349,-0.172&�6v   $��55��6�   $	��145��6�   $��36(�6�   �� 5   
   $��Final Choice:��6   $
��Left=�6(   $��
553,54,0,0q�6C   $	��Top=�6W   $	��119��6o   $��18(�6{   $
��Bold"�6�   �� 5   
   $��Final Score:��6   $
��Left=�6'   $��
553,89,0,0q�6B   $	��Top=�6V   $	��119��6n   $��18(�6z   $
��Bold"�6�   �� 5   
   $��Final Time:��6   $
��Left=�6&   $��553,123,0,0q�6A   $	��Top=�6V   $	��119��6n   $��18(�6z   $
��Bold"�6�   �� 5   
   -   5        $ FinalChoiceText��5      $��0��6)   $
��Left=�65   $��
672,55,0,0q�6P   $	��Top=�6d   $��57��6|   $��18(�6�   �� 5   
   -   5        $ FinalScoreText��5      $��0��6(   $
��Left=�64   $��
672,88,0,0q�6O   $	��Top=�6c   $��57��6{   $��18(�6�   �� 5   
   -   5        $ FinalTimeText��5      $��0��6'   $
��Left=�63   $��672,123,0,0q�6N   $	��Top=�6c   $��57��6{   $��18(�6�   �� 5   
   -   5         ��Name$ PauseBtn��5      $��Pause��6#   $
��Left=�63   $��
55,131,0,0q�6N   $	��Top=�6b   $	��156��6z   $��42��6�   6�   �� 5   
   -	   5         ��Name$ HintListBox��5      $
��Left=�6'   $	��141��6B   $��
55,242,0,0q�6O   $	��Top=�6c   $	��317��6{   .@�5      ��This is a sample Hint��63   ��5      This is another hint��62   ��5      This is yet another hint��66   ��5      This is yet another hint��66   ��5      This is yet another hint��66   ��5      This is yet another hint��66   ��5      This is yet another hint��66   ��5      This is yet another hint��66   ��5       This is yet another hint��66   5!      �� 5"   
   -
   5        $ SendHintBtn��5"      $
��Send��6&   $
��Left=�65   $��216,404,0,0q�6P   $	��Top=�6e   $	��156��6}   $��42��6�   6�   �� 5#   
   $��Pre-made hints:��6   $
��Left=�6*   $��
55,213,0,0q�6E   $	��Top=�6Y   $	��156��6q   $��14(�6}   $
��Bold"�6�   $��20��6�   $��0q�6�   �� 5$   
   -   5        $ HintbotReadyBtn��5$      $��Send Hintbot Ready��6*   $
��Left=�6G   $��519,162,0,0q�6b   $	��Top=�6w   $	��201��6�   $��42��6�   6�   �� 5%   
   -   5        $ SendTypedHintBtn��5%      $
��Send��6+   $
��Left=�6:   $��564,404,0,0q�6U   $	��Top=�6j   $	��156��6�   $��42��6�   6�   �� 5&   
   $��Custom hints:��6   $
��Left=�6(   $��403,218,0,0q�6C   $	��Top=�6X   $	��156��6p   $��14(�6|   $
��Bold"�6�   $��20��6�   $��0q�6�   �� 5'   
   -   5         ��Name$ HintTextBox��5'      $
��Left=�6'   $	��141��6B   $��403,242,0,0q�6O    �� TextWrapping$
 Wrap=�6d   $��Enter Hint...��6x   $	��Top=�6�   $	��326��6�   �� 5(   
   -   5        $ PauseStatusBtn��5(      $��Game is unpaused��6(   $
��Left=�6C   $��
55,178,0,0q�6^   $	��Top=�6r   $	��156��6�   $��14(�6�   $��20��6�   $��0q�6�   5*      5+      