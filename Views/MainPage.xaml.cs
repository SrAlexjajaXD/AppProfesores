using AppProfesores.Data;

namespace AppProfesores.Views;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
        await Navigation.PushModalAsync(new Views.MenuGeneral());
    }

	private void SalirBtn_Clicked(object sender, EventArgs e)
	{
		Application.Current.Quit();
	}

	private async void AboutBtn_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushModalAsync(new Views.Menu());
    }
}

