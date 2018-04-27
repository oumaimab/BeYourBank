﻿using System;
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
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data Source=C:\Users\MYC\Documents\PFE\BeYourBankBD.accdb";
            BindGrid();
        }

        public void BindGrid()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            cmd.Connection = connection;
            cmd.CommandText = "select * from [Utilisateurs]";

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
            else
            {
                MessageBox.Show("Pleaase select chi haja ...");
            }
        }
    }
}
