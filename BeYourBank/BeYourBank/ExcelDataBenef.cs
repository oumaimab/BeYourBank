using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Configuration;
using System.IO;

namespace BeYourBank
{
    class ExcelDataBenef
    {
        OleDbConnection con;

        public void bindexcel(string filename)
        {

            con = new OleDbConnection();

            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            con.Open();
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();


            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook;
            Excel.Worksheet worksheet;
            Excel.Range range;
            workbook = excelApp.Workbooks.Open(filename);
            worksheet = (Excel.Worksheet)workbook.Worksheets.get_Item(1);

            int column = 0;
            int row = 0;

            range = worksheet.UsedRange;
            DataTable dt = new DataTable();
          
            for (row = 2; row <= range.Rows.Count; row++)
            {
                String[] lst = new string[20];
                for (column = 1; column <= range.Columns.Count; column++)
                {
                    lst[column] = (range.Cells[row, column] as Excel.Range).Value2.ToString();

                }
               
                cmd.Connection = con;
                cmd.CommandText = "insert into [Beneficiaire] values ('" + lst[1] + "','" + lst[2] + "','" + lst[3] + "','" + lst[4] + "','" + lst[5] + "','" + lst[6] + "','" + lst[7] + "','" + lst[8] + "','" + lst[9] + "','" + lst[10] + "','" + lst[11] + "','" + lst[12] + "','" + lst[13] + "')";
                cmd.ExecuteNonQuery();
            }
            workbook.Close(true, Missing.Value, Missing.Value);
            excelApp.Quit();
            MessageBox.Show("Liste ajoutée avec succès");

        }
     
    }
}

