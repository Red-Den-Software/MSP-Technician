using msptool;
using System.ComponentModel;
using System.Media;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

public class PopupViewModel
{
    
    private System.Windows.Media.Brush _ellipseStroke = System.Windows.Media.Brushes.Red;
    public System.Windows.Media.Brush EllipseStroke
    {
        get => _ellipseStroke;
        set
        {
            _ellipseStroke = value;
            
        }
    }
    private System.Windows.Visibility _yVisibility = System.Windows.Visibility.Visible;
    private System.Windows.Visibility _ellipseVisibility = System.Windows.Visibility.Visible;
    private string _errorText;
    private System.Windows.Media.Brush _titlebg = System.Windows.Media.Brushes.Red;
    private string _plogo = string.Empty;
    private System.Windows.Media.Brush _plogoForeground = System.Windows.Media.Brushes.Red;
    public string plogo
    {
        get => _plogo; set{
           _plogo = value;
        }
    }
    private string _title;
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
        }
    }
    public System.Windows.Media.Brush plogoForeground
    {
        get => _plogoForeground;
        set
        {
            _plogoForeground = value;

        }
    }   
    public System.Windows.Visibility EllipseVisibility
    {
        get => _ellipseVisibility;
        set
        {
            _ellipseVisibility = System.Windows.Visibility.Visible;
            
        }
    }
    public System.Windows.Media.Brush TitleBG
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
    public System.Windows.Visibility YVisibility
    {
        get => _yVisibility;
        set
        {
            _yVisibility = System.Windows.Visibility.Visible;

        }
    }
    private System.Windows.Visibility _nVisibility = System.Windows.Visibility.Visible;
    public System.Windows.Visibility nVisibility
    {
        get => _nVisibility;
        set { _nVisibility = System.Windows.Visibility.Visible; }
    }
    private string _rbutText;
    public string rbutText
    {
        get => _rbutText;
        set { _rbutText = value; }
    }
    private string _lbutText;
    public string lbutText
    {
        get => _lbutText;
        set { _lbutText = value; }
    }
    private string _mbutText;
    public string mbutText
    {
        get => _mbutText;
        set { _mbutText = value; }
    }
    private System.Windows.Visibility _mVisibility = System.Windows.Visibility.Visible;
    public System.Windows.Visibility mVisibility
    {
        get => _mVisibility;
        set { _mVisibility = System.Windows.Visibility.Visible; }
    }
    private string _mbuttonthickness;
    public string mbuttonthickness

    {
        get => _mbuttonthickness;
        set
        {
            _mbuttonthickness = value;
        }
    }
    private string _rbuttonthickness;
    public string rbuttonthickness
    {
        get => _rbuttonthickness;
        set { _rbuttonthickness = value; }
    }
    private string _lbuttonthickness;
    public string lbuttonthickness
    {
        get => _lbuttonthickness;
        set { _lbuttonthickness = value; }
    }
}