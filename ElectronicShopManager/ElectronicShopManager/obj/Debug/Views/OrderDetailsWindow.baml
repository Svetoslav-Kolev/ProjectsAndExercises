   M S B A M L   `   `   `  ����P  LElectronicShopManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:ElectronicShopManager.ViewsElectronicShopManager.Views  5   	   4    .ElectronicShopManager.Views.OrderDetailsWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ;local)clr-namespace:ElectronicShopManager.Views     5   	   X TXceed.Wpf.Toolkit, Version=4.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	     � Title$  OrderDetailsWindow��5	   	   $
��Auto��6$   $	��800��62   .��5
      �� .+�5   
   &   System.Windows.Controls.DataGrid  -   5           
IsReadOnly . ��5   %   �� *	Details+ �� SelectedItem 6{   �� *SelectedDetail+5      �� 5   
   $	��450��6   $��20q�6   $��1��6'   
���   �6;   �� 5   
   $'��!Options to add or remove a detail��6   $��Center=�6<   �� 5   
   $��Product to purchase:��6   �� 5   
   -   5         ��Name$ ProductsComboBox��5       �� DisplayMemberPath$ Value��6Z    �� SelectedValuePath$	 Key��6t   ��6-   �� *
Products+ �� Mode$ TwoWay=� �� SelectedValue 6�   �� *SelectedProductID+�� 5   
   $S��MQuantity of product to purchase (select a product before selecting quantity):��6   �� 5   
   -   5        $ QuantitiesComboBox��5       � ToolTip$5 /Must select a product before selecting quantity��6P   ��6/   �� *
Quantity+ 6�   �� *SelectedQuantity+�� 5   
   $ ��Discount: (in percentages)��6   �� 5   
   -   5        $��myUpDownControl��5   �   $��	0,20,0,20q�6�   ��6   �� *
Discount+$ TwoWay=�	 �� UpdateSourceTrigger$	 PropertyChanged=�"
 �� ValidatesOnNotifyDataErrors
 . .��6�    =� ErrorTemplate 5      �� 5      .R�5      �� .+�5      ��  ��Name$ textBox��64   �� 5      ��6'   �� ��5      �� 5   "   .R�5   &   �� 
���  ����60   �� *ErrorContent+6N   5   #   5       5!      5"      .��5#   (   5$      �� 5%   
   $��Add Details for the order��6   $' !Please fill out all details first��65    ^� ShowOnDisabled . $��0 50 0 0q�6�   $	��150��6�   $��30��6�   ��6�   �� *AddDetailsCommand+6�   �� 5&   
   $��Remove Order Detail��6   $# Select detail to remove first��6/    . $��0 50 0 0q�6�   $	��150��6�   $��30��6�   ��6|   �� *RemoveDetailCommand+6�   �� 5'   
   $��Center=�62   ��6   �� *Notification+62   5(      5)      