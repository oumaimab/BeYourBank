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
           // string motif = "";

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
          List<string> lstMotif = new List<string>() ;
            
       
            using (StreamReader streamReader = new StreamReader(fs, Encoding.UTF8))
            {
                //streamReader.ReadLine();
                //int i = 0;
                while ((currentLine = streamReader.ReadLine()) != null)
                { 
                    if (currentLine.Length > 100)
                    {
                        currentLine.Substring(204, 25).Replace("  ", string.Empty);

                        try
                        {
                            cmd1.Connection = con;
                            cmd1.CommandText = "select [motif] from [Operations] where idBeneficiaire = '" + currentLine.Substring(176, 8) + "' and [typeOperation]= 'Creation';";
                             OleDbDataReader reader= cmd1.ExecuteReader();
                            while (reader.Read())
                            {
                                //motif = reader[0].ToString();
                                lstMotif.Add(String.Format("{0}", reader[0]));
                            }  //MessageBox.Show(motif);
                             reader.Close();
                            cmd.Connection = con;
                            cmd2.Connection = con;
                            cmd2.CommandText = "update [Operations] set [numCarte] =' " + currentLine.Substring(133, 16) + "' where idBeneficiaire = '" + currentLine.Substring(176, 8) + "and[typeOperation] = 'Creation' ";
                            cmd2.ExecuteNonQuery();
                            //for ( int i =0; i < lstMotif.Count(); i++)
                            //{


                            cmd.CommandText = "INSERT INTO [Carte] Values ( '" + currentLine.Substring(133, 16) + "' ,' " + currentLine.Substring(176, 8) + "',' "+lstMotif[0].ToString() + "' ,' " + currentLine.Substring(204, 25) + "');";

                            // }
                           // i++;
                                cmd.ExecuteNonQuery();
                              
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Beneficiare inéxistant"+ex );
                        }
                       
                    }
                }
                
                con.Close();
                MessageBox.Show("Cartes affectées");

            }
        }
    }
}
