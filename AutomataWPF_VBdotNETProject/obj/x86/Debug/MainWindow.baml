   M S B A M L   `   `   `  ����  AutomataWPF_VBdotNETProject,    &AutomataWPF_VBdotNETProject.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      o kTelerik.Windows.Controls.RibbonBar, Version=2011.1.419.35, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2011.1.419.35, Culture=neutral, PublicKeyToken=5803cfa389c90ce7m iTelerik.Windows.Controls.Docking, Version=2011.1.419.35, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2011.1.419.35, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n	 jTelerik.Windows.Controls.GridView, Version=2011.1.419.35, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p
 lTelerik.Windows.Controls.Navigation, Version=2011.1.419.35, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2011.1.419.35, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Ktelerik1http://schemas.telerik.com/2008/xaml/presentation     	 
  5   Q     � Title$  
MainWindow��6   $	��667��6   $
��1010��6%   $	��Top=�62   .��5   
   � -   5         �Name$ mainGrid��5      $	��633��6!   $	��999��6.    � RowDefinitions 5   
   �� 5      $
��117* �6   �� 5      $
��516* �6    � ColumnDefinitions 5
   
   �� 5      $
��103* �6   �� 5      $
��400* �6   .+�5   
   +  %Telerik.Windows.Controls.RadRibbonBar  -   5        $��RadRibbonBar1��5   n   $��2��6   $	��171��63   $
��Left=�6@   $��	1,-28,0,0q�6[   $	��Top=�6�   $	��986��6�   $��2��6�   .@�5      +  %Telerik.Windows.Controls.RadRibbonTab  $��Automatan Type��6#   .@�5      -  'Telerik.Windows.Controls.RadRibbonGroup  $	��150��6)   .@�5      (  "Telerik.Windows.Controls.RadButton  -   5        $��RadButtonDFA��5   g   $��DFA Automaton��6(   $��60��6@   $
��Left=�6L   $	��140��6{   5        5      $	��150��6)   .@�5        -   5        $��RadButtonNFA��5   g   $��NFA Automaton��6(   $��60��6@   $
��Left=�6L   $	��140��6{   5        5      $	��150��6)   .@�5        -   5        $��RadButtoneNFA��5   h   $��eNFA Automaton��6(   $��60��6A   $
��Left=�6M   $	��140��6}   5        5      $	��150��6)   .@�5        -   5        $!��RadButtoneRegularExpression��5   l   $��Regular Expression��6(   $��60��6E   $
��Left=�6Q   $	��140��6�   5      5        5      $��	Open File��6#   .@�5        $	��150��6)   .@�5        -   5        $��RadButtoneOpenFile��5   c   $��	Open File��6(   $��60��6<   $
��Left=�6H   $	��140��6}   5       5!      5"      �� 5#   
   $��Visible=�6   $��Visible=�6>   $��1��6f   $��1��6w   $��0,0,12,7q�6�   .��5$   
   �� -   5         ��Name$ 
mainCanvas��5$      $
��5000��6%   $
��1000��62   $��1��6�   $��1��6   � ToolTip 5%      �� 5&      -	   5        $��lblCoordonate��5&      ��5(      �� 5)       �� EndPoint$ 0.5,1&�6&    �� 
StartPoint$ 0.5,0&�67   .��5*      �  � Color$ 	#D8380000��6#   	 � Offset$	 0Q�65   � 5+      $ White��6#   $	 1Q�61   5,      5.      5/      , 
 &Telerik.Windows.Controls.PanelBarPanel  50   
   -
   5        $��PanelBarPanel1��50   f   $��1��6    $	��509��6-   $
��Left=�6:   $��5,0,0,0q�6U   $	��Top=�6|   $	��194��6�   ��51      �� 52      $ 1,0.5&�6&   $ 0,0.5&�67   .��53      � $ 	#FF6F6161��6#   $	 0Q�65   � 54      $ 	#FFEBEBEB��6#   $	 1Q�65   55      .+�57      �� .+�58      �� $��Control Panel��6   $��Center=�60   $��16(�6M   $
��Bold"�6[   $��Eras ITC)�6m   6�   *  $Telerik.Windows.Controls.RadExpander  5:      $��Draw Automaton��6&   $��10,10q�6>   
���������5;      �� 5<      .+�5=        -   5        
  Name$
 RadButtonDrawAutomata��5=   0   $��DrawAutomaton��6�   $��27��6�   $��Center=�6�   $	��120��6�   5>      5@        5B      $��Check a chain��6&   $��10,10q�6=   
���������5C      �� 5D      .+�5E      �� $!��Enter a Chain to manipulate��6$   �� 5F      6*   �� 5G      -   5         ��Name$ TextBoxChainInput��5G   &   �� 5H      6*     5I      -   5        $
 RadButtonCheckChain��5I   0   $��Check Chain��6o   $��27��6�   $��Center=�6�   $	��120��6�   �� 5J      6*     5K      -   5        $
 RadButtonIllustrate��5K   0   $��Illustrate Path��6p   $��27��6�   $��Center=�6�   $	��120��6�   5M      5O        5Q      $��Manipulate Automaton��6&   $��10,10q�6D   
���������5R      �� 5S      .+�5T      �� 6*     5U      -   5        $
 RadButtonConvertToeNFA��5U   0   $��Convert to eNFA Automata��6u   $��25��6�   $��Center=�6�   $	��160��6�   �� 5V      6*     5W      -   5        $
 RadButtonConvertToNFA��5W   0   $��Convert to NFA Automata��6t   $��25��6�   $��Center=�6�   $	��160��6�   �� 5X      6*     5Y      -   5        $
 RadButtonConvertToDFA��5Y   0   $��Convert to DFA Automata��6t   $��25��6�   $��Center=�6�   $	��160��6�   �� 5Z      6*     5[      -   5        $
 RadButtonConvertToMinDFA��5[   0   $��Convert to Minimize DFA��6z   $��25��6�   $��Center=�6�   $	��160��6�   �� 5\      6*     5]      -   5        $
 RadButtonConvertToRE��5]   0   $��Convert to RE��6r   $��25��6�   $��Center=�6�   $	��160��6�   5^      5`        5b      $��Regular Expression Info��6&   $��10,10q�6G   
���������5c      �� 5d      .+�5e      �� $,��&Current Manipulated Regular Expression��6$   �� 5f      6*   �� 5g      -   5        $ TextBoxCurrentRE��5g   &   5h      5j      5l      5m      5n      5o      