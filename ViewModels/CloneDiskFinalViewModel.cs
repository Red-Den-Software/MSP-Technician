using msptool.Button_Commands;
using msptool.Functions;
using msptool.MVVM;
using msptool.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace msptool.ViewModels
{
    public class CloneDiskFinalViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ICommand CloneCommand { get; }
        private CloneDiskFinal cloneDiskFinal;
        public CloneDiskFinalViewModel()
        {

            CloneCommand = new AsyncRelayCommand(StartCloneAsync);

        }
        private async Task StartCloneAsync(object? parameter)
        {
            var progress = new Progress<CopyProgress>(p =>
            {
                CopyProgress = p.Percentage;
            });

            await Task.Run(() =>
            {
                LowLevelDiskCopy.CopySectors(progress);
            });

        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private double _copyProgress;

        public double CopyProgress
        {
            get => _copyProgress;
            set
            {
                _copyProgress = value;
                OnPropertyChanged();
            }
        }

        public async Task StartCopyAsync()
        {
            var progress = new Progress<CopyProgress>(p =>
            {
                CopyProgress = p.Percentage;
            });

            await Task.Run(() =>
            {
                LowLevelDiskCopy.CopySectors(progress);
            });
        }
    }
}

