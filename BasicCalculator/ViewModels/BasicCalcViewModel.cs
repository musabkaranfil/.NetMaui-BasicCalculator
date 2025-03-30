using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NCalc;

namespace BasicCalculator.ViewModels;

public partial class BasicCalcViewModel : ObservableObject
{
    [ObservableProperty]
    string _input = "";

    [ObservableProperty]
    string _expression = "";

    [ObservableProperty]
    string _result = "";

    bool _switchToNewExpression = true;
    bool _operatorAssigned = false;

    [RelayCommand]
    private void InputNumber(string digit)
    {
        if (digit == "," && Input.Contains(","))
            return;

        if (digit == "0" && Input.Substring(0) == "0") 
            return;

        if (digit == "," && Input.Length == 0)
            Input += "0";

        if (_switchToNewExpression is true)
            Input = "";

        if (digit == "," && _switchToNewExpression is true)
            Input = "0";

        _switchToNewExpression = false;

        Input += digit;
    }

    [RelayCommand]
    private void Operator(string opKey)
    {
        if (Input.Length == 0)
            return;

        if (_operatorAssigned is true && _switchToNewExpression is true)
            return;

        if (_operatorAssigned is true && Input.Length < 1)
            return;

        if (Input.Contains("-") && Input.Length == 1)
            return;

        Expression += Input + " " + opKey + " ";
        Result = Expression;

        _switchToNewExpression = true;
        _operatorAssigned = true;
    }

    [RelayCommand]
    private void Calculate()
    {
        try
        {
            if (_operatorAssigned is false || _switchToNewExpression is true)
                return;

            Expression += Input;
            
            string correctedString = Expression.Replace(',', '.');
            Expression expression = new Expression(correctedString);
            object result = expression.Evaluate()!;

            Input = result.ToString()!;

            if (result is double && (double)result % 1 != 0)
                Input = Convert.ToDouble(result).ToString("0.####");

            Result = Expression + " = " + Input;

            Expression = "";

            _switchToNewExpression = true;
            _operatorAssigned = false;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Hata!", $"İşlem yapılamıyor: {ex}", "Tamam");
        }
    }

    [RelayCommand]
    private void AllClear()
    {
        Input = "";
        Result = "";
        Expression = "";

        _operatorAssigned = false;
        _switchToNewExpression = false;
    }

    [RelayCommand]
    private void Delete()
    {
        if (Input.Length > 0 && _switchToNewExpression is false)
            Input = Input.Substring(0, Input.Length - 1);
    }

    [RelayCommand]
    private void Negate()
    {
        if (Input.Contains("-"))
            return;

        if (_switchToNewExpression is true)
        {
            Input = "-";
            _switchToNewExpression = false;
            return;
        }

        Input = "-" + Input;

        _switchToNewExpression = false;
    }

    [RelayCommand]
    private void Percentage()
    {
        if (Input != "" & !Input.Contains(","))
        {
            Input = Input + "/ 100";
            Expression expression = new Expression(Input);
            object? result = expression.Evaluate();
            Input = result!.ToString()!;

            _switchToNewExpression = true;
        }
    }
}