using Npgsql;
using System;
using System.Collections.Generic;

namespace MauiAppTestesProjete2
{
    public class Listas
    {
        public static List<string> ListaLogin = new List<string>();
        public static List<string> ListaSenha = new List<string>();
        public static List<string> ListaLoginAdmin = new List<string>() { "admin" };
        public static List<string> ListaSenhaAdmin = new List<string>() { "admin" };
        public static string UsuarioAtivo = "";

        public static void CarregarDados()
        {
            CarregarDadosCadastroUsuario();  // Carrega dados da tabela cadastrousuario
        }

        private static void CarregarDadosCadastroUsuario()
        {
            ListaLogin.Clear();
            ListaSenha.Clear();

            try
            {
                using var connection = new NpgsqlConnection(AppConfig.ConnectionString);
                connection.Open();

                using var cmd = new NpgsqlCommand("SELECT Nome, Senha FROM cadastrousuario", connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string nome = reader["Nome"].ToString();
                    string senha = reader["Senha"].ToString();

                    ListaLogin.Add(nome);
                    ListaSenha.Add(senha);
                }
            }
            catch (PostgresException ex)
            {
                Console.WriteLine($"Erro Postgres ao carregar os dados: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar os dados: {ex.Message}");
            }
        }

        public static void ImprimirDados()
        {
            Console.WriteLine("Dados carregados:");
            for (int i = 0; i < ListaLogin.Count; i++)
            {
                Console.WriteLine($"Usuário: {ListaLogin[i]}, Senha: {ListaSenha[i]}");
            }
        }
    }
}
