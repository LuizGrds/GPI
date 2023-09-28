using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MauiAppTestesProjete2
{
    public static class AppConfig
{
    public static string ConnectionString => "Server=localhost;Port=5432;User Id=postgres;Password=123;Database=GPI;";
}

}
