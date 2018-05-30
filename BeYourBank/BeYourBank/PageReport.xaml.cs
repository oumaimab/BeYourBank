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
using System.Data;

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour PageReport.xaml
    /// </summary>
    public partial class PageReport : Page
    {
        private string idUtilisateur;

        private OleDbConnection connection = new OleDbConnection();
        DataTable ds = new DataTable("Beneficiare_table");
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
                string sql = "SELECT dateOperation as [Date Opération] , TypeOperation as [Type Opération] , count(idOperation) as [Nombre de bénéficaires] FROM Operations group by dateOperation , TypeOperation order by dateOperation desc ";
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connection);
                dataAdapter.Fill(ds);
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
            
            Chunk c1 = new Chunk("Rapport \n", FontFactory.GetFont("Candara"));
            c1.Font.Color = new iTextSharp.text.BaseColor(0, 0, 20);
            c1.Font.SetStyle(0);
            c1.Font.Size = 25;

            Chunk c2 = new Chunk("BeYourBank", FontFactory.GetFont("Candara"));
            c2.Font.Color = new iTextSharp.text.BaseColor(0, 0, 20);
            c2.Font.SetStyle(0);
            c2.Font.Size = 20;

            //auth
            Chunk auth = new Chunk("Date de création : " + DateTime.Now.ToShortDateString(), FontFactory.GetFont("Candara"));
            BaseFont bFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fontAuth = new Font(bFont, 8, 2, BaseColor.GRAY);
            iTextSharp.text.Paragraph authP = new iTextSharp.text.Paragraph(auth);
            authP.Alignment = Element.ALIGN_RIGHT;
            doc.Add(authP);

            //pdf header
            iTextSharp.text.Paragraph p1 = new iTextSharp.text.Paragraph(c1);
            p1.Alignment = Element.ALIGN_CENTER;
            p1.Add(c2);
            doc.Add(p1);

            //ajouter une ligne de séparation
            iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_MIDDLE, 0)));
            doc.Add(p);

            doc.Add(new Chunk("\n", FontFactory.GetFont("Verdana")));

            //table d'historique
            PdfPTable pdfTableHist = new PdfPTable(ds.Columns.Count);

            //table header
            BaseFont bfColumunHeader = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font ftnColumnHeader = new Font(bfColumunHeader, 10, 1, BaseColor.WHITE);
            for (int i = 0; i < ds.Columns.Count; i++)
            {
                PdfPCell cell = new PdfPCell();
                cell.BackgroundColor = BaseColor.GRAY;
                cell.AddElement(new Chunk(ds.Columns[i].ColumnName.ToUpper(), ftnColumnHeader));
                pdfTableHist.AddCell(cell);
            }
            //table cellules
            for (int i = 0; i < ds.Rows.Count; i++)
            {
                pdfTableHist.AddCell(ds.Rows[i][0].ToString().Substring(0, 10));
                for (int j = 0; j < ds.Columns.Count- 1 ; j++)
                {   
                    pdfTableHist.AddCell(ds.Rows[i][j+1].ToString());
                }
            }
            doc.Add(pdfTableHist);
            doc.Close();
            MessageBox.Show("PDF CREATED !");
        }
    }
}
