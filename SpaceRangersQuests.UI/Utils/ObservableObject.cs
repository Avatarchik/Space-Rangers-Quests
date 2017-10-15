using System.ComponentModel;
using JetBrains.Annotations;

namespace SpaceRangersQuests.UI.Utils
{
    /// <summary>
    /// Класс для обёртывания работы с INotifyPropertyChanged
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Дергаем событие по изменению свойства
        /// </summary>
        /// <param name="propertyName">Имя свойства</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(/*[CallerMemberName]*/ string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
