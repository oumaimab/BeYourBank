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
using System.Configuration;

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
            
            FillCombo();
        }
        public void FillCombo()
        {

            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            OleDbCommand cmd = new OleDbCommand();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            cmd.Connection = connection;
            cmd.CommandText = "select refConvention from [Convention];";
            List<string> LstConv = new List<string>();
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                LstConv.Add(String.Format("{0}",reader[0]));
            }
           // MessageBox.Show("done zeema " + LstConv[0]);
            comboConv.ItemsSource = LstConv;
            connection.Close();
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

                                    //cmd.Parameters.AddWithValue("@mu", MailUser.Text);
                                    //cmd.Parameters.AddWithValue("@uln", UserLName.Text);
                                    //cmd.Parameters.AddWithValue("@ufn", UserFName.Text);
                                    //cmd.Parameters.AddWithValue("@tu", telUser.Text);
                                    //cmd.Parameters.AddWithValue("@cu", CINUser.Text);

                                    //cmd.Parameters.AddWithValue("@lu", loginUser.Text);
                                    //cmd.Parameters.AddWithValue("@mdpu", MdpUser.Text);
                                    //cmd.Parameters.AddWithValue("@conv", comboConv.SelectedItem.ToString());

                                    cmd.Connection = connection;
                                    //  MessageBox.Show(CINUser.Text+UserFName.Text+UserLName.Text+"  *" +MdpUser.Text);
                                    //cmd.CommandText = "insert into [Utilisateurs] (noCINUser,nomUser,prenomUser,noTelUser,adrMail,login,password) Values(@cu,@uln ,@ufn,@tu,@mu,@lu,@mdpu)";
                                    cmd.CommandText= "INSERT INTO Utilisateurs Values ('" + CINUser.Text + "', '" + UserLName.Text  + "', '" + UserFName.Text + "', '" +telUser.Text  + "', '" +MailUser.Text + "', '" +loginUser.Text  + "', '" + MdpUser.Text  + " ', ' " + comboConv.SelectedItem.ToString() + " ')" ; 
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Utilisateur ajouté");
                                    WindowGestionUtilisateurs wg = new WindowGestionUtilisateurs();
                                    connection.Close();
                                    
                                    this.Hide();
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
