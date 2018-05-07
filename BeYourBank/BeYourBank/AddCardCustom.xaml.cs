using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    
    public partial class AddCardCustom : Window
    {
        private OleDbConnection connection = new OleDbConnection();
        private List<Beneficiaire> liste_creation = new List<Beneficiaire>();
        public AddCardCustom(string idUser)
        {
            InitializeComponent();
            lbl_idUser.Content = idUser;
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data Source=C:\Users\MYC\Documents\PFE\BeYourBankBD.accdb";
        }

        public void fill_list()
        {
            try
            {
                connection.Open();
                for (int i = 0; i < listBox_CIN.Items.Count; i++)
                {
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    command.CommandText = "select * from Beneficiaire where noCINBeneficiaire ='" + listBox_CIN.Items[i].ToString() + "';";
                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Beneficiaire bn = new Beneficiaire(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), reader[8].ToString(), reader[9].ToString(), reader[10].ToString(), reader[11].ToString(), reader[12].ToString());
                        liste_creation.Add(bn);

                    }
                }
                listView.ItemsSource = liste_creation;

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connection" + ex);
            }
        }
    }
}
