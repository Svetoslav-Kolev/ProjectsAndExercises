   M S B A M L   `   `   `  ����P  LElectronicShopManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:ElectronicShopManager.ViewsElectronicShopManager.Views  5   	   S.clr-namespace:ElectronicShopManager.Converters ElectronicShopManager.Converters  5   	   0    *ElectronicShopManager.Views.AddOrderWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ;local)clr-namespace:ElectronicShopManager.Views     5   	   X TXceed.Wpf.Toolkit, Version=4.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5      ?
converters.clr-namespace:ElectronicShopManager.Converters  5   	     � Title$  AddOrderWindow��5
   	   $	��450��6    $	��800��6-    � 	Resources 5      6  0ElectronicShopManager.Converters.StatusConverter   StatusConverter��%   &	          5   
   6%   5      .��5      �� -   5         ��Name$ ContainerPanel��5      .+�5   
   �� $*��$Select Employee to handle the order:��6   �� 5   
    �� DisplayMemberPath$ Value��6A    �� SelectedValuePath$	 Key��6[   ��6   �� *	Employees+ �� Mode$ TwoWay=� �� SelectedValue 6s   �� *SelectedEmployeeID+�� 5   
   $��Indicate order status:��6   �� 5   
   ��6   �� *
Statuses+$ TwoWay=� �� SelectedItem 6@   �� *Status+��5      �� 5      .R�5      �� ��6    ��  �� 	Converter# [  5      5      �� 5   
   $-��'Initial Product purchase for the Order:��6   �� 5   
   -   5        	 ��Name$	 ProductsComboBox��5      $ Value��6Z   $	 Key��6t   ��6-   �� *
Products+$ TwoWay=� 6�   �� *SelectedProductID+�� 5   
   $��Address:��6   �� 5   
   ��6   �� *	Address+�� 5   
   $S��MQuantity of product to purchase (select a product before selecting quantity):��6   �� 5   
   -   5        $	 QuantitiesComboBox��5      
 � ToolTip$5
 /Must select a product before selecting quantity��6P   ��6/   �� *
Quantity+ 6�   �� *SelectedQuantity+�� 5   
   $.��(Discount for the order: (in percentages)��6   �� 5    
   -   5        $��	0,20,0,20q�5    �   ��6   �� *
Discount+$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�" �� ValidatesOnNotifyDataErrors . .��6�    =� ErrorTemplate 5!      �� 5"      .R�5#      �� .+�5$      ��  ��Name$ textBox��64   �� 5%      ��6'   �� ��5&      �� 5'   "   .R�5(   &   �� 
���  ����60   �� *ErrorContent+6N   5)   #   5+      5,      5-      .��5.   (   5/      �� 50   
   -   5        $��	Add Order��50      $'
 !Please fill out all details first��6%    ^� ShowOnDisabled . $��0 50 0 0q�6�   $	��150��6�   $��30��6�   ��6�   �� *AddOrderCommand+6�   �� 51   
   ��6   �� *Notification+52      53      