using System;
using System.Collections.ObjectModel;

namespace MyNet.Avalonia.PerfTest.ViewModels;

public class FormsViewModel : ViewModelBase
{
    private string _textField1 = string.Empty;
    private string _textField2 = string.Empty;
    private string _textField3 = string.Empty;
    private string _email = string.Empty;
    private string _phone = string.Empty;
    private string _address = string.Empty;
    private string _city = string.Empty;
    private string _zipCode = string.Empty;
    private string _country = string.Empty;
    private bool _checkbox1;
    private bool _checkbox2;
    private bool _checkbox3;
    private bool _checkbox4;
    private bool _checkbox5;
    private string _selectedItem = "Option 1";
    private DateTimeOffset _selectedDate = DateTimeOffset.Now;
    private string _multilineText = string.Empty;

    public ObservableCollection<string> ComboBoxItems { get; } = new()
    {
        "Option 1", "Option 2", "Option 3", "Option 4", "Option 5",
        "Option 6", "Option 7", "Option 8", "Option 9", "Option 10"
    };

    public string TextField1
    {
        get => _textField1;
        set => SetProperty(ref _textField1, value);
    }

    public string TextField2
    {
        get => _textField2;
        set => SetProperty(ref _textField2, value);
    }

    public string TextField3
    {
        get => _textField3;
        set => SetProperty(ref _textField3, value);
    }

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Phone
    {
        get => _phone;
        set => SetProperty(ref _phone, value);
    }

    public string Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    public string City
    {
        get => _city;
        set => SetProperty(ref _city, value);
    }

    public string ZipCode
    {
        get => _zipCode;
        set => SetProperty(ref _zipCode, value);
    }

    public string Country
    {
        get => _country;
        set => SetProperty(ref _country, value);
    }

    public bool Checkbox1
    {
        get => _checkbox1;
        set => SetProperty(ref _checkbox1, value);
    }

    public bool Checkbox2
    {
        get => _checkbox2;
        set => SetProperty(ref _checkbox2, value);
    }

    public bool Checkbox3
    {
        get => _checkbox3;
        set => SetProperty(ref _checkbox3, value);
    }

    public bool Checkbox4
    {
        get => _checkbox4;
        set => SetProperty(ref _checkbox4, value);
    }

    public bool Checkbox5
    {
        get => _checkbox5;
        set => SetProperty(ref _checkbox5, value);
    }

    public string SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value);
    }

    public DateTimeOffset SelectedDate
    {
        get => _selectedDate;
        set => SetProperty(ref _selectedDate, value);
    }

    public string MultilineText
    {
        get => _multilineText;
        set => SetProperty(ref _multilineText, value);
    }
}
