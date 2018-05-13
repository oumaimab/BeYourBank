using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour fichierGenreWindow.xaml
    /// </summary>
    public partial class fichierGenreWindow : Window
    {
        private OleDbConnection connection = new OleDbConnection();
        public string id_fichier;
        public fichierGenreWindow(string idFichier)
        {
            InitializeComponent();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            id_fichier = idFichier;
        }

        private void btn_envoyer_Click(object sender, RoutedEventArgs e)
        {
            try
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

                MailMessage mailMessage = new MailMessage(senderEmailId, "oumaima.belahsen212@gmail.com");

                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.Subject = "Fichier généré";
                mailMessage.Body = "Bonjour, \n\nVous trouverez ci-joint le fichier généré !\n \nCordialment,";

                mailMessage.Attachments.Add(new Attachment(AppDomain.CurrentDomain.BaseDirectory + "PREP_CONVENTION000000." + id_fichier));
            }
                
                
    
                smptClient.Send(senderEmailId, txtBox_mail.Text, );
                MessageBox.Show("Un mail de récupération vous a été envoyé !");
        }
    }
}
