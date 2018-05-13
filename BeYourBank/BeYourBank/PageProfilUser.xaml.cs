using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
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

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour PageProfilUser.xaml
    /// </summary>
    public partial class PageProfilUser : Page
    {
        private OleDbConnection connection = new OleDbConnection();

        public PageProfilUser(string idUser)
        {
         
            InitializeComponent();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            lbl_idUser_profile.Content = idUser;
            setPage();
        }
        private void setPage()
        {
            OleDbCommand cmd = new OleDbCommand();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            cmd.Connection = connection;

            cmd.CommandText = "select * from [utilisateurs] where [noCINUser]='" + lbl_idUser_profile.Content.ToString() + "';";
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               txtBox_CIN.Text = reader[0].ToString();
               txtBox_nom.Text = reader[1].ToString();
               txtBox_prenom.Text = reader[2].ToString();
               txtBox_tel.Text = reader[3].ToString();
                txtBox_mail.Text = reader[4].ToString();
                txtBox_login.Text = reader[5].ToString();
                txtBox_password.Password = reader[6].ToString();
                txtBox_convention.Text = reader[7].ToString();
            }

            reader.Close();
            connection.Close();
        }

        

        private void buttonCIN_Click(object sender, RoutedEventArgs e)
        {
           

            this.txtBox_CIN.IsReadOnly = false;
            this.txtBox_CIN.Background = Brushes.White; 
            this.txtBox_CIN.IsEnabled = true;
            this.button_checkCIN.Visibility = Visibility.Visible;
           
        }

        private void buttonNom_Click(object sender, RoutedEventArgs e)
        {
            this.txtBox_nom.IsReadOnly = false;
            this.txtBox_nom.Background = Brushes.White;
            this.txtBox_nom.IsEnabled = true;
            this.button_checkNom.Visibility = Visibility.Visible;
        }

        private void buttontel_Click(object sender, RoutedEventArgs e)
        {
            this.txtBox_tel.IsReadOnly = false;
            this.txtBox_tel.Background = Brushes.White;
            this.txtBox_tel.IsEnabled = true;
            this.button_checkTel.Visibility = Visibility.Visible;
        }

        private void buttonmdp_Click(object sender, RoutedEventArgs e)
        {
            EditMdp em = new EditMdp(lbl_idUser_profile.Content.ToString());
            em.ShowDialog();
        }

        private void buttonmail_Click(object sender, RoutedEventArgs e)
        {
            this.txtBox_mail.IsReadOnly = false;
            this.txtBox_mail.Background = Brushes.White;
            this.txtBox_mail.IsEnabled = true;
            this.button_checkMail.Visibility = Visibility.Visible;
        }

        private void buttonprenom_Click(object sender, RoutedEventArgs e)
        {
            this.txtBox_prenom.IsReadOnly = false;
            this.txtBox_prenom.Background = Brushes.White;
            this.txtBox_prenom.IsEnabled = true;
            this.button_checkprenom.Visibility = Visibility.Visible;
        }
        public void check_CIN_Click(object sender , RoutedEventArgs e)
           
        {
        
            connection = new OleDbConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            connection.Open();
            OleDbCommand cmd = new OleDbCommand();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            cmd.Connection = connection;
            cmd.CommandText = "update [utilisateurs] set noCINUser ='" + txtBox_CIN.Text + "' where noCINUser = '" +lbl_idUser_profile.Content.ToString() + "';";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Vous avez bien changé votre identifiant.Veuillez vous reconnecter pour continuer");
            button_checkCIN.Visibility = Visibility.Hidden;
            txtBox_CIN.IsReadOnly = true;
            txtBox_CIN.Background = Brushes.LightGray;
        }
        public void check_nom_Click(object sender, RoutedEventArgs e)

        {

            connection = new OleDbConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            connection.Open();
            OleDbCommand cmd = new OleDbCommand();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            cmd.Connection = connection;
            cmd.CommandText = "update [utilisateurs] set nomUser ='" + txtBox_nom.Text + "' where noCINUser = '" + lbl_idUser_profile.Content.ToString() + "';";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Nom modifié");
            button_checkNom.Visibility = Visibility.Hidden;
            txtBox_nom.IsReadOnly = true;
            txtBox_nom.Background = Brushes.LightGray;
        }

        public void check_prenom_Click(object sender, RoutedEventArgs e)

        {
            connection = new OleDbConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            connection.Open();
            OleDbCommand cmd = new OleDbCommand();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            cmd.Connection = connection;
            cmd.CommandText = "update [utilisateurs] set prenomUser ='" + txtBox_prenom.Text + "' where noCINUser = '" + lbl_idUser_profile.Content.ToString() + "';";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Prénom modifié");
            button_checkprenom.Visibility = Visibility.Hidden;
            txtBox_prenom.IsReadOnly = true;
            txtBox_prenom.Background = Brushes.LightGray;
        }
        public void check_tel_Click(object sender, RoutedEventArgs e)

        {
            connection = new OleDbConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            connection.Open();
            OleDbCommand cmd = new OleDbCommand();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            cmd.Connection = connection;
            cmd.CommandText = "update [utilisateurs] set noTelUser ='" + txtBox_tel.Text + "' where noCINUser = '" + lbl_idUser_profile.Content.ToString() + "';";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Nom modifié");
            button_checkTel.Visibility = Visibility.Hidden;
            txtBox_tel.IsReadOnly = true;
            txtBox_tel.Background = Brushes.LightGray;
        }
        public void check_Mail_Click(object sender, RoutedEventArgs e)

        {
            connection = new OleDbConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            connection.Open();
            OleDbCommand cmd = new OleDbCommand();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            cmd.Connection = connection;
            cmd.CommandText = "update [utilisateurs] set adrMail ='" + txtBox_mail.Text + "' where noCINUser = '" + lbl_idUser_profile.Content.ToString() + "';";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Nom modifié");
            button_checkMail.Visibility = Visibility.Hidden;
            txtBox_mail.IsReadOnly = true;
            txtBox_mail.Background = Brushes.LightGray;
        }

        public void check_mdp_Click(object sender, RoutedEventArgs e)

        {
        }





    }
}
