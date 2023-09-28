using Npgsql;
using System;
using System.Collections.Generic;

namespace MauiAppTestesProjete2
{
    public partial class MainPage
    {
        private bool MostraSenha = false;
        private int posLogin;
        private int posLoginAdmin;
        private string Login;
        private string Senha;
        private List<string> ListaLogin = Listas.ListaLogin;
        private List<string> ListaSenha = Listas.ListaSenha;
        private List<string> ListaLoginAdmin = Listas.ListaLoginAdmin;
        private List<string> ListaSenhaAdmin = Listas.ListaSenhaAdmin;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnLogarClicked(object sender, EventArgs e)
        {
            Login = EntryLogin.Text;
            Senha = EntrySenha.Text;

            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Senha))
            {
                await DisplayAlert("Erro", "Entre com um Login/Senha Válido!", "Continuar");
                return;
            }

            if (Login == "admin" && Senha == "admin")
            {
                Listas.UsuarioAtivo = Login;
                await DisplayAlert("Bem-Vindo(a) Admin", "Você logou como admin com sucesso!", "Continuar");
                await Navigation.PushAsync(new MenuPage());
                SemanticScreenReader.Announce(LogarBtn.Text);
                return;
            }

            bool usuarioExiste = VerificarUsuarioExiste(Login);

            if (!usuarioExiste)
            {
                await DisplayAlert("Erro", "Usuário não encontrado!", "Continuar");
                return;
            }

            bool isSenhaValid = VerificarCredenciais(Login, Senha);

            if (!isSenhaValid)
            {
                await DisplayAlert("Erro", "Senha inválida!", "Continuar");
                return;
            }

            Listas.UsuarioAtivo = Login;

            // Modificação para redirecionar para a página correta do funcionário
            if (ListaLogin.Contains(Login))
            {
                await DisplayAlert("Bem-Vindo(a)", "Você logou como funcionário com sucesso!", "Continuar");
                await Navigation.PushAsync(new MenuFuncionarioPage());
                SemanticScreenReader.Announce(LogarBtn.Text);
            }
            else if (ListaLoginAdmin.Contains(Login))
            {
                await DisplayAlert("Bem-Vindo(a) Admin", "Você logou como admin com sucesso!", "Continuar");
                await Navigation.PushAsync(new MenuPage());
            }
            else
            {
                await DisplayAlert("Bem-Vindo(a)", "Você logou como funcionário com sucesso!", "Continuar");
                await Navigation.PushAsync(new MenuFuncionarioPage());
                SemanticScreenReader.Announce(LogarBtn.Text);
            }
        }



        private bool VerificarUsuarioExiste(string login)
        {
            try
            {
                using var connection = new NpgsqlConnection(AppConfig.ConnectionString);
                connection.Open();

                using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM CadastroFuncionario WHERE Nome = @login", connection);
                cmd.Parameters.AddWithValue("login", login);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count > 0;
            }
            catch (PostgresException ex)
            {
                Console.WriteLine($"Erro Postgres ao verificar o usuário: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar o usuário: {ex.Message}");
                return false;
            }
        }

        private void OnEntryLoginChanged(object sender, EventArgs e)
        {
            SemanticScreenReader.Announce(EntryLogin.Text);
        }

        private void OnEntrySenhaChanged(object sender, EventArgs e)
        {
            SemanticScreenReader.Announce(EntrySenha.Text);
        }

        private void OnEntryLoginCompleted(object sender, EventArgs e)
        {
            SemanticScreenReader.Announce(EntryLogin.Text);
        }

        private void OnEntrySenhaCompleted(object sender, EventArgs e)
        {
            SemanticScreenReader.Announce(EntrySenha.Text);
        }

        private void OnMostrarSenhaButtonClicked(object sender, EventArgs e)
        {
            if (MostraSenha == false)
            {
                MostraSenha = true;
                EntrySenha.IsPassword = false;
                HideIcon.IsVisible = false;
                ShowIcon.IsVisible = true;
            }
            else
            {
                MostraSenha = false;
                EntrySenha.IsPassword = true;
                HideIcon.IsVisible = true;
                ShowIcon.IsVisible = false;
            }
        }

        private void InserirFuncionario(string connectionString, string nomeFuncionario, string cargoFuncionario)
        {
            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                connection.Open();

                // Verificar se o nome do funcionário já existe
                using var checkCmd = new NpgsqlCommand("SELECT COUNT(*) FROM Funcionario WHERE Nome = @nome", connection);
                checkCmd.Parameters.AddWithValue("nome", nomeFuncionario);
                var count = (long)checkCmd.ExecuteScalar();

                // Se o nome já existe, não inserir novamente
                if (count > 0)
                {
                    Console.WriteLine("O nome do funcionário já existe na tabela.");
                    return;
                }

                // Preparar a consulta de inserção
                using var cmd = new NpgsqlCommand("INSERT INTO Funcionario (Nome, Cargo) VALUES (@nome, @cargo)", connection);
                cmd.Parameters.AddWithValue("nome", nomeFuncionario);
                cmd.Parameters.AddWithValue("cargo", cargoFuncionario);

                // Executar a consulta de inserção
                cmd.ExecuteNonQuery();
            }
            catch (PostgresException ex)
            {
                Console.WriteLine($"Erro Postgres ao inserir o nome do funcionário: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inserir o nome do funcionário: {ex.Message}");
            }
        }

        private bool VerificarCredenciais(string login, string senha)
        {
            try
            {
                using var connection = new NpgsqlConnection(AppConfig.ConnectionString);
                connection.Open();

                using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM CadastroFuncionario WHERE Nome = @login AND Senha = @senha", connection);
                cmd.Parameters.AddWithValue("login", login);
                cmd.Parameters.AddWithValue("senha", senha);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count > 0;
            }
            catch (PostgresException ex)
            {
                Console.WriteLine($"Erro Postgres ao verificar as credenciais: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar as credenciais: {ex.Message}");
                return false;
            }
        }
    }
}



