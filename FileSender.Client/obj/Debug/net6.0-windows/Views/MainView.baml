   M S B A M L   `   `   `  ����  FileSender.Client&     FileSender.Client.Views.MainView   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=4.6.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   -   5        $��Window��5   	     � 
ResizeMode$  NoResize=�5   	    � WindowStyle$
 None=�6    � AllowsTransparency$
 True��62   
������  � Title$ Client��5      $	��600��6&   $	��900��63    � 	Resources 5
         AppColor �� Color��%+   &	        �� 5   
   $ #003171��6+   5      .��5      �� ���#d7dbe2 �� CornerRadius$ 7��6"   ._�5   
   �  � RowDefinitions 5      �� 5      $��50��6    �� 5      .+�5      �� -   5        #��[  5      $ 
7, 7, 0, 0��6<   ._�5      ]� $��False��6    5       �� 
TargetType . *MaterialDesignFloatingActionMiniDarkButton	 �� BasedOn
 �� Property �� Value��%  '��       7  �� 5      " ��6    8	   .�5      �� 
 � ��
 �����6;   �� 5      
 � ��
 ���� 6;   �� 5      
 � ��
 ���� 6<   �� 5       
 � ��$ Calibri)�6;   �� 5"      
 � ��$ 40��67   �� 5#      
 � ��$ 40��66   5%      5&      .+�5(      �� -   5        $��
0, 0, 5, 0q�5(   L   $��Right=�6   ��6   0  *MaterialDesignThemes.Wpf.PackIconExtension     Kind$ Close=�6   �� 5)      -   5        $��Right=�5)   z   ��6     $ WindowMinimize=�6z   �� 5+      $��Center=�6}   $��15, 0, 15, 0q�6�   $
��Left=�6�   ��6     $ AccessPointNetwork=�   Size 30��6`   �� *Color+6�   �� 5,      $��Center=�6P   
�������$��Comic Sans MS)�6�   $��20(�6�   $
��Left=�6�   ��6    ��  �� ElementName$ Window�� �� Path$ Title�6�   5.      5/      A� 51       A� Columns$ 2��6   $��1��6&    53      +  %MaterialDesignThemes.Wpf.ShadowAssist $  MaterialDesignFlatAccentBgButton+  %MaterialDesignThemes.Wpf.ButtonAssist��%  '��       '�� /    7  7   7   �� 55      " ��6   .�57      �� 
 � ��
 �����67   �� 58      
 �@��CornerRadius$ 7��69   �� 5:      
 �@��Padding$ 10q�64   �� 5;      
 � ��$ 20q�63   �� 5=      
 �@ ShadowDepth$ Depth5=�6T   �� 5>      
 �@ ShadowEdges$	 All=�6T   5@      �� 5B      " ��6   8	   .�5D      �� 
 � ��8  67   �� 5E      
 � ��8  68   �� 5F      
 � ��
 �����67   �� 5H      
 � ��$ Calibri)�67   �� 5I      
 � ��$ 16(�65   �� 5K      
 � ��$ 5q�63   �� 5M      
 �@ CornerRadius$ 15��6U   5O      5Q      .+�5S      �� ._�5T      �� -   5         ��Name$ LogBox��5T      ��6.   �� *Logs+��5V      �� 5W      .R�5X   "   $�  $� Orientation$ Vertical=�69   5Y      5\      5]      � 5_       5a      �� 5b      �� 5c      $��3* �6(   �� 5d      .+�5g      �� $��0��6   ._�5h      A� $ 2��6&   .+�5j      �� $��Connect��6P   ��6%   �� *ConnectToServerCommand+6P   �� 5k      $��
DisConnect��6Q   ��6%   �� *DisConnectServerCommand+6Q   5m      5n      �� 5p      -   5        $��FileBox��5p       J� 	AllowDrop$
 True��6,   $��1��6`   ._�5q      �� $��Drag Your File Here��6$   $��Comic Sans MS)�6?   $��24(�6Z   $��Center=�6h   $��Center=�6�   5r      �� 5t      $��2��6   ._�5u      �� $��40��6M   $
��Send��6Y   ��6!   �� *SendFileToServerCommand+6Y   5v      5x      5z      5|      5}      5      