using msptool;
using System.ComponentModel;
using System.Media;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;

public class PopupViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    
    #region Popup Methods

    public void PopupViewModel_Error(object sender, string message)
    {
        SystemSounds.Exclamation.Play();

        var vm = new PopupViewModel
        {
            TitleBG = Brushes.Red,
            plogo = "X",
            plogoForeground = Brushes.Red,
            EllipseVisibility = Visibility.Visible,
            EllipseStroke = Brushes.Red,

            errorText = message,

            mVisibility = Visibility.Visible,
            mbutText = "OK",

            mbuttonthickness = "2",
            rbuttonthickness = "0",
            lbuttonthickness = "0"
        };

        ShowPopup(vm);
    }

    public void PopupViewModel_Options(object sender, string message)
    {
        SystemSounds.Exclamation.Play();

        var vm = new PopupViewModel
        {
            

            EllipseVisibility = Visibility.Hidden,

         

            mVisibility = Visibility.Hidden,
            nVisibility = Visibility.Visible,
            YVisibility = Visibility.Visible,

            mbuttonthickness = "0",
            rbuttonthickness = "2",
            lbuttonthickness = "2",

           
        };

        ShowPopup(vm);
    }

    private void ShowPopup(PopupViewModel vm)
    {
        var popup = new PopupWindow
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            DataContext = vm
        };

        popup.Show();
    }

    #endregion

    #region Fields

    private Brush _ellipseStroke = Brushes.Red;
    private Visibility _yVisibility = Visibility.Visible;
    private Visibility _ellipseVisibility = Visibility.Hidden;
    private Visibility _nVisibility = Visibility.Visible;
    private Visibility _mVisibility = Visibility.Hidden;
    private string _plogo_font = "30";
    private string _errorText = string.Empty;
    private Brush _titlebg = Brushes.Red;
    private string _plogo = string.Empty;
    private Brush _plogoForeground = Brushes.Red;
    private string _title = string.Empty;
    private string _centerInlineText = string.Empty;
    private string _rbutText = string.Empty;
    private string _lbutText = string.Empty;
    private string _mbutText = string.Empty;
    private Visibility _pathBox = Visibility.Visible;
    private string _mbuttonthickness = "0";
    private string _rbuttonthickness = "0";
    private string _lbuttonthickness = "0";

    #endregion

    #region Properties
    
    public Brush EllipseStroke
    {
        get => _ellipseStroke;
        set
        {
            _ellipseStroke = value;
           
        }
    }
    public string pathText { get; set; } = string.Empty;
    public string centerInlineText
    {
        get => _centerInlineText;
        set
            {
            _centerInlineText = value;
        }
    }
    public ComboBox pathComboBox { get; set; } = new ComboBox();
    public string plogo
    {
        get => _plogo;
        set
        {
            _plogo = value;
           
        }
    }
    public string plogo_font
    {
        get => _plogo_font;
        set
        {
            _plogo_font = value;

        }
    }
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
         
        }
    }

    public Brush plogoForeground
    {
        get => _plogoForeground;
        set
        {
            _plogoForeground = value;
            
        }
    }

    public Visibility EllipseVisibility
    {
        get => _ellipseVisibility;
        set
        {
            _ellipseVisibility = value;
            
        }
    }

    public Brush TitleBG
    {
        get => _titlebg;
        set
        {
            _titlebg = value;
           
        }
    }

    public string errorText
    {
        get => _errorText;
        set
        {
            _errorText = value;
          
        }
    }

    public Visibility YVisibility
    {
        get => _yVisibility;
        set
        {
            _yVisibility = value;
           
        }
    }

    public Visibility nVisibility
    {
        get => _nVisibility;
        set
        {
            _nVisibility = value;

            if (value == Visibility.Visible)
            {
                rbuttonthickness = "2";
                lbuttonthickness = "2";
            }

           
        }
    }

    public string rbutText
    {
        get => _rbutText;
        set
        {
            _rbutText = value;
            
        }
    }
    public Visibility pathBox
            {
        get => _pathBox;
        set
        {
            _pathBox = value;
        }
    }
    public string lbutText
    {
        get => _lbutText;
        set
        {
            _lbutText = value;
         
        }
    }

    public string mbutText
    {
        get => _mbutText;
        set
        {
            _mbutText = value;
            
        }
    }
   
    public Visibility mVisibility
    {
        get => _mVisibility;
        set
        {
            _mVisibility = value;
           
        }
    }

    public string mbuttonthickness
    {
        get => _mbuttonthickness;
        set
        {
            _mbuttonthickness = value;
            
        }
    }

    public string rbuttonthickness
    {
        get => _rbuttonthickness;
        set
        {
            _rbuttonthickness = value;
            
        }
    }

    public string lbuttonthickness
    {
        get => _lbuttonthickness;
        set
        {
            _lbuttonthickness = value;
           
        }
    }

    #endregion
}