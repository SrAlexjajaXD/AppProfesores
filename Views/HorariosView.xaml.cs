using AppProfesores.Data;

namespace AppProfesores.Views;

public partial class HorariosView : ContentPage
{
	public HorariosView()
	{
		InitializeComponent();
        Conexion.consultarMates(MyListView);
    }

	private async void Regresar_Clicked(object sender, EventArgs e)
	{
		await Navigation.PopModalAsync();
	}
}