
using Microsoft.Data.SqlClient;
using AppProfesores.Data;
using Microsoft.Identity.Client;
using System.ComponentModel;
using Microsoft.Maui.Controls;

namespace AppProfesores.Views;

public partial class MenuGeneral : ContentPage
{
	public MenuGeneral()
	{
		InitializeComponent();
	}

	private async void Salir_Clicked(object sender, EventArgs e)
	{
		await Navigation.PopModalAsync();
	}

	private async void generar_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushModalAsync(new Views.GenerarView());
	}

	private async void horarios_Clicked(object sender, EventArgs e)
	{
        await Navigation.PushModalAsync(new Views.HorariosView());
    }

	private async void maestros_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushModalAsync(new Maestrosview());

		
    }

}