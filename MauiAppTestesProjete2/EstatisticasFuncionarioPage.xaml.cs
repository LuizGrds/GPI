using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using Microsoft.Maui.Controls;
using Microcharts.Maui;

namespace MauiAppTestesProjete2
{
    public partial class EstatisticasFuncionarioPage : ContentPage
    {
        public EstatisticasFuncionarioPage()
        {
            InitializeComponent();

            // Chame um método para carregar os dados do banco e criar o gráfico
            LoadDataAndCreateChart();
        }

        private async void LoadDataAndCreateChart()
        {
            // Obter os dados do banco de dados
            var entries = await ObterDadosDoBanco();

            var chart = new LineChart
            {
                Entries = entries,
                LabelTextSize = 20
            };

            var chartView = new ChartView
            {
                Chart = chart,
                WidthRequest = 300,
                HeightRequest = 300
            };

            Content = chartView;
        }

        private async Task<List<ChartEntry>> ObterDadosDoBanco()
        {
            var entries = new List<ChartEntry>();

            try
            {
                using var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=123;Database=GPI");
                await connection.OpenAsync();

                using var cmd = new NpgsqlCommand("SELECT produto_id, tempo_cronometro FROM RegistroCronometro", connection);

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var produtoId = reader["produto_id"].ToString();
                    var tempoCronometro = Convert.ToSingle(reader["tempo_cronometro"].ToString());

                    var entry = new ChartEntry(tempoCronometro)
                    {
                        Label = produtoId,
                        ValueLabel = $"{tempoCronometro} min",
                        Color = GetRandomColor()
                    };

                    entries.Add(entry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter os registros do banco: {ex.Message}");
            }

            return entries;
        }

        private SKColor GetRandomColor()
        {
            Random random = new Random();
            return new SKColor((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
        }
    }
}
