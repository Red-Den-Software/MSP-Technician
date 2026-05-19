using Caliburn.Micro;
using msptool.Commands;
using msptool.Commands.UpdateViewCommand;
using msptool.MVVM;
using msptool.ViewModels;
using msptool.Views;
using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace msptool.ViewModels
{

    public class ShellViewModel : INotifyPropertyChanged
    {
        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
        public LinearGradientBrush GlassBackgroundBrush { get; } = new LinearGradientBrush
        {
            StartPoint = new System.Windows.Point(0, 0),
            EndPoint = new System.Windows.Point(1, 1),
            
            GradientStops = new GradientStopCollection
    {
        // Top highlight
        new GradientStop(
            (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF2A2D32"), 0.0),

        new GradientStop(
            (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF1F2125"), 0.5),

        new GradientStop(
            (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF16181C"), 1.0)
    }
        };

        public ICommand UpdateViewCommand { get; }

        public ShellViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(UpdateView);

            CurrentView = new HomeViewModel();
        }

        private void UpdateView(object parameter)
        {
            switch (parameter?.ToString())
            {
                case "Data":
                    CurrentView = new DataViewModel();
                    break;
                case "Tools":
                    CurrentView = new ToolViewModel();
                    break;
                case "Home":
                    CurrentView = new HomeViewModel();
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

