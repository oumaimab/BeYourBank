using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
using System.Windows.Shapes;

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour EditMdp.xaml
    /// </summary>
    public partial class EditMdp : Window

    {
        private OleDbConnection connection = new OleDbConnection();

        public EditMdp(string idUser)
        {
            InitializeComponent();
            lbl_idUser_editmdp.Content = idUser;
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            OleDbCommand cmd1 = new OleDbCommand();
            string oldPassword= null  ;

            if (connection.State != ConnectionState.Open)
                connection.Open();

            cmd.Connection = connection;
            cmd.CommandText = "select [password] from [utilisateurs] where noCINUser ='" + lbl_idUser_editmdp.Content.ToString() + "';";
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               oldPassword = reader[0].ToString();
            }
            oldPassword = Regex.Replace(oldPassword, @"\s", "");
            if (passwordBox_old.Password.Equals(oldPassword))
            {
                if (passwordBox_new.Password == passwordBox_new1.Password)
                {
                    cmd1.Connection = connection;

                   cmd1.CommandText = "update [utilisateurs] set [password] ='" + passwordBox_new.Password + "' where noCINUser = '" + lbl_idUser_editmdp.Content.ToString() + "';";
                    cmd1.ExecuteNonQuery();
                    MessageBox.Show("Mot de passe mis à jour.");
                    this.Close();
                }
                else { MessageBox.Show("Les mots de passe ne sont pas identiques.");
                    passwordBox_old.Password = "";
                    passwordBox_new.Password = "";
                    passwordBox_new1.Password = "";
                }
            }
            else { MessageBox.Show("Ancien mot de passe incorrect.");
                passwordBox_old.Password = "";
                passwordBox_new.Password = "";
                passwordBox_new1.Password = "";
            }



        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
