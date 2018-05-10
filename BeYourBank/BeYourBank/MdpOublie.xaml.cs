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
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Data.OleDb;

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour MdpOublie.xaml
    /// </summary>
    public partial class MdpOublie : Window
    {
        private OleDbConnection connection = new OleDbConnection();
        public MdpOublie()
        {
            InitializeComponent();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
        }

        private void btn_envoyer_Click(object sender, RoutedEventArgs e)
        {
            if (! string.IsNullOrWhiteSpace(txtBox_mail.Text))
            {
                var smtpServerName = ConfigurationManager.AppSettings["SmtpServer"];
                var port = ConfigurationManager.AppSettings["Port"];
                var senderEmailId = ConfigurationManager.AppSettings["SenderEmailId"];
                var senderPassword = ConfigurationManager.AppSettings["SenderPassword"];
                var smptClient = new SmtpClient(smtpServerName, Convert.ToInt32(port))
                {
                    Credentials = new NetworkCredential(senderEmailId, senderPassword),
                    EnableSsl = true
                };
                String login = null;
                String motDePasse = null;
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "select login, password from Utilisateurs where adrMail ='" + txtBox_mail.Text + "' ;";
                OleDbDataReader reader = command.ExecuteReader();
                int count = 0;
                while (reader.Read())
                {
                    count++;
                    login = reader[0].ToString();
                    motDePasse = reader[1].ToString();
                }
                reader.Close();
                if (count == 1)
                {
                    smptClient.Send(senderEmailId, txtBox_mail.Text, " Récupération de mot de passe Be Your Bank ", "Bonjour, \n\nVos identifiants sont comme suit: \n \nIdentifiant :  " + login + " \nMot de passe:  " + motDePasse + "\n");
                    MessageBox.Show("Un mail de récupération vous a été envoyé !");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Le mail saisi n'existe pas. \nVeuillez rééssayer !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtBox_mail.Clear();
                }

                connection.Close();
            }
            else
            {
                MessageBox.Show("Vous n'avez rien saisi. \nVeuillez rééssayer !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

