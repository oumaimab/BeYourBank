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
using System.Data;

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        private OleDbConnection connection = new OleDbConnection();

        public AddUserWindow()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data Source=C:\Users\MYC\Documents\PFE\BeYourBankBD.accdb";
        }

        public void ComboShow()
        {
            try
            {
                RefConv.Items.Add("Aucune");
                connection.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT refConvention FROM Convention ;";
                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (!string.IsNullOrWhiteSpace(reader[0].ToString()))
                    {
                        RefConv.Items.Add(reader[0].ToString());
                    }
                }
                reader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connection" + ex);
            }
        }


        private void adButton_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            if (UserLName.Text != "")
            {
                if (UserFName.Text != "")
                {
                    if (CINUser.Text != "")
                    {
                        if (loginUser.Text != "")
                        {
                            if (MdpUser.Text != "")
                            {
                                if (Mdp2User.Text == MdpUser.Text)
                                {

                                    cmd.Parameters.AddWithValue("@mu", MailUser.Text);
                                    cmd.Parameters.AddWithValue("@uln", UserLName.Text);
                                    cmd.Parameters.AddWithValue("@ufn", UserFName.Text);
                                    cmd.Parameters.AddWithValue("@tu", telUser.Text);
                                    cmd.Parameters.AddWithValue("@cu", CINUser.Text);

                                    cmd.Parameters.AddWithValue("@lu", loginUser.Text);
                                    cmd.Parameters.AddWithValue("@mdpu", MdpUser.Text);

                                    cmd.Connection = connection;
                                    //  MessageBox.Show(CINUser.Text+UserFName.Text+UserLName.Text+"  *" +MdpUser.Text);
                                    //cmd.CommandText = "insert into [Utilisateurs] (noCINUser,nomUser,prenomUser,noTelUser,adrMail,login,password) Values(@cu,@uln ,@ufn,@tu,@mu,@lu,@mdpu)";
                                    cmd.CommandText= "INSERT INTO Utilisateurs Values ('" + CINUser.Text + "', '" + UserLName.Text  + "', '" + UserFName.Text + "', '" +telUser.Text  + "', '" +MailUser.Text + "', '" +loginUser.Text  + "', '" + MdpUser.Text  +"', '" + RefConv.SelectedItem.ToString() + "' );"; 
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Utilisateur ajouté");
                                    WindowGestionUtilisateurs wg = new WindowGestionUtilisateurs();
                                    connection.Close();
                                    
                                    this.Close();
                                    /*
                                    OleDbCommand cmd = new OleDbCommand();
                                    if (connection.State != ConnectionState.Open)
                                        connection.Open(); */
                                }
                                else
                                {
                                    MessageBox.Show("Mots de passe non identiques ");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Veuillez saisir le mot de passe ");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Veuillez saisir le numéro CIN  ");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Veuillez saisir l'adresse mail ");
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez saisir le prénom ");
                }
            }
            else
            {
                MessageBox.Show("Veuillez saisir le nom ");
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
