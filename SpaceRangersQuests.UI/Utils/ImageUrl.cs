using System;
using System.Windows;

namespace SpaceRangersQuests.UI.Utils
{
    /// <summary>
    /// Вспомогательный класс для хранения Uri картинок в ресурсах
    /// </summary>
    public class ImageUrl : DependencyObject
    {
        public static readonly DependencyProperty ImageUriProperty = DependencyProperty
            .Register("ImageUri", typeof(Uri), typeof(ImageUrl), new PropertyMetadata(default(Uri)));

        /// <summary>
        /// Ссылка на кртинку.
        /// Использовать формат: /&lt;App&gt;;component/&lt;ImagePath&gt;
        /// </summary>
        public Uri ImageUri
        {
            get { return (Uri)GetValue(ImageUriProperty); }
            set { SetValue(ImageUriProperty, value); }
        }
    }
}
