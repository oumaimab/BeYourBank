using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour DeleteUserWindow.xaml
    /// </summary>
    public partial class DeleteUserWindow : Window
    {
        private OleDbConnection connection = new OleDbConnection();
        public DeleteUserWindow(string iduser)
        {
            InitializeComponent();
            lbl_idUser.Content = iduser;
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data Source=C:\Users\MYC\Documents\PFE\BeYourBankBD.accdb";
        }

        private void btn_yes_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue("@cu", lbl_idUser.Content.ToString());
            cmd.CommandText = "delete from [Utilisateurs] where noCINUser= @cu ";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Utilisateur supprimé avec succès !");
        }

        private void btn_no_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
