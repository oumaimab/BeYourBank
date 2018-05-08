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
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Configuration;

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour conventionWindow.xaml
    /// </summary>
    public partial class conventionWindow : Window
    {
        private OleDbConnection connection = new OleDbConnection();
        public conventionWindow(String idUser)
        {
            InitializeComponent();
            lbl_idUser.Content = idUser;
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
        }
        private void btn_annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void btn_Enregistrer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "INSERT INTO Convention ( refConvention, codeProduit, numCompte, codeCompagnie, nomOrganisme, raisonSociale, idUser ) VALUES ('" + txtBox_refConvention.Text + "', '" + txtBox_codeProduit.Text + "','" + txtBox_ribCompte.Text + "'," + txtBox_codeCompanie.Text  + " ,'" + txtBox_nomOrganisme.Text + "','" + txtBox_raisonSociale.Text + "','" + lbl_idUser.Content + "');";
                command.ExecuteNonQuery();
                MessageBox.Show("Informations enregistrées");
                WelcomeWindow welcome = new WelcomeWindow(lbl_idUser.Content.ToString());
                this.Hide();
                welcome.lbl_utilisateur.Content = lbl_nomUser.Content;
                welcome.Show();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion" + ex);
            }

        }

        private void txtBox_codeCompanie_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void txtBox_ribCompte_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
    }
}
