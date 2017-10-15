using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using SpaceRangersQuests.Model;
using SpaceRangersQuests.Model.Entity;
using SpaceRangersQuests.Model.Player;
using SpaceRangersQuests.UI.Utils;

namespace SpaceRangersQuests.UI.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            OpenCommand = new RelayCommand(() => OpenCommandExecute());
            TransitionCommand = new RelayCommand(transition=> TransitionCommandExecute(transition));
            Player = new Player();
        }

        public ICommand OpenCommand { get; private set; }

        public ICommand TransitionCommand { get; private set; }

        public Player Player { get; set; }

        private void OpenCommandExecute()
        {
            var ofd = new OpenFileDialog
            {
                Filter = "QuestFiles | *.qm",
                Multiselect = false,
                CheckFileExists = true,
                CheckPathExists = true,
            };

            var dialogResult = ofd.ShowDialog();
            if (!dialogResult.HasValue || !dialogResult.Value)
                return;

            using (var stream = File.OpenRead(ofd.FileName))
            {
                Player.Play(FileQuest.Load(stream));
            }
        }

        private void TransitionCommandExecute(object transitionValue)
        {
            var transition = transitionValue as Transition;
            Player.StartTransition(transition);
        }
    }
}
