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

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour DatePickers.xaml
    /// </summary>
    public partial class DatePickers : Window
    {
        public DatePickers()
        {
            InitializeComponent();
        }

        private void Btn_Continuer_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateDebut = datePicker_de.SelectedDate.Value.Date;
            DateTime dateFin = datePicker_a.SelectedDate.Value.Date;


            //MessageBox.Show(resultDebut.Day + "/" + resultDebut.Month + "/" + resultDebut.Year);
            //MessageBox.Show(resultFin.Day + "/" + resultFin.Month + "/" + resultFin.Year);
            MessageBox.Show("de" + dateDebut.ToString() + "a" + dateFin.ToString());

            Page_dashboard pd = new Page_dashboard();
            pd.showCharts(dateDebut,dateFin);
            Close();

        }

        private void Btn_Annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
