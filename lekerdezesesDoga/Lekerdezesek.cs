using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace lekerdezesesDoga
{
    internal class Lekerdezesek
    {
        private string connectionString;

        public Lekerdezesek()
        {
            this.connectionString = "server=localhost;database=jozsefattila;user=root;password='';";
            Lekerdezes();
        }

        private void Lekerdezes()
        {
            using (var connection = new MySqlConnection(connectionString)) 
            {
                try
                {
                    connection.Open();
                    string q = "SELECT szemely.nev FROM `szemely` INNER JOIN kituntetes ON szemely.az = kituntetes.szemaz WHERE kituntetes.ev = 2016;";
                    Kiiratas(connection, q, "2016-s emberkék");
                    string q1 = "SELECT szemely.nev FROM `szemely` INNER JOIN foglalkozas ON szemely.az = foglalkozas.szemaz WHERE foglalkozas.fognev LIKE(\"%kritikus%\");";
                    Kiiratas(connection, q1, "kritikust szót tartalmazó foglalkozású embereke");
                    string q2 = "SELECT szemely.nev, COUNT(foglalkozas.szemaz) as db FROM `foglalkozas` INNER JOIN szemely ON foglalkozas.szemaz = szemely.az GROUP BY foglalkozas.szemaz HAVING db >= 3;";
                    Kiiratas(connection, q2, "3x kituntetett emberek");
                    string q3 = "SELECT szemely.nev,foglalkozas.fognev, COUNT(foglalkozas.fognev) as db FROM `foglalkozas` INNER JOIN szemely ON foglalkozas.szemaz = szemely.az GROUP BY foglalkozas.szemaz ORDER BY db DESC;";
                    Kiiratas(connection, q3, "leggyakoribb foglalkozású személyek");
                    string q4 = "SELECT szemely.nev, kituntetes.ev FROM szemely INNER JOIN kituntetes ON szemely.az = kituntetes.szemaz WHERE kituntetes.ev IN (SELECT kituntetes.ev FROM `kituntetes` INNER JOIN szemely ON kituntetes.szemaz = szemely.az WHERE szemely.nev = \"Bertha Bulcsu\");";
                    Kiiratas(connection, q4, "Bertha Bulcsuval együtt kitüntetett személyek");
                    string q5 = "SELECT DISTINCT foglalkozas.fognev, COUNT(foglalkozas.fognev) as foglalkozasokSzama FROM `foglalkozas`  INNER JOIN szemely ON foglalkozas.szemaz = szemely.az GROUP BY foglalkozas.fognev HAVING foglalkozasokSzama < 3  \r\nORDER BY `foglalkozasokSzama` DESC;";
                    Kiiratas(connection, q5, "3-nál kevesebb darabszámú foglalkozás");


                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }

        }

        private void Kiiratas(MySqlConnection connection, string querry, string feladatCim)
        {
            
            using (var command = new MySqlCommand(querry, connection))
            {
                using(var reader = command.ExecuteReader())
                {
                    Console.WriteLine(feladatCim + ": ");
                    Console.WriteLine("---------------------------------------- \t");
                    DataTable schema = reader.GetSchemaTable();
                    foreach (DataRow row in schema.Rows)
                    {
                        Console.Write($"{row["ColumnName"], 10}");
                    }
                    Console.WriteLine();
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader[i], 15} \t");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
