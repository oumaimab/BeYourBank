using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data.OleDb;
using System.Configuration;

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour PageReport.xaml
    /// </summary>
    public partial class PageReport : Page
    {
        private string idUtilisateur;
        private OleDbConnection connection = new OleDbConnection();
        public PageReport(string idUser)
        {
            InitializeComponent();
            idUtilisateur = idUser;
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string nomOrganisme;
            string referenceConvention;
            string codeCompagnie;
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Convention WHERE refConvention= (SELECT idConvention FROM Utilisateurs WHERE noCINUser='" + idUtilisateur + "');";
                OleDbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    referenceConvention = reader[0].ToString();
                    codeCompagnie = reader[3].ToString();
                    nomOrganisme = reader[4].ToString();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion" + ex);
            }
            Document doc = new Document(iTextSharp.text.PageSize.A4, 20, 20, 40, 30);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream("Rapport_BeYourBank.pdf", FileMode.Create));
            doc.Open();

            Chunk c1 = new Chunk("Rapport", FontFactory.GetFont("Helvetica"));
            c1.Font.Color = new iTextSharp.text.BaseColor(0, 0, 20);
            c1.Font.SetStyle(0);
            c1.Font.Size = 20;

            Chunk c2 = new Chunk("--- BE YOUR BANK ---", FontFactory.GetFont("Verdana"));
            c2.Font.Color = new iTextSharp.text.BaseColor(0, 0, 20);
            c2.Font.SetStyle(0);
            c2.Font.Size = 15;

            Chunk c3 = new Chunk("--- BE YOUR BANK ---", FontFactory.GetFont("Verdana"));
            c3.Font.Color = new iTextSharp.text.BaseColor(0, 0, 20);
            c3.Font.SetStyle(0);
            c3.Font.Size = 15;

            iTextSharp.text.Paragraph p1 = new iTextSharp.text.Paragraph("Hello ! premier paragraphe");
            doc.Add(p1);
            doc.Close();
            MessageBox.Show("PDF CREATED !");
        }
    }
}
