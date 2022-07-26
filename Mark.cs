using System.Windows;

namespace MediaMarkInImage
{
    class Mark : DependencyObject
    {
        public static string GetMark(DependencyObject obj)
        {
            return (string)obj.GetValue(Mediapath);
        }

        public static void SetMark(DependencyObject obj, string value)
        {
            obj.SetValue(Mediapath, value);
        }

        public static readonly DependencyProperty Mediapath =
            DependencyProperty.RegisterAttached("Mediapath", typeof(string), typeof(Mark), new PropertyMetadata("", OnMarkChanged));

        private static void OnMarkChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var element = obj as UIElement;
            if (element != null)
            {
                //element.RenderTransformOrigin = new Point(0.5, 0.5);
                //element.RenderTransform = new RotateTransform((double)e.NewValue);
            }
        }
    }
}