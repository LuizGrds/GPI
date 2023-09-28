using Microcharts;
using SkiaSharp;

namespace MauiAppTestesProjete2;

public partial class EstatisticasPage : ContentPage
{

    ChartEntry[] entries = new[]
        {
            new ChartEntry(5)
            {
                Label = "Fernando",
                ValueLabel = "5 p/h",
                Color = SKColor.Parse("#2c3e50")
            },
            new ChartEntry(3)
            {
                Label = "Teles",
                ValueLabel = "3 p/h",
                Color = SKColor.Parse("#77d065")
            },
            new ChartEntry(7)
            {
                Label = "Luiz",
                ValueLabel = "7 p/h",
                Color = SKColor.Parse("#b455b6")
            },
            new ChartEntry(10)
            {
                Label = "João",
                ValueLabel = "10 p/h",
                Color = SKColor.Parse("#3498db")
            }
        };

    ChartEntry[] entriesFernando = new[]
        {
            new ChartEntry(10)
            {
                Label = "Produto 1",
                ValueLabel = "10 min",
                Color = SKColor.Parse("#2c3e50")
            },
            new ChartEntry(12)
            {
                Label = "Produto 2",
                ValueLabel = "12 min",
                Color = SKColor.Parse("#77d065")
            },
            new ChartEntry(14)
            {
                Label = "Produto 3",
                ValueLabel = "14 min",
                Color = SKColor.Parse("#b455b6")
            },
            new ChartEntry(12)
            {
                Label = "Produto 4",
                ValueLabel = "12 min",
                Color = SKColor.Parse("#3498db")
            }
        };

    ChartEntry[] entriesTeles = new[]
        {
            new ChartEntry(20)
            {
                Label = "Produto 1",
                ValueLabel = "20 min",
                Color = SKColor.Parse("#2c3e50")
            },
            new ChartEntry(25)
            {
                Label = "Produto 2",
                ValueLabel = "25 min",
                Color = SKColor.Parse("#77d065")
            },
            new ChartEntry(15)
            {
                Label = "Produto 3",
                ValueLabel = "15 min",
                Color = SKColor.Parse("#b455b6")
            },
            new ChartEntry(20)
            {
                Label = "Produto 4",
                ValueLabel = "20 min",
                Color = SKColor.Parse("#3498db")
            }
        };

    ChartEntry[] entriesLuiz = new[]
        {
            new ChartEntry(8)
            {
                Label = "Produto 1",
                ValueLabel = "8 min",
                Color = SKColor.Parse("#2c3e50")
            },
            new ChartEntry(9)
            {
                Label = "Produto 2",
                ValueLabel = "9 min",
                Color = SKColor.Parse("#77d065")
            },
            new ChartEntry(7)
            {
                Label = "Produto 3",
                ValueLabel = "7 min",
                Color = SKColor.Parse("#b455b6")
            },
            new ChartEntry(10)
            {
                Label = "Produto 4",
                ValueLabel = "10 min",
                Color = SKColor.Parse("#3498db")
            }
        };

    ChartEntry[] entriesJoao = new[]
        {
            new ChartEntry(5)
            {
                Label = "Produto 1",
                ValueLabel = "5 min",
                Color = SKColor.Parse("#2c3e50")
            },
            new ChartEntry(7)
            {
                Label = "Produto 2",
                ValueLabel = "7 min",
                Color = SKColor.Parse("#77d065")
            },
            new ChartEntry(6)
            {
                Label = "Produto 3",
                ValueLabel = "6 min",
                Color = SKColor.Parse("#b455b6")
            },
            new ChartEntry(6)
            {
                Label = "Produto 4",
                ValueLabel = "6 min",
                Color = SKColor.Parse("#3498db")
            }
        };

    public EstatisticasPage()
	{
		InitializeComponent();

        chartView.Chart = new BarChart
        {
            Entries = entries,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal, 
            LabelTextSize = 20
        };

        chartViewFernando.Chart = new LineChart
        {
            Entries = entriesFernando,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
            LabelTextSize = 20
        };

        chartViewTeles.Chart = new LineChart
        {
            Entries = entriesTeles,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
            LabelTextSize = 20
        };

        chartViewLuiz.Chart = new LineChart
        {
            Entries = entriesLuiz,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
            LabelTextSize = 20
        };

        chartViewJoao.Chart = new LineChart
        {
            Entries = entriesJoao,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
            LabelTextSize = 20
        };
    }
}