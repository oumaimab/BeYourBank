using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BeYourBank
{


    class RetourData
    {
        OleDbConnection con;
        public void textData(string filename)
        {
            con = new OleDbConnection();

            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            con.Open();
            OleDbCommand cmd = new OleDbCommand();
            OleDbCommand cmd1 = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            FileStream fs = new FileStream(filename, FileMode.Open,
                FileAccess.Read);
            string currentLine;
            using (StreamReader streamReader = new StreamReader(fs, Encoding.UTF8))
            {
                //streamReader.ReadLine();
                while ((currentLine = streamReader.ReadLine()) != null)
                {
                    if (currentLine.Length > 100)
                    {
                        currentLine.Substring(204, 25).Replace("  ", string.Empty);

                        try
                        {
                            cmd1.Connection = con;
                            cmd1.CommandText = "select [motif] from [Operations] where noCIN = '" + currentLine.Substring(133, 16) + "'; ";
                            cmd.Connection = con;
                            cmd.CommandText = "INSERT INTO [Carte] Values ( '" + currentLine.Substring(133, 16) + "' ,' " + currentLine.Substring(176, 8) + "',' 1' ,' " + currentLine.Substring(204, 25) + "');";
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Cartes affectées");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Beneficiare inéxistant");
                        }
                    }
                }
            }
        }
    }
}
