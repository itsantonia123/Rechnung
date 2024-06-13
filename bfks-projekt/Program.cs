using System.Numerics;
using Npgsql;

class Program
{
    const string connectionString = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=postgres;";

    static void Main()
    {
        //InitTables();
        //InsertSampleData();
        //Reset();
        ShowAuftragHtml(201002001);
    }

    static void Reset()
    {
        RunFromFile("sql-cmds/reset.sql");
    }

    static void InitTables()
    {
        RunFromFile("sql-cmds/init.sql");
    }

    static void InsertSampleData()
    {
        RunFromFile("sql-cmds/sample.sql");
    }
    static Dictionary<string, Dictionary<string, object>> GetPositionen(int auftragsNr)
    {
        var conn = new NpgsqlConnection(connectionString);
        conn.Open();

        var cmd = new NpgsqlCommand("SELECT * FROM position WHERE " + auftragsNr + " = @auftragsnr", conn);
        cmd.Parameters.AddWithValue("auftragsnr", auftragsNr);

        var dict = new Dictionary<string, Dictionary<string, object>>();

        var reader = cmd.ExecuteReader();
        var enumerator = reader.GetEnumerator();
        while (enumerator.MoveNext())
        {
            //reader.Read();
            var subdict = new Dictionary<string, object>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var name = reader.GetName(i);
                var value = reader.GetValue(i);

                subdict.Add(name, value);
            }
#pragma warning disable CS8604 // Possible null reference argument.
            dict.Add(reader["positionnr"].ToString(), subdict);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        conn.Close();

        return dict;
    }

    static Dictionary<string, object> GetDictionaryFromTable(string table, string pkFieldName, int pkValue)
    {
        var conn = new NpgsqlConnection(connectionString);
        conn.Open();

        var cmd = new NpgsqlCommand("SELECT * FROM " + table + " WHERE " + pkFieldName + " = @pkValue", conn);
        cmd.Parameters.AddWithValue("pkValue", pkValue);

        var reader = cmd.ExecuteReader();
        reader.Read();

        var dict = new Dictionary<string, object>();
        for (int i = 0; i < reader.FieldCount; i++)
        {
            var name = reader.GetName(i);
            var value = reader.GetValue(i);

            dict.Add(name, value);
        }

        conn.Close();

        return dict;
    }

    static double GetEinzelpreis(int artikelNr)
    {
        var conn = new NpgsqlConnection(connectionString);
        conn.Open();

        var cmd = new NpgsqlCommand("SELECT einzelpreis FROM artikel WHERE " + artikelNr + " = @artikelnr", conn);
        cmd.Parameters.AddWithValue("artikelnr", artikelNr);

        var reader = cmd.ExecuteReader();
        reader.Read();
        
        double artikelEinzelpreis = Decimal.ToDouble((Decimal) reader["einzelpreis"]);

        conn.Close();

        return artikelEinzelpreis;
    }
    static String GetArtikelname(int artikelNr)
    {
        var conn = new NpgsqlConnection(connectionString);
        conn.Open();

        var cmd = new NpgsqlCommand("SELECT bezeichnung FROM artikel WHERE " + artikelNr + " = @artikelnr", conn);
        cmd.Parameters.AddWithValue("artikelnr", artikelNr);

        var reader = cmd.ExecuteReader();
        
        String artikelName = reader.GetName(0);

        conn.Close();

        return artikelName;
    }

    static Dictionary<string, Dictionary<string, object>> GetVerkaeufer(int verkaeuferId)
    {
        var conn = new NpgsqlConnection(connectionString);
        conn.Open();

        var cmd = new NpgsqlCommand("SELECT * FROM position WHERE " + verkaeuferId + " = @verkaeuferid", conn);
        cmd.Parameters.AddWithValue("verkaeuferid", verkaeuferId);

        var dict = new Dictionary<string, Dictionary<string, object>>();

        var reader = cmd.ExecuteReader();
        var enumerator = reader.GetEnumerator();
        while (enumerator.MoveNext())
        {
            //reader.Read();
            var subdict = new Dictionary<string, object>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var name = reader.GetName(i);
                var value = reader.GetValue(i);

                subdict.Add(name, value);
            }
#pragma warning disable CS8604 // Possible null reference argument.
            //dict.Add(reader["verkaeuferid"].ToString(), subdict);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        conn.Close();

        return dict;
    }
/*
    static Dictionary<string, object> GetKontoNr(string table, string pkFieldName, int pkValue)
    {
        var conn = new NpgsqlConnection(connectionString);
        conn.Open();

        var cmd = new NpgsqlCommand("SELECT * FROM " + table + " WHERE " + pkFieldName + " = @pkValue", conn);
        cmd.Parameters.AddWithValue("pkValue", pkValue);

        var reader = cmd.ExecuteReader();
        reader.Read();

        var dict = new Dictionary<string, object>();
        for (int i = 0; i < reader.FieldCount; i++)
        {
            var name = reader.GetName(i);
            var value = reader.GetValue(i);

            dict.Add(name, value);
        }

        conn.Close();

        return dict;
    }
    */

    static void RunFromFile(string filePath)
    {
        var conn = new NpgsqlConnection(connectionString);
        conn.Open();

        string sqlScript = System.IO.File.ReadAllText(filePath);
        using (var cmd = new NpgsqlCommand(sqlScript, conn))
        {
            cmd.ExecuteNonQuery();
            Console.WriteLine("Script " + filePath + " executed successfully!");
        }

        conn.Close();
    }


    static void ShowAuftragHtml(int auftragsNr)
    {
        var conn = new NpgsqlConnection(connectionString);
        conn.Open();

        var rechnung = GetDictionaryFromTable("rechnung", "auftragsnr", auftragsNr);
        
        // CSS styles
        string style = @"
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
        }
        .header, .footer {
            text-align: left;
            margin-bottom: 20px;
        }
        .header img {
            float: left;
            margin-right: 20px;
        }
        .header h1 {
            margin: 0;
        }
        .header .details {
            float: right;
            text-align: right;
        }
        .clear {
            clear: both;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }
        table, th, td {
            border: 1px solid black;
        }
        th, td {
            padding: 8px;
            text-align: left;
        }
        .no-border {
            border: none;
        }
    </style>";

        // Start building HTML
        string html = $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <title>Rechnung</title>
        {style}
    </head>
    <body>";

        // Build header
        html += @"
        <div class=""header"">
            <img src=""logo.png"" alt=""Company Logo"" width=""100"">
            <div class=""details"">
                <p>Winkler KG, Schulstr.11, 12345 Neuheim</p>
                <p>An: Manfred Heller<br>
                Dinkelstrasse 39<br>
                34056 Mauerbach</p>
            </div>
            <div class=""clear""></div>
            <h1>Rechnung</h1>
        </div>";

        // Build main content table
        html += @"
        <table>
            <tr>
                <td class=""no-border"">Auftrags Nr. <strong>" + auftragsNr + @"</strong></td>
                <td class=""no-border"">Kunden Nr. <strong>" + rechnung["kundennr"] + @"</strong></td>
                <td class=""no-border"">Datum <strong>" + rechnung["datum"] + @"</strong></td>
                <td class=""no-border"">Bearbeiter <strong>" + rechnung["bearbeiter"] + @"</strong></td>
            </tr>
        </table>";

        // Build position table
        var positionen = GetPositionen(auftragsNr);
        //var artikelNr = positionen["artikelnr"];
        html += @"
        <table>
            <thead>
                <tr>
                    <th>Pos. Nr.</th>
                    <th>Artikel Nr.</th>
                    <th>Artikel</th>
                    <th>Menge</th>
                    <th>Einzelpreis</th>
                    <th>Gesamtpreis</th>
                </tr>
            </thead>
            <tbody>";


        foreach (var positionKvp in positionen)
        {
            var positionKey = positionKvp.Key;
            int artikelnummer = (int)positionKvp.Value["artikelnr"];
            var menge = positionKvp.Value["menge"];
            var gesamtpreis = positionKvp.Value["gesamtpreis"];
            String artikelname = GetArtikelname(artikelnummer);
            double einzelpreis = GetEinzelpreis(artikelnummer);

            html += $@"
                <tr>
                    <td>{positionKey}</td>
                    <td>{artikelnummer}</td>
                    <td>{artikelname}</td>
                    <td>{menge}</td>
                    <td>{einzelpreis} €</td>
                    <td>{gesamtpreis} €</td>
                </tr>";
        }


        String mwst = "19%";
        html += @"
            </tbody>
            <tfoot>
                <tr>
                    <td colspan=""4"" class=""no-border""></td>
                    <td>Netto-Betrag</td>
                    <td>" + rechnung["nettowert"] + @" €</td>
                </tr>
                <tr>
                    <td colspan=""4"" class=""no-border""></td>
                    <td>19% MwSt</td>
                    <td>" + mwst + @" €</td>
                </tr>
                <tr>
                    <td colspan=""4"" class=""no-border""></td>
                    <td><strong>Gesamt-Betrag</strong></td>
                    <td><strong>" + rechnung["bruttowert"] + @" €</strong></td>
                </tr>
            </tfoot>
        </table>";

        // Remaining text
        html += @"
        <p>Wir danken für Ihren Einkauf</p>
        <p>Zahlungsvermerk: " + rechnung["zahlungsvermerkung"] + @"<br>Zahlbar bis " + rechnung["fälligkeitsdatum"] + @"</p>";

        // Build footer
        int verkaeuferId = (int)rechnung["verkaeuferid"];
        var verkaeufer = GetVerkaeufer(verkaeuferId);
        //char kontoNr = GetKontoNr(verkaeuferId);
        
        foreach (var verkaeuferKvp in verkaeufer)
        {
            var verkaeuferKey = verkaeuferKvp.Key;
            int artikelnummer = (int)verkaeuferKvp.Value["artikelnr"];
            var menge = verkaeuferKvp.Value["menge"];
            var gesamtpreis = verkaeuferKvp.Value["gesamtpreis"];
            String artikelname = GetArtikelname(artikelnummer);
            double einzelpreis = GetEinzelpreis(artikelnummer);
            
        html += @"
        <div class=""footer"">
            <p>Bankname: Deutsche Bank<br>Konto: " + verkaeufer["kontonr"] + @"<br>BLZ: " + verkaeufer["blz"] + @"<br>Steuernummer: " + verkaeufer["steuernummer"] + @"<br>US-ID-Nr: " + verkaeufer["ustidnr"] + @"<br>Amtsgericht " + verkaeufer["amtsgericht"] + @"<br>HBR " + verkaeufer["hbr"] + @"<br>Firmensitz: Neuheim<br>" + verkaeufer["straße"] + @", 75175 Neuheim<br>Tel.: " + verkaeufer["telefon"] + @"<br>Fax: " + verkaeufer["fax"] + @"<br>Geschäftsführung: " + verkaeufer["geschäftsführung"] + @"</p>
        </div>";
        }        

        html += @"
    </body>
    </html>";

        System.IO.File.WriteAllText("auftrag.html", html);

        //conn.Close();
    }
}