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
    /// Logique d'interaction pour EditUser.xaml
    /// </summary>
    public partial class EditUser : Window
    {
        OleDbConnection con;

        public EditUser()
        {
            InitializeComponent();
        }
        public void FillCombo()
        {
            con = new OleDbConnection();

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
                LstConv.Add(String.Format("{0}", reader[0]));
            }
            // MessageBox.Show("done zeema " + LstConv[0]);
            comboConvEdit.ItemsSource = LstConv;
            con.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            con = new OleDbConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            con.Open();
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            if (MdpUserEdit.Password == Mdp2UserEdit.Password) { 
                cmd.Connection = con;
                cmd.CommandText = " update [Utilisateurs] set nomUser = '" + UserLNameEdit.Text + "',prenomUser ='" + UserFNameEdit.Text + "', noTelUser ='" + telUserEdit.Text + "',adrMail=' " + MailUserEdit.Text + "', login = '" + loginUserEdit.Text + "'  ,[idConvention]=' " + comboConvEdit.SelectionBoxItem.ToString() + "'  , [password] = '" + MdpUserEdit.Password + ",' where [noCINUser]='" + CINUserEdit.Text + "');";
                cmd.ExecuteNonQuery();
                MessageBox.Show("Utilisateur modifié");
                this.Hide();
            }
            else { MessageBox.Show("Mots de passe non identiques"); }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
this.Hide();
        }
    }
}
