using Npgsql;
using System;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MauiAppTestesProjete2
{
    public partial class QrCodePage : ContentPage
    {
        private Stopwatch stopWatch;
        private Timer timer;
        private int funcionarioId = 1; // Substitua pelo ID real do funcionário
        private string produtoId; // Alterado para string
        private TimeSpan tempoDecorrido; // Adicionando a variável tempoDecorrido
        string UsuarioAtivo = Listas.UsuarioAtivo;
        private string ConnectionString => "Server=localhost;Port=5432;User Id=postgres;Password=123;Database=GPI;";

        public QrCodePage()
        {
            InitializeComponent();
        }

        private void cameraView_CamerasLoaded(object sender, EventArgs e)
        {
            if (cameraView.Cameras.Count > 0)
            {
                cameraView.Camera = cameraView.Cameras.First();
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await cameraView.StopCameraAsync();
                    await cameraView.StartCameraAsync();
                });
            }
        }

        private void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                string scannedText = $"{args.Result[0].BarcodeFormat}: {args.Result[0].Text}";
                barcodeResult.Text = scannedText;

                // Atribuir o texto lido no código QR ao produtoId
                produtoId = args.Result[0].Text;

                // Iniciar o timer para atualizar a UI a cada segundo
                timer = new Timer(UpdateUI, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));

                stopWatch = new Stopwatch();
                stopWatch.Start();

                TempoReal.IsVisible = true;
                TempoGasto.IsVisible = false;
                FinishButton.IsVisible = true;
                ErrorButton.IsVisible = true;
            });
        }

        private void UpdateUI(object state)
        {
            if (stopWatch != null && stopWatch.IsRunning)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    TimeSpan elapsedTime = stopWatch.Elapsed;
                    TempoReal.Text = $"{elapsedTime.Hours:00}:{elapsedTime.Minutes:00}:{elapsedTime.Seconds:00}";
                });
            }
        }

        private async void OnFinishButtonClicked(object sender, EventArgs e)
        {
            if (stopWatch != null && stopWatch.IsRunning)
            {
                stopWatch.Stop();
                timer.Dispose();

                tempoDecorrido = stopWatch.Elapsed;

                TempoReal.IsVisible = false;
                TempoGasto.IsVisible = true;
                FinishButton.IsVisible = false;
                ErrorButton.IsVisible = false;

                TempoGasto.Text = $"{tempoDecorrido.Hours:00}:{tempoDecorrido.Minutes:00}:{tempoDecorrido.Seconds:00}";

                Console.WriteLine("Tentando gravar registro de tempo no banco de dados...");
                // Replace the call to GravarRegistroDeTempo with GravarRegistroCronometro
                await GravarRegistroCronometro(tempoDecorrido, produtoId);

                // Mostrar o tempo gasto em um alerta
                await DisplayAlert("Tempo Gasto", $"Tempo gasto: {tempoDecorrido.Hours:00}:{tempoDecorrido.Minutes:00}:{tempoDecorrido.Seconds:00}", "OK");
            }
            else
            {
                Console.WriteLine("O cronômetro não está em execução.");
            }
        }


        private async void OnErrorButtonClicked(object sender, EventArgs e)
        {
            if (stopWatch != null && stopWatch.IsRunning)
            {
                stopWatch.Stop();
                timer.Dispose();

                tempoDecorrido = stopWatch.Elapsed;

                var errorDescription = await DisplayPromptAsync("Defeito", "Descreva o defeito:");

                Console.WriteLine("Tentando gravar registro de erro no banco de dados...");
                await GravarRegistroDeErro(errorDescription);
            }
            else
            {
                Console.WriteLine("O cronômetro não está em execução.");
            }
        }

        private async Task GravarRegistroCronometro(TimeSpan tempoDecorrido, string produtoId)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                string query = "INSERT INTO RegistroCronometro (funcionario, tempo_cronometro, produto_id) VALUES (@funcionario, @tempoCronometro, @produtoId)";
                using var cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@funcionario", UsuarioAtivo);
                cmd.Parameters.AddWithValue("@tempoCronometro", tempoDecorrido);
                cmd.Parameters.AddWithValue("@produtoId", produtoId);

                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                    Console.WriteLine("Registro do cronômetro gravado com sucesso!");
                else
                    Console.WriteLine("Nenhum registro do cronômetro foi gravado.");

                connection.Close();
            }
            catch (PostgresException ex)
            {
                Console.WriteLine($"Erro Postgres ao gravar o registro do cronômetro: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao gravar o registro do cronômetro: {ex.Message}");
            }
        }

        private async Task GravarRegistroDeErro(string errorDescription)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                string query = "INSERT INTO RegistroErro (funcionario, produto_id, erro) VALUES (@funcionario, @produtoId, @erro)";
                using var cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@funcionario", UsuarioAtivo);
                cmd.Parameters.AddWithValue("@produtoId", produtoId);
                cmd.Parameters.AddWithValue("@erro", errorDescription);

                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                    Console.WriteLine("Registro de erro gravado com sucesso!");
                else
                    Console.WriteLine("Nenhum registro de erro foi gravado.");

                connection.Close();
            }
            catch (PostgresException ex)
            {
                Console.WriteLine($"Erro Postgres ao gravar o registro de erro: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao gravar o registro de erro: {ex.Message}");
            }
        }

    }
}
