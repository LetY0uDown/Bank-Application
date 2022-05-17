using System.Windows.Controls;

namespace Bank.Views.Pages;

public sealed partial class TransactionsPage : Page 
{ 
    public TransactionsPage() => InitializeComponent();

    private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        if (e.Text[0].Equals('-')) e.Handled = true;
    }
}
