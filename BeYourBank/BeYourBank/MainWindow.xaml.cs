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

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //string conString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ToString();
        private OleDbConnection connection = new OleDbConnection();

        public MainWindow()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data Source=C:\Users\MYC\Documents\PFE\BeYourBankBD.accdb";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                ConCheck.Content = "Connection réussie";
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connection" + ex);
            }

        }
        private void btn_signIn_Click(object sender, RoutedEventArgs e)
        {
            connection.Open();
            String idUtilisateur = null;
            String nomUtilisateur = null;
            String prenomUtilisateur = null;
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            command.CommandText = "select * from Utilisateurs where login ='" + textBox_login.Text + "' and password= '" + textBox_mdp.Password + "'";
            OleDbDataReader reader = command.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                count++;
                idUtilisateur = reader[0].ToString();
                nomUtilisateur = reader[1].ToString();
                prenomUtilisateur = reader[2].ToString();
            }
            reader.Close();
            if (count == 1)
            {
                MessageBox.Show("Login et mdp corrects");
                OleDbCommand command1 = new OleDbCommand();
                command1.Connection = connection;
                command1.CommandText = "SELECT idUser FROM Convention WHERE EXISTS (SELECT noCINUser FROM Utilisateurs WHERE login ='" + textBox_login.Text + "' and Convention.idUser = Utilisateurs.noCINUser) ;";
                OleDbDataReader reader1 = command1.ExecuteReader();
                this.Hide();
                if (reader1.Read())
                {
                    WelcomeWindow welcome = new WelcomeWindow();
                    welcome.Show();
                }
                else
                {
                    conventionWindow convWindow = new conventionWindow();
                    convWindow.lbl_nomUser.Content = nomUtilisateur + "  " + prenomUtilisateur;
                    convWindow.lbl_idUser.Content = idUtilisateur;
                    convWindow.Show();
                }
                reader1.Close();

            }
            else if (count > 1)
            {
                MessageBox.Show("Login et mdp redondants");
            }
            else
            {
                MessageBox.Show("Login et mdp incorrects. \n Veuillez réessayer !");
                textBox_login.Clear();
                textBox_mdp.Clear();

            }
            connection.Close();
        }
    }
}
