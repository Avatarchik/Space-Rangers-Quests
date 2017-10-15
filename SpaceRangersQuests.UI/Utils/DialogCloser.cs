using System.Windows;

namespace SpaceRangersQuests.UI.Utils
{
    /// <summary>
    /// <see cref="http://blog.excastle.com/2010/07/25/mvvm-and-dialogresult-with-no-code-behind/comment-page-1/"/>
    /// </summary>
    public static class DialogCloser
    {
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached(
                "DialogResult",
                typeof(bool?),
                typeof(DialogCloser),
                new PropertyMetadata(DialogResultChanged));

        private static void DialogResultChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window == null)
                return;
            window.DialogResult = e.NewValue as bool?;
        }

        public static void SetDialogResult(Window target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);
        }

        public static bool? GetDialogResult(Window target)
        {
            return (bool?) target.GetValue(DialogResultProperty);
        }
    }
}
