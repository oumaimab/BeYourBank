using System;
using System.Collections.Generic;
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
    /// Logique d'interaction pour TypeCartesWindow.xaml
    /// </summary>
    public partial class TypeCartesWindow : Window
    {
        private OleDbConnection connection = new OleDbConnection();
        public TypeCartesWindow()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data Source=C:\Users\MYC\Documents\PFE\BeYourBankBD.accdb";
            update_list();
        }

        public void update_list()
        {
            ListeTypes.Items.Clear();
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "select * from TypeCarte ;";
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ListeTypes.Items.Add(reader[1].ToString());
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connection" + ex);
            }
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "Insert into TypeCarte (nomType) values ( '" + txtBox_type.Text + "' );";
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connection" + ex);
            }
            update_list();
            txtBox_type.Clear();
        }

        private void ListeTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_supprimer.IsEnabled = true;
        }

        private void btn_supprimer_Click(object sender, RoutedEventArgs e)
        {
            DeleteTypeWindow dtW = new DeleteTypeWindow();
            dtW.lbl_nom_type.Content = ListeTypes.SelectedItem.ToString();
            dtW.ShowDialog();
            update_list();
        }
    }
}
