using System.Windows.Controls;

namespace Bank.Views.Pages;

public sealed partial class TransactionsPage : Page 
{ 
    public TransactionsPage() => InitializeComponent();
    private static readonly Regex _numberRegex = new("^[0-9]*$");

    private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        if (!_numberRegex.IsMatch(e.Text)) e.Handled = true;
    }
}
