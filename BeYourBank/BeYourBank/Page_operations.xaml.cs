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
using System.Data.OleDb;
using System.IO;

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour Page_operations.xaml
    /// </summary>
    public partial class Page_operations : Page
    {
        private OleDbConnection connection = new OleDbConnection();
        public Page_operations(string idUser)
        {
            InitializeComponent();
            lbl_user.Content = idUser;
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data Source=C:\Users\MYC\Documents\PFE\BeYourBankBD.accdb";
        }

        private void btn_genererFichier_Click(object sender, RoutedEventArgs e)
        {
            string referenceConvention = null;
            string codeProduit = null;
            string numCompte = null;
            string codeCompagnie = null;
            string nomOrganisme = null;
            string raisonSociale = null;
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "select * from Convention where idUser ='" + lbl_user.Content.ToString() +"';";
                OleDbDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        referenceConvention = reader[0].ToString();
                        codeProduit = reader[1].ToString();
                        numCompte = reader[2].ToString();
                        codeCompagnie = reader[3].ToString();
                        nomOrganisme = reader[4].ToString();
                        raisonSociale = reader[5].ToString();
                    }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connection" + ex);
            }

            string dateTodayDay = null;
            string dateTodayMonth= null;
            string dateTodayYear2 = null;
            string dateTodayFormat = null;
            string index = "222";
            if (System.DateTime.Now.Day.ToString().Length == 1)
            {
                dateTodayDay = "0" + System.DateTime.Now.Day;
            }
            else if(System.DateTime.Now.Day.ToString().Length == 2)
            {
                dateTodayDay = System.DateTime.Now.Day.ToString();
            }

            if (System.DateTime.Now.Month.ToString().Length == 1)
            {
                dateTodayMonth = "0" + System.DateTime.Now.Month;
            }
            else if (System.DateTime.Now.Day.ToString().Length == 2)
            {
                dateTodayMonth = System.DateTime.Now.Month.ToString();
            }

            dateTodayFormat = dateTodayDay + dateTodayMonth + System.DateTime.Now.Year.ToString();
            dateTodayYear2 = System.DateTime.Now.Year.ToString().ElementAt(2).ToString() + System.DateTime.Now.Year.ToString().ElementAt(3).ToString();
            string fichier = "PREP_CONVENTION000000."+ dateTodayYear2 + dateTodayMonth + dateTodayDay + index +".txt";
            using (StreamWriter writer = new StreamWriter(fichier, true))
            {
                writer.Write("7FH"+codeCompagnie+"00001"+dateTodayFormat+System.DateTime.Now.Hour+ System.DateTime.Now.Minute + System.DateTime.Now.Second + index);
            }




            MessageBox.Show("Le fichier a bien été créé dans l'emplacement spécifié!", "ok", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
