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
    class ExcelData
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
            //string st="";
            range = worksheet.UsedRange;
            DataTable dt = new DataTable();
            MessageBox.Show(range.Rows.Count.ToString());
            MessageBox.Show((range.Cells[1, 1] as Excel.Range).Value2.ToString());

            for (row = 2; row <= range.Rows.Count; row++)
            {
                String[] lst = new string[20];
                for (column = 1; column <= range.Columns.Count; column++)
                {
                   // MessageBox.Show(dr[column].ToString());
                    lst[column] = (range.Cells[row, column] as Excel.Range).Value2.ToString();

                }
                //MessageBox.Show(lst[1].ToString());

                cmd.Connection = con;
                cmd.CommandText = "insert into [Utilisateurs] values ('" + lst[1] + "','" + lst[2] + "','" + lst[3] + "','" + lst[4] + "','" + lst[5] + "','" + lst[6] + "','" + lst[7] + "','" + lst[8] + "');";
                cmd.ExecuteNonQuery();

            }
                workbook.Close(true, Missing.Value, Missing.Value);
            excelApp.Quit();


        }
        //public List<string> data
        //    {
        //        get
        //        {
        //            Excel.Application xlApp = new Excel.Application();
        //Excel.Workbook xlWorkBook;
        //Excel.Worksheet xlWorkSheet;
        //Excel.Range range;

        //xlWorkBook = xlApp.Workbooks.Open("C:\\Users\\Sara\\Desktop\\testest.xlsx");
        //        List<string> Folders = new List<string>();
        //            //string[,] Folders = new string[2, 6];

        //            try
        //            {
        //            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
        //            range = xlWorkSheet.UsedRange;



        //            for (int cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
        //            {
        //                Folders.Add(((range.Cells[2, cCnt] as Excel.Range).Value).ToString());
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            //some error msg
        //        }
        //        finally
        //        {
        //            xlWorkBook.Close();
        //            xlApp.Quit();
        //        }
        //        return Folders;
        //    }
        //        }
        //    }

    }
}

