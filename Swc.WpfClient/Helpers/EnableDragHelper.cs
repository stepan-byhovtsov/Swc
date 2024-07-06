using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Swc.WpfClient
{
    public class EnableDragHelper
    {
        public static readonly DependencyProperty EnableDragProperty = DependencyProperty.RegisterAttached(
            "EnableDrag",
            typeof (bool),
            typeof (EnableDragHelper),
            new PropertyMetadata(default(bool), OnLoaded));

        private static void OnLoaded(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var uiElement = dependencyObject as UIElement;
            if (uiElement == null || (dependencyPropertyChangedEventArgs.NewValue is bool) == false)
            {
                return;
            }
            if ((bool)dependencyPropertyChangedEventArgs.NewValue  == true)
            {
                uiElement.MouseMove += UIElementOnMouseMove;
                uiElement.MouseLeftButtonDown += UiElementOnMouseLeftButtonDown;
            }
            else
            {
                uiElement.MouseMove -= UIElementOnMouseMove;
            }

        }

        private static void UiElementOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var uiElement = sender as UIElement;
            if (uiElement != null)
            {
                if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
                {
                    DependencyObject parent = uiElement;
                    int avoidInfiniteLoop = 0;
                    // Search up the visual tree to find the first parent window.
                    while ((parent is Window) == false)
                    {
                        parent = VisualTreeHelper.GetParent(parent);
                        avoidInfiniteLoop++;
                        if (avoidInfiniteLoop == 1000)
                        {
                            // Something is wrong - we could not find the parent window.
                            return;
                        }
                    }
                    var window = (Window) parent; 
                    window.WindowState = window.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
                }
            }
        }

        private static void UIElementOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            var uiElement = sender as UIElement;
            if (uiElement != null)
            {
                if (mouseEventArgs.LeftButton == MouseButtonState.Pressed)
                {
                    DependencyObject parent = uiElement;
                    int avoidInfiniteLoop = 0;
                    // Search up the visual tree to find the first parent window.
                    while ((parent is Window) == false)
                    {
                        parent = VisualTreeHelper.GetParent(parent);
                        avoidInfiniteLoop++;
                        if (avoidInfiniteLoop == 1000)
                        {
                            // Something is wrong - we could not find the parent window.
                            return;
                        }
                    }
                    var window = (Window) parent;
                    if (window.WindowState == WindowState.Maximized)
                    {
                        window.WindowState = WindowState.Normal;
                        window.Top = 0;
                        window.Left = mouseEventArgs.GetPosition(window).X - window.ActualWidth / 2;
                    }
                    window.DragMove();
                }
            }
        }

        public static void SetEnableDrag(DependencyObject element, bool value)
        {
            element.SetValue(EnableDragProperty, value);
        }

        public static bool GetEnableDrag(DependencyObject element)
        {
            return (bool)element.GetValue(EnableDragProperty);
        }
    }
}
