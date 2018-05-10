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
        bool oldValues = false;

        string oldRefConv;
        string oldCodeProduit;
        string oldRibCompte;
        string oldCodeCompagnie;
        string oldnomOrganisme;
        string oldRaisonSociale;

        public PageConventionUser(string idUser)
        {
            InitializeComponent();
            lbl_idUser.Content = idUser;
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
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
                OleDbCommand command1 = new OleDbCommand();
                command.Connection = connection;
                command1.Connection = connection;
                command.CommandText = "SELECT idConvention FROM Utilisateurs WHERE noCINUser='" + lbl_idUser.Content.ToString() + "'";
                command1.CommandText = "SELECT * FROM Convention WHERE refConvention= (SELECT idConvention FROM Utilisateurs WHERE noCINUser='" + lbl_idUser.Content.ToString() + "');";
                OleDbDataReader reader = command.ExecuteReader();
                OleDbDataReader reader1 = command1.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrWhiteSpace(reader[0].ToString()) || reader[0].ToString() == "Aucune")
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

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            btn_edit.IsEnabled = false;
            btn_Enregistrer.Visibility = Visibility.Visible;
            btn_annuler.Visibility = Visibility.Visible;

            txtBox_codeProduit.IsReadOnly = false;
            txtBox_ribCompte.IsReadOnly = false;
            txtBox_codeCompanie.IsReadOnly = false;
            txtBox_nomOrganisme.IsReadOnly = false;
            txtBox_raisonSociale.IsReadOnly = false;


            txtBox_codeProduit.Background = Brushes.White;
            txtBox_ribCompte.Background = Brushes.White;
            txtBox_codeCompanie.Background = Brushes.White;
            txtBox_nomOrganisme.Background = Brushes.White;
            txtBox_raisonSociale.Background = Brushes.White;

            if(newConv == 1)
            {
                txtBox_refConvention.Background = Brushes.White;
                txtBox_refConvention.IsReadOnly = false;
            }



            if (!oldValues)
            {
                if(txtBox_refConvention.Text != "")
                {
                    oldRefConv = txtBox_refConvention.Text;
                }
                if (txtBox_codeProduit.Text != "")
                {
                    oldCodeProduit = txtBox_codeProduit.Text;
                }
                if (txtBox_ribCompte.Text != "")
                {
                    oldRibCompte = txtBox_ribCompte.Text;
                }
                if (txtBox_codeCompanie.Text != "")
                {
                    oldCodeCompagnie = txtBox_codeCompanie.Text;
                }
                if (txtBox_nomOrganisme.Text != "")
                {
                    oldnomOrganisme = txtBox_nomOrganisme.Text;
                }
                if (txtBox_raisonSociale.Text != "")
                {
                    oldRaisonSociale = txtBox_raisonSociale.Text;
                }
                oldValues = true;
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
                    command.CommandText = "INSERT INTO Convention ( refConvention, codeProduit, numCompte, codeCompagnie, nomOrganisme, raisonSociale, IndexFichier ) VALUES ('" + txtBox_refConvention.Text + "', '" + txtBox_codeProduit.Text + "','" + txtBox_ribCompte.Text + "'," + txtBox_codeCompanie.Text + " ,'" + txtBox_nomOrganisme.Text + "','" + txtBox_raisonSociale.Text + "','"+"0"+"');";
                    command1.CommandText = " UPDATE Utilisateurs SET idConvention = '" + txtBox_refConvention.Text + "' WHERE noCINUser = '" + lbl_idUser.Content.ToString() + "' ;";
                    command.ExecuteNonQuery();
                    command1.ExecuteNonQuery();
                    MessageBox.Show("Informations enregistrées");
                }
                else
                {
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE Convention SET codeProduit = '" + txtBox_codeProduit.Text + "', numCompte = '" + txtBox_ribCompte.Text + "', codeCompagnie = '" + txtBox_codeCompanie.Text + "', nomOrganisme = '" + txtBox_nomOrganisme.Text + "', raisonSociale = '" + txtBox_raisonSociale.Text + "' where refConvention = '" + txtBox_refConvention.Text + "';";
                    //command1.CommandText = " UPDATE Utilisateurs SET idConvention = '" + txtBox_refConvention.Text + "' WHERE noCINUser = '" + lbl_idUser.Content.ToString() + "' ;";
                    command.ExecuteNonQuery();
                   // command1.ExecuteNonQuery();
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
            if (oldValues)
            {
                if (!string.IsNullOrWhiteSpace(oldRefConv))
                {
                    txtBox_refConvention.Text = oldRefConv;
                }
                else
                    txtBox_refConvention.Clear();

                if (!string.IsNullOrWhiteSpace(oldCodeProduit))
                {
                    txtBox_codeProduit.Text = oldCodeProduit;
                }
                else
                    txtBox_codeProduit.Clear();

                if (!string.IsNullOrWhiteSpace(oldRibCompte))
                {
                    txtBox_ribCompte.Text = oldRibCompte;
                }
                else
                    txtBox_ribCompte.Clear();

                if (!string.IsNullOrWhiteSpace(oldCodeCompagnie))
                {
                    txtBox_codeCompanie.Text = oldCodeCompagnie;
                }
                else
                    txtBox_codeCompanie.Clear();

                if (!string.IsNullOrWhiteSpace(oldnomOrganisme))
                {
                    txtBox_nomOrganisme.Text = oldnomOrganisme;
                }
                else
                    txtBox_nomOrganisme.Clear();

                if (!string.IsNullOrWhiteSpace(oldRaisonSociale))
                {
                    txtBox_raisonSociale.Text = oldRaisonSociale;
                }
                else
                    txtBox_raisonSociale.Clear();
            }
            oldValues = false;
            setPage();
        }


    }
}
