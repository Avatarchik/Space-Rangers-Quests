using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace SpaceRangersQuests.UI.Utils
{
    /// <summary>
    /// Класс для определения нахождения в дизайнере,
    /// а также простановке параметров в режиме дизайнера.
    /// </summary>
    public static class DesignTimeMode
    {
        private static bool? _isDesignModel;

        /// <summary>
        /// В дизайн моде?
        /// </summary>
        public static bool IsDesignMode
        {
            get
            {
                if (!_isDesignModel.HasValue)
                {
                    _isDesignModel = (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                                     || (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv");
                }

                return _isDesignModel.Value;
            }
        }

        #region Background
        /// <summary>
        /// <see cref="http://mnajder.blogspot.ru/2011/09/custom-design-time-attributes-in.html"/>
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached(
            "Background", typeof(System.Windows.Media.Brush), typeof(DesignTimeMode),
            new PropertyMetadata(BackgroundChanged));

        public static System.Windows.Media.Brush GetBackground(DependencyObject dependencyObject)
        {
            return (System.Windows.Media.Brush)dependencyObject.GetValue(BackgroundProperty);
        }
        public static void SetBackground(DependencyObject dependencyObject, System.Windows.Media.Brush value)
        {
            dependencyObject.SetValue(BackgroundProperty, value);
            SetValue(BackgroundProperty, dependencyObject, value);
        }
        private static void BackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetValue(BackgroundProperty, d, e.NewValue);
        }
        #endregion

        #region MinWidth

        public static readonly DependencyProperty MinWidthProperty = DependencyProperty.RegisterAttached(
            "MinWidth", typeof(double), typeof(DesignTimeMode),
            new PropertyMetadata(MinWidthChanged));

        public static double GetMinWidth(DependencyObject dependencyObject)
        {
            return (double)dependencyObject.GetValue(MinWidthProperty);
        }
        public static void SetMinWidth(DependencyObject dependencyObject, double value)
        {
            dependencyObject.SetValue(MinWidthProperty, value);
        }

        private static void MinWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetValue(MinWidthProperty, d, e.NewValue);
        }

        #endregion

        #region Visibility

        public static readonly DependencyProperty VisibilityProperty = DependencyProperty.RegisterAttached(
            "Visibility", typeof(Visibility), typeof(DesignTimeMode),
            new PropertyMetadata(VisibilityChanged));

        public static Visibility GetVisibility(DependencyObject dependencyObject)
        {
            return (Visibility)dependencyObject.GetValue(VisibilityProperty);
        }
        public static void SetVisibility(DependencyObject dependencyObject, Visibility value)
        {
            dependencyObject.SetValue(VisibilityProperty, value);
            SetValue(VisibilityProperty, dependencyObject, value);
        }

        private static void VisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!IsDesignMode)
                return;

            SetValue(VisibilityProperty, d, e.NewValue);
        }

        #endregion

        #region Fill

        public static readonly DependencyProperty FillProperty = DependencyProperty.RegisterAttached(
            "Fill", typeof(Brush), typeof(DesignTimeMode),
            new PropertyMetadata(FillChanged));

        public static Brush GetFill(DependencyObject dependencyObject)
        {
            return (Brush)dependencyObject.GetValue(FillProperty);
        }
        public static void SetFill(DependencyObject dependencyObject, Brush value)
        {
            dependencyObject.SetValue(FillProperty, value);
            SetValue(FillProperty, dependencyObject, value);
        }

        private static void FillChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!IsDesignMode)
                return;

            SetValue(FillProperty, d, e.NewValue);
        }

        #endregion

        private static void SetValue(DependencyProperty dependencyProperty, DependencyObject dependencyObject, object value)
        {
            SetValue(dependencyProperty.Name, dependencyObject, value);
        }

        private static void SetValue(string propertyName, DependencyObject dependencyObject, object value)
        {
            if (!IsDesignMode)
                return;

            var propertyInfo = dependencyObject.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
                return;

            propertyInfo.SetValue(dependencyObject, value, null);
        }
    }
}
