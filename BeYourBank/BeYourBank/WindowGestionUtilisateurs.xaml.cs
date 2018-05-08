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
    /// Logique d'interaction pour WindowGestionUtilisateurs.xaml
    /// </summary>
    public partial class WindowGestionUtilisateurs : Window
    {
        private OleDbConnection connection = new OleDbConnection();
        DataTable dt;
        public WindowGestionUtilisateurs()
        {
            InitializeComponent();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            BindGrid();
        }

        public void BindGrid()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            cmd.Connection = connection;
            cmd.CommandText = "select * from [Utilisateurs] where login <> 'admin';";

            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable("Utilisateurs");
            da.Fill(dt);
            grUsers.ItemsSource = dt.AsDataView();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow au = new AddUserWindow();
            au.ShowDialog();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            //int rowIndex = grUsers.CurrentCell.RowIndex;
            if (grUsers.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)grUsers.SelectedItems[0];
                DeleteUserWindow du = new DeleteUserWindow(row["noCINUser"].ToString());
                du.lbl_name_user.Content = row["nomUser"].ToString() + " " + row["prenomUser"].ToString();
                du.ShowDialog();
            }
        }

        /*
            if (grUsers.SelectedItems.Count > 0)
            {
            DataRowView row = (DataRowView)grUsers.SelectedItems[0];
                OleDbCommand cmd = new OleDbCommand();
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@cu", row["noCINUser"]);
                cmd.CommandText = "delete from [Utilisateurs] where noCINUser= @cu ";
                cmd.ExecuteNonQuery();
                BindGrid();
                MessageBox.Show("Employee Deleted Successfully...");
            }
        }*/

        private void btnDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow main = new MainWindow();
            main.Show();
        }

        private void grUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_edit.IsEnabled = true;
            buttonDelete.IsEnabled = true;
        }

        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            BindGrid();
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {

            EditUser eu = new EditUser();
            if (grUsers.SelectedItems.Count > 0)
            {


                DataRowView row = (DataRowView)grUsers.SelectedItems[0];

                // eu.UserFName.Text = row["nomUser"];
                eu.CINUserEdit.Text = row["noCINUser"].ToString();
                eu.UserLNameEdit.Text = row["nomUser"].ToString();
                eu.UserFNameEdit.Text = row["prenomUser"].ToString();
                eu.telUserEdit.Text = row["noTelUser"].ToString();
                eu.MailUserEdit.Text = row["adrMail"].ToString();
                eu.loginUserEdit.Text = row["login"].ToString();
                eu.MdpUserEdit.Text = row["password"].ToString();
               // eu.comboConvEdit.SelectedValue= row["idConvention"].ToString();                      

                eu.ShowDialog();
            }
            else
            {
                MessageBox.Show("Pleaase select chi haja ...");
            }

        }

        private void buttonImport_Click(object sender, RoutedEventArgs e)
        {
            
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
                                       //dlg.DefaultExt = ".txt"; // Default file extension
                                       //  dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                ExcelData exceldata = new ExcelData();
                exceldata.bindexcel(filename);
            }
        }
    }
}
