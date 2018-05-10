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
        private OleDbConnection con = new OleDbConnection();
        

        public AddUserWindow()
        {
            InitializeComponent();
            
            FillCombo();
        }
        public void FillCombo()
        {

            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select refConvention from [Convention];";
            List<string> LstConv = new List<string>();
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                LstConv.Add(String.Format("{0}",reader[0]));
            }
            // MessageBox.Show("done zeema " + LstConv[0]);
            RefConv.ItemsSource = LstConv;
            con.Close();
        }
    public string loginGenerator(TextBox fName, TextBox lName ) {
            con = new OleDbConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
        OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select login from [Utilisateurs];";
            List<string> LstLogin = new List<string>();
        OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               LstLogin.Add(String.Format("{0}",reader[0]));
            }
            con.Close();
            string result = fName.Text.ToString().Substring(0,2) + lName.Text + "@byb";
       if (LstLogin.Contains(result))
            {
                    return (result.Substring(0,result.Length-4) + "1@byb");
              

            }
       else {
            return result;
               
                    }

            
        }
        private void adButton_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            if (UserLName.Text != "")
            {
                if (UserFName.Text != "")
                {
                    if (CINUser.Text != "")
                    {
                        if (CINUser.Text != "")
                        {
                            if (MdpUser.Text != "")
                            {
                                if (Mdp2User.Text == MdpUser.Text)
                                {

                                    cmd.Connection = con;
                                    string loginG = loginGenerator(UserFName, UserLName);
                                    cmd.CommandText= "INSERT INTO Utilisateurs Values ('" + CINUser.Text + "', '" + UserLName.Text  + "', '" + UserFName.Text + "', '" +telUser.Text  + "', '" +MailUser.Text + "', '" + loginG  + "', '" + MdpUser.Text  + " ', ' " + RefConv.SelectedItem.ToString() + " ')" ; 
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Utilisateur ajouté avec login:"+loginG);
                                    WindowGestionUtilisateurs wg = new WindowGestionUtilisateurs();
                                    con.Close();
                                    
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
