using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour PageConventionUser.xaml
    /// </summary>
    public partial class PageConventionUser : Page
    {
        private OleDbConnection connection = new OleDbConnection();
        private int newConv = 0;
        public PageConventionUser(string idUser)
        {
            InitializeComponent();
            lbl_idUser.Content = idUser;
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
        }

        private void page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                OleDbCommand command1 = new OleDbCommand();
                command.Connection = connection;
                command1.Connection = connection;
                command.CommandText = "SELECT idConvention FROM Utilisateurs WHERE noCINUser='" + lbl_idUser.Content.ToString() + "'";
                command1.CommandText = "SELECT * FROM Convention WHERE refConvention= (SELECT idConvention FROM Utilisateurs WHERE noCINUser='" + lbl_idUser.Content.ToString() + "');";
                OleDbDataReader reader = command.ExecuteReader();
                OleDbDataReader reader1 = command1.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrWhiteSpace(reader[0].ToString()))
                    {
                        newConv = 1;
                    }
                }
                reader.Close();
                while (reader1.Read())
                {
                    txtBox_refConvention.Text = reader1[0].ToString();
                    txtBox_codeProduit.Text = reader1[1].ToString();
                    txtBox_ribCompte.Text = reader1[2].ToString();
                    txtBox_codeCompanie.Text = reader1[3].ToString();
                    txtBox_nomOrganisme.Text = reader1[4].ToString();
                    txtBox_raisonSociale.Text = reader1[5].ToString();
                }
                reader1.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion" + ex);
            }
        }
        private void setPage()
        {
            btn_edit.IsEnabled = true;
            btn_Enregistrer.Visibility = Visibility.Hidden;
            btn_annuler.Visibility = Visibility.Hidden;

            txtBox_refConvention.IsReadOnly = true;
            txtBox_codeProduit.IsReadOnly = true;
            txtBox_ribCompte.IsReadOnly = true;
            txtBox_codeCompanie.IsReadOnly = true;
            txtBox_nomOrganisme.IsReadOnly = true;
            txtBox_raisonSociale.IsReadOnly = true;

            txtBox_refConvention.Background = Brushes.LightGray;
            txtBox_codeProduit.Background = Brushes.LightGray;
            txtBox_ribCompte.Background = Brushes.LightGray;
            txtBox_codeCompanie.Background = Brushes.LightGray;
            txtBox_nomOrganisme.Background = Brushes.LightGray;
            txtBox_raisonSociale.Background = Brushes.LightGray;

            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Convention WHERE refConvention= (SELECT idConvention FROM Utilisateurs WHERE noCINUser='" + lbl_idUser.Content.ToString() + "');";
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    txtBox_refConvention.Text = reader[0].ToString();
                    txtBox_codeProduit.Text = reader[1].ToString();
                    txtBox_ribCompte.Text = reader[2].ToString();
                    txtBox_codeCompanie.Text = reader[3].ToString();
                    txtBox_nomOrganisme.Text = reader[4].ToString();
                    txtBox_raisonSociale.Text = reader[5].ToString();
                }
                reader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion" + ex);
            }   

        }
        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            btn_edit.IsEnabled = false;
            btn_Enregistrer.Visibility = Visibility.Visible;
            btn_annuler.Visibility = Visibility.Visible;

            txtBox_refConvention.IsReadOnly = false;
            txtBox_codeProduit.IsReadOnly = false;
            txtBox_ribCompte.IsReadOnly = false;
            txtBox_codeCompanie.IsReadOnly = false;
            txtBox_nomOrganisme.IsReadOnly = false;
            txtBox_raisonSociale.IsReadOnly = false;


            txtBox_refConvention.Background = Brushes.White;
            txtBox_codeProduit.Background = Brushes.White;
            txtBox_ribCompte.Background = Brushes.White;
            txtBox_codeCompanie.Background = Brushes.White;
            txtBox_nomOrganisme.Background = Brushes.White;
            txtBox_raisonSociale.Background = Brushes.White;

            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "SELECT idConvention FROM Utilisateurs WHERE noCINUser='" + lbl_idUser.Content.ToString() + "'";
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrWhiteSpace(reader[0].ToString()))
                    {
                        newConv = 1;
                    }
                }
                reader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion" + ex);
            }


            /*    try
                {
                    connection.Open();
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE Convention SET refConvention = '" + txtBox_refConvention.Text + "', codeProduit = '" + txtBox_codeProduit.Text + "', numCompte = '" + txtBox_ribCompte.Text + "', codeCompagnie = '" + txtBox_codeCompanie.Text + "', nomOrganisme = '" + txtBox_nomOrganisme.Text + "', raisonSociale = '" + txtBox_raisonSociale.Text + "';";
                    OleDbDataReader reader1 = command1.ExecuteReader();
                    while (reader1.Read())
                    {
                        refConvention = reader1[0].ToString();
                    }
                    reader1.Close();
                    if (!string.IsNullOrWhiteSpace(refConvention))
                    {
                        OleDbCommand command2 = new OleDbCommand();
                        command2.Connection = connection;
                        command2.CommandText = "UPDATE Convention SET refConvention = '" + txtBox_refConvention.Text + "', codeProduit = '" + txtBox_codeProduit.Text + "', numCompte = '" + txtBox_ribCompte.Text + "', codeCompagnie = '" + txtBox_codeCompanie.Text + "', nomOrganisme = '" + txtBox_nomOrganisme.Text + "', raisonSociale = '" + txtBox_raisonSociale.Text + "';";
                        OleDbDataReader reader2 = command2.ExecuteReader();
                        while (reader2.Read())
                        {
                            txtBox_refConvention.Text = reader2[0].ToString();
                            txtBox_codeProduit.Text = reader2[1].ToString();
                            txtBox_ribCompte.Text = reader2[3].ToString();
                            txtBox_codeCompanie.Text = reader2[4].ToString();
                            txtBox_nomOrganisme.Text = reader2[5].ToString();
                            txtBox_raisonSociale.Text = reader2[6].ToString();
                        }

                    }




                    connection.Open();
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE Convention SET refConvention = '" + txtBox_refConvention.Text + "', codeProduit = '" + txtBox_codeProduit.Text + "', numCompte='" + txtBox_ribCompte.Text + "', codeCompagnie='" + txtBox_codeCompanie.Text + "', nomOrganisme ='" + txtBox_nomOrganisme.Text + "', raisonSociale='" + txtBox_raisonSociale.Text + "';";
                    command.ExecuteNonQuery();
                    MessageBox.Show("Informations enregistrées");
                    WelcomeWindow welcome = new WelcomeWindow(lbl_idUser.Content.ToString());
                    this.Hide();
                    welcome.lbl_utilisateur.Content = lbl_nomUser.Content;
                    welcome.Show();
                    connection.Close();
                    */
        }
        private void txtBox_codeCompanie_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void txtBox_ribCompte_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void btn_Enregistrer_Click(object sender, RoutedEventArgs e)
        { 
            try
            {
                connection.Open();
                if (newConv == 1)
                {
                    OleDbCommand command = new OleDbCommand();
                    OleDbCommand command1 = new OleDbCommand();
                    command.Connection = connection;
                    command1.Connection = connection;
                    command.CommandText = "INSERT INTO Convention ( refConvention, codeProduit, numCompte, codeCompagnie, nomOrganisme, raisonSociale ) VALUES ('" + txtBox_refConvention.Text + "', '" + txtBox_codeProduit.Text + "','" + txtBox_ribCompte.Text + "'," + txtBox_codeCompanie.Text + " ,'" + txtBox_nomOrganisme.Text + "','" + txtBox_raisonSociale.Text + "');";
                    command1.CommandText = " UPDATE Utilisateurs SET idConvention = '" + txtBox_refConvention.Text + "' WHERE noCINUser = '" + lbl_idUser.Content.ToString() + "' ;";
                    command.ExecuteNonQuery();
                    command1.ExecuteNonQuery();
                    MessageBox.Show("Informations enregistrées");
                }
                else
                {
                    OleDbCommand command = new OleDbCommand();
                    OleDbCommand command1 = new OleDbCommand();
                    command.Connection = connection;
                    command1.Connection = connection;
                    command.CommandText = "UPDATE Convention SET refConvention = '" + txtBox_refConvention.Text + "', codeProduit = '" + txtBox_codeProduit.Text + "', numCompte = '" + txtBox_ribCompte.Text + "', codeCompagnie = '" + txtBox_codeCompanie.Text + "', nomOrganisme = '" + txtBox_nomOrganisme.Text + "', raisonSociale = '" + txtBox_raisonSociale.Text + "';";
                    command1.CommandText = " UPDATE Utilisateurs SET idConvention = '" + txtBox_refConvention.Text + "' WHERE noCINUser = '" + lbl_idUser.Content.ToString() + "' ;";
                    command.ExecuteNonQuery();
                    command1.ExecuteNonQuery();
                    MessageBox.Show("Informations enregistrées");
                }
                connection.Close();
                setPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion" + ex);
            }

        }

    private void btn_annuler_Click(object sender, RoutedEventArgs e)
        {
            setPage();
        }


    }
}
