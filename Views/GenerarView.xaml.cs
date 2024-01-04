
using Acr.UserDialogs;
using AppProfesores.Data;

namespace AppProfesores.Views;

public partial class GenerarView : ContentPage
{
	public GenerarView()
	{
		InitializeComponent();
		
		Conexion.consultarHorario(MyListView);
	}

	private async void Regresar_Clicked(object sender, EventArgs e)
	{
		await Navigation.PopModalAsync();
	}

	private void Eliminar_Clicked(object sender, EventArgs e)
	{
		Conexion.eliminarHorarios();
		MyListView.ItemsSource = null;

    }

	private void Entry_TextChanged(object sender, TextChangedEventArgs e)
	{
		Conexion.actualizarHorario(MyListView, entryProfesor.Text);
	}

	private void Generar_Clicked(object sender, EventArgs e)
	{
        Conexion.eliminarHorarios();
        Conexion.generarHorarios();
        Conexion.consultarHorario(MyListView);

    }

    private void GenerarIndiviudal_Clicked(object sender, EventArgs e)
	{
		if (entryProfesor.Text != null)
		{
            Conexion.eliminarHorarios();
            Conexion.generarHorarioIndiDe(entryProfesor.Text);
            Conexion.consultarHorario(MyListView);
		}
		else
		{
			Conexion.eliminarHorarios();
			Conexion.generarHorarioIndi();
			Conexion.consultarHorario(MyListView);
		}
        
    }
}