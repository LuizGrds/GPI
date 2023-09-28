namespace MauiAppTestesProjete2;

public partial class MenuPage : ContentPage
{
    string UsuarioAtivo = Listas.UsuarioAtivo;
    public MenuPage()
	{
		InitializeComponent();
        BemVindo.Text = "Bem-Vindo(a) " + UsuarioAtivo;
    }

    private void OnEstatisticasButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new EstatisticasPage());
    }

    private void OnCadastroClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new CadastroPage());
    }

}