namespace AppProfesores;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		var navPage= new NavigationPage(new Views.MainPage());

		navPage.BarBackground=Colors.WhiteSmoke;

        MainPage = navPage;
	}
}
