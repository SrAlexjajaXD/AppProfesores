namespace AppProfesores.Views;

public partial class Menu : ContentPage
{
	public Menu()
	{
		InitializeComponent();
	}

	private async void Button_Clicked(object sender, EventArgs e)
	{
		await Navigation.PopModalAsync();
    }
}