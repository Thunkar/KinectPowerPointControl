   M S B A M L   `   `   `  ���� R  NKinectPowerPointControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:KinectPowerPointControl.ViewModel!KinectPowerPointControl.ViewModel  W0clr-namespace:KinectPowerPointControl.Converters"KinectPowerPointControl.Converters  +    %KinectPowerPointControl.View.MainView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     >vm/clr-namespace:KinectPowerPointControl.ViewModel     G
converters0clr-namespace:KinectPowerPointControl.Converters       N� Title$  MainView�� � 	Resources 5   /KinectPowerPointControl.ViewModel.MainViewModel   MainViewModelE  ?KinectPowerPointControl.Converters.BooleanToVisibilityConverter   BooleanToVisibilityConverterC   =KinectPowerPointControl.Converters.KinectCoordinatesConverter  KinectCoordinatesConverter@   :KinectPowerPointControl.Converters.BooleanToColorConverter  BooleanToColorConverter;   5KinectPowerPointControl.Converters.TextToIntConverter  TextToIntConverterA  ;KinectPowerPointControl.Converters.GestureToStringConverter  GestureToStringConverter �� 
TargetType  h1 �� Property �� Value  h2  p��%�  &	        &	      &	 
     &	      &	      &	      &	      &	 �     &	                  �� " ��.���  � ��$
 Bold"���  � ��$ 21(���  � ��$ Center=���  � ��$ 0,0,10,0q��� " ��.���  � ��$
 Bold"���  � ��$ 20(���  � ��$ Center=���  � ��$ 0,0,10,0q��� " ��.���  � ��$
 Bold"���  � ��$ 20(���  � ��$ Center=���  � ��$ Center=�.���  � DataContext# [   � ColumnDefinitions �� $��* ��� $
��Auto ��� $��* �.+��� $��10q�$��Stretch=�$��Center=�.+��� $��Gesture Status��$��32(�� $
��0,10q� �� $��2* ��� $��* � � RowDefinitions �� $��* ��� $��* ��� $��* ��� $��* �.+��� $��Left hand status��#��[ �� $��1��#��[ ���� *)'KinectGestureProcessor.CurrentLeftState+ �� 	Converter# [ �� $��Left separated��$��1��#��[ �� $��1��$��1��#��[ ���� *&$KinectGestureProcessor.LeftSeparated+# [ ���� *&$KinectGestureProcessor.LeftSeparated+�� $��	Left grip��$��2��#��[ �� $��1��$��2��#��[ ���� *!KinectGestureProcessor.LeftGrip+# [ ���� *!KinectGestureProcessor.LeftGrip+�� $��Left up��$��3��#��[ �� $��1��$��3��#��[ ���� *KinectGestureProcessor.LeftUp+# [ ���� *KinectGestureProcessor.LeftUp+� $
��0,10q� �� $��2* ��� $��* � �� $��* ��� $��* ��� $��* ��� $��* �.+��� $��Right hand status��$��0��#��[ �� $��1��$��0��#��[ ���� **(KinectGestureProcessor.CurrentRightState+# [ �� $��Right separated��$��1��#��[ �� $��1��$��1��#��[ ���� *'%KinectGestureProcessor.RightSeparated+# [ ���� *'%KinectGestureProcessor.RightSeparated+�� $��
Right grip��$��2��#��[ �� $��1��$��2��#��[ ���� *" KinectGestureProcessor.RightGrip+# [ ���� *" KinectGestureProcessor.RightGrip+�� $��Right up��$��3��#��[ �� $��1��$��3��#��[ ���� * KinectGestureProcessor.RightUp+# [ ���� * KinectGestureProcessor.RightUp+� $��Center=�$��1�� �� $
��Auto ��� $
��Auto ��� $��* �.+��� 	 �� Orientation$	 
Horizontal=�$��Center=�.+��� $��
Tilt angle��$��Center=�$
��Bold"��� -   
 ��Name$
 AngleDecrease��$��<���� -    ��Name$ 	TiltAngle��$��Center=�$
��Bold"�$��Center=����� *KinectHandler.Tilt+ �� Mode$ TwoWay=�# [ �� -   $
 AngleIncrease��$��>���� $��1��$	��640��$	��480������ *KinectHandler.ImageFromKinect+� $��1��$	��Top=�$��10�� � IsIndeterminate �� *KinectHandler.Busy+y��� *KinectHandler.Busy+# [ �� $��1��$��Center=�$��Center=�$	��640��$	��480��
������ .+�A� 
����  �
����������� *$"KinectGestureProcessor.EllipseSize+���� *$"KinectGestureProcessor.EllipseSize+
 �� Top �� *%#KinectGestureProcessor.HeadPosition+# [  �� ConverterParameter$ Y�� �� Left �� *%#KinectGestureProcessor.HeadPosition+# [ $ X��A� 
����������� *!KinectGestureProcessor.LeftGrip+# [ ���� *$"KinectGestureProcessor.EllipseSize+���� *$"KinectGestureProcessor.EllipseSize+ �� *)'KinectGestureProcessor.LeftHandPosition+# [ $ Y�� �� *)'KinectGestureProcessor.LeftHandPosition+# [ $ X��A� 
����������� *" KinectGestureProcessor.RightGrip+# [ ���� *$"KinectGestureProcessor.EllipseSize+���� *$"KinectGestureProcessor.EllipseSize+ �� **(KinectGestureProcessor.RightHandPosition+# [ $ Y�� �� **(KinectGestureProcessor.RightHandPosition+# [ $ X���� $��2��$��Center=�$��5q�$��Center=�.+��� -   $
 OpenPPT������ $��Open presentation�� 	 ButtonContent#��[	 �� -   $
 ClosePPT������ $��Close presentation��#��[	 �  �� $��8* ��� $
��Auto �.+���  �� TextWrapping$ NoWrap=�$��18(� �� MaxLines$ 5��$��
10,10,0,10q����� *PowerPointConnector.File+�� -   $
 Browse��$��1��$��Browse��$��Right=�$��18(�$��0, 10q�