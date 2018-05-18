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
    /// Logique d'interaction pour PageHistorique.xaml
    /// </summary>
    public partial class PageHistorique : Page
    {
        private OleDbConnection connection = new OleDbConnection();
        public PageHistorique()
        {
            InitializeComponent();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            BindGrid();
        }

        public void BindGrid()
        {
            try
            {
                connection.Open();
                string sql = "SELECT dateOperation , TypeOperation , count(idOperation) as nbrB FROM Operations group by dateOperation , TypeOperation order by dateOperation desc ";
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connection);
                DataTable ds = new DataTable("Beneficiare_table");
                dataAdapter.Fill(ds);
                connection.Close();
                dataGrid_history.ItemsSource = ds.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connection" + ex);
            }
        }
    }
}
