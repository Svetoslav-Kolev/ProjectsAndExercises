   M S B A M L   `   `   `  ����P  LElectronicShopManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:ElectronicShopManager.ViewsElectronicShopManager.Views  5   	   S.clr-namespace:ElectronicShopManager.Converters ElectronicShopManager.Converters  5   	   1    +ElectronicShopManager.Views.ModifyOrderView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ;local)clr-namespace:ElectronicShopManager.Views     5   	   E
converters.clr-namespace:ElectronicShopManager.Converters     5   	     � Title$  ModifyOrderView��5	   	   $
��Auto��6!   $	��800��6/    � 	Resources 5
      6  0ElectronicShopManager.Converters.StatusConverter   StatusConverter��%   &	          5   
   6%   5      .��5      �� .+�5   
   �� $*��$Select Employee to handle the order:��6   �� 5   
   -   5         ��Name$ employeesCmb��5       �� DisplayMemberPath$ Value��6W    �� SelectedValuePath$	 Key��6r   ��6)   �� *	Employees+ �� Mode$ TwoWay=� �� SelectedValue 6�   �� *SelectedEmployeeID+$ TwoWay=��� 5   
   $��Indicate order status:��6   �� 5   
   ��6   �� *
Statuses+$ TwoWay=� �� SelectedItem 6@   �� *Status+��5      �� 5      .R�5      �� ��6    ��  �� 	Converter# [  5      5      �� 5   
   $��Address:��6   �� 5   
   $��0 0 0 20q�6~   ��6   �� *	Address+$ TwoWay=�	 �� UpdateSourceTrigger$	 PropertyChanged=�"
 �� ValidatesOnNotifyDataErrors
 . .��6�    =� ErrorTemplate 5      �� 5      .R�5      �� .+�5      ��  ��Name$ textBox��64   �� 5      ��6'   �� ��5      �� 5    "   .R�5!   &   �� 
���  ����60   �� *ErrorContent+6N   5"   #   5$      5%      5&      .��5'   (   5(      �� 5)   
   $��Receipt Number:��6   �� 5*   
   $��0 0 0 20q�6�   ��6   �� *ReceiptNumber+$ TwoWay=�$	 PropertyChanged=�
 . .��6�    5+      �� 5,      .R�5-      �� .+�5.      �� $ textBox��64   �� 5/      ��6'   �� ��50      �� 51   "   .R�52   &   �� 
���  ����60   �� *ErrorContent+6N   53   #   55      56      57      .��58   (   59      �� 5:      $G��ADate of purchase: (please insert in format {yyyy-MM-dd HH:mm:ss})��6   �� 5;   
   ��6   �� *	DateGiven+$ TwoWay=� �� StringFormat$ yyyy-MM-dd HH:mm:ss��$	 PropertyChanged=�
 . .��6�    5<      �� 5=      .R�5>      �� .+�5?      �� $ textBox��64   �� 5@      ��6'   �� ��5A      �� 5B   "   .R�5C   &   �� 
���  ����60   �� *ErrorContent+6N   5D   #   5F      5G      5H      .��5I   (   5J      �� 5K   
   $��Modify Order��6    � ToolTip$1 +Please fill out all details correctly first��6(    ^� ShowOnDisabled . $��0 50 0 0q�6�   $	��150��6�   $��30��6�   ��6�   �� *ModifyOrderCommand+6�   �� 5L   
   $��Center=�62   ��6   �� *Notification+62   5M      5N      