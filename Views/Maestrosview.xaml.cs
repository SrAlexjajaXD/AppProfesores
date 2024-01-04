using AppProfesores.Data;

namespace AppProfesores.Views;

public partial class Maestrosview : ContentPage
{
	public Maestrosview()
	{
		InitializeComponent();
        Conexion.consultarProfes(MyListView);
    }

	private async void Regresar_Clicked(object sender, EventArgs e)
	{
        await Navigation.PopModalAsync();
    }
}