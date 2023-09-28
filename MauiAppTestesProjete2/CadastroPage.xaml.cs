using System;
using System.Collections.Generic;
using Npgsql;



namespace MauiAppTestesProjete2
{
    public partial class CadastroPage : ContentPage
    {
        private bool MostraSenha = false;
        private string Login;
        private string Senha;
        private readonly List<string> ListaLogin = Listas.ListaLogin;
        private readonly List<string> ListaSenha = Listas.ListaSenha;
        private readonly List<string> ListaLoginAdmin = Listas.ListaLoginAdmin;
        private const string ConnectionString = "Server=localhost;Port=5432;User Id=postgres;Password=123;Database=GPI;";

        public CadastroPage()
        {
            InitializeComponent();
        }

        private async void OnCadastrarClicked(object sender, EventArgs e)
        {
            Login = EntryNovoLogin.Text;
            Senha = EntryNovaSenha.Text;

            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Senha))
            {
                await DisplayAlert("Erro", "Entre com um Login/Senha Válido!", "Continuar");
                return;
            }

            // Verifique se o funcionário já existe
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM CadastroFuncionario WHERE Nome = @Login", connection))
            {
                cmd.Parameters.AddWithValue("Login", Login);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0)
                {
                    await DisplayAlert("Erro!", "Esse usuário já existe!", "Continuar");
                    return;
                }
            }

            // Inserir novo funcionário com nome e senha
            using (var cmd = new NpgsqlCommand("INSERT INTO CadastroFuncionario (Nome, Senha) VALUES (@Nome, @Senha)", connection))
            {
                cmd.Parameters.AddWithValue("Nome", Login);
                cmd.Parameters.AddWithValue("Senha", Senha);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    await DisplayAlert("Sucesso!", "Funcionário cadastrado com sucesso!", "Continuar");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Erro!", "Ocorreu um erro ao cadastrar o funcionário.", "Continuar");
                }
            }
        }

        private void OnEntryNovoLoginChanged(object sender, TextChangedEventArgs e)
        {
            SemanticScreenReader.Announce(EntryNovoLogin.Text);
        }

        private void OnEntryNovoLoginCompleted(object sender, EventArgs e)
        {
            SemanticScreenReader.Announce(EntryNovoLogin.Text);
        }

        private void OnEntryNovaSenhaChanged(object sender, TextChangedEventArgs e)
        {
            SemanticScreenReader.Announce(EntryNovaSenha.Text);
        }

        private void OnEntryNovaSenhaCompleted(object sender, EventArgs e)
        {
            SemanticScreenReader.Announce(EntryNovaSenha.Text);
        }

        private void OnMostrarSenhaButtonClicked(object sender, EventArgs e)
        {
            MostraSenha = !MostraSenha;
            EntryNovaSenha.IsPassword = !MostraSenha;
            HideIcon.IsVisible = MostraSenha;
            ShowIcon.IsVisible = !MostraSenha;
        }
    }
}
