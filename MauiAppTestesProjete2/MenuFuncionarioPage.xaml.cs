namespace MauiAppTestesProjete2;

public partial class MenuFuncionarioPage : ContentPage
{
    string UsuarioAtivo = Listas.UsuarioAtivo;
    public MenuFuncionarioPage()
    {
        InitializeComponent();
        BemVindo.Text = "Bem-Vindo(a) " + UsuarioAtivo;
    }

    private void OnQrCodeButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new QrCodePage());
    }

    private void OnEstatisticasButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new EstatisticasFuncionarioPage());
    }

}