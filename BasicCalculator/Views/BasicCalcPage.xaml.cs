using BasicCalculator.ViewModels;

namespace BasicCalculator.Views;

public partial class BasicCalcPage : ContentPage
{
    public BasicCalcPage()
    {
        InitializeComponent();

        BindingContext = new BasicCalcViewModel();
    }
}