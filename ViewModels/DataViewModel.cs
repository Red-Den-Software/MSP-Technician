using Microsoft.Xaml.Behaviors.Core;
using msptool.Commands.UpdateViewCommand;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace msptool.ViewModels
{
    internal class DataViewModel : INotifyPropertyChanged
    {
        private object _currentView;
        public DataViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(UpdateView);
           
        }

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
        private void UpdateView(object parameter)
        {
            switch (parameter?.ToString())
            {
                case "CloneDisk":
                    CurrentView = new CloneDiskViewModel();
                    break;
               
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
        
        
    

    
