using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            OleDbCommand cmd2 = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            FileStream fs = new FileStream(filename, FileMode.Open,
                FileAccess.Read);
            string currentLine;
            string indexF = null;
            using (StreamReader streamReader = new StreamReader(fs, Encoding.UTF8))
            {
                //streamReader.ReadLine();
                while ((currentLine = streamReader.ReadLine()) != null)
                {
                    if (currentLine.Length == 25)
                    {
                        indexF = currentLine.Substring(22, 3);
                        MessageBox.Show("-" + indexF + "-");
                    }
                        
                    if (currentLine.Length > 100)
                    {
                        currentLine.Substring(204, 25).Replace("  ", string.Empty);

                        try
                        {
                            cmd.Connection = con;
                            cmd1.Connection = con;
                            cmd2.Connection = con;
                            string motif = null;
                            string CIN = Regex.Replace(currentLine.Substring(176, 8), @"\s", "");
                            cmd1.CommandText = "select [motif] from [Operations] WHERE (((Operations.[idBeneficiaire]) ='" + CIN + "') AND [typeOperation] = 'Creation' AND ((Mid([idFichier],7,3))='"+ indexF +"'));";
                            OleDbDataReader reader = cmd1.ExecuteReader();
                            while (reader.Read())
                            {
                                motif = reader[0].ToString();
                            }  
                            reader.Close();
                            cmd.CommandText = "INSERT INTO [Carte] Values ( '"+ currentLine.Substring(133, 16)+"' ,'"+CIN+"','"+motif+"' ,'"+ currentLine.Substring(204, 25) + "');";
                            cmd2.CommandText = "update [Operations] set [numCarte] ='"+ currentLine.Substring(133, 16)+"' where idBeneficiaire ='"+CIN+"' and [typeOperation] ='Creation';";
                            cmd.ExecuteNonQuery();
                            cmd2.ExecuteNonQuery();
                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Beneficiare inéxistant" + ex);
                        }
                    }
                }
                MessageBox.Show("Cartes affectées");
            }
        }
    }
}
