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

namespace Language_3ISP97_TuzhilovDvoryaninov
{
    /// <summary>
    /// Логика взаимодействия для ListOfClientServices.xaml
    /// </summary>
    public partial class ListOfClientServices : Window
    {
        EF.Client globalClient;

        public ListOfClientServices(EF.Client client)
        {
            InitializeComponent();
            globalClient = client;
            Filter();
        }

        public void Filter()
        {

            List<EF.ClientService> services = new List<EF.ClientService>();
            services = ClassHelper.AppData.Context.ClientService.Where(i => i.IDClient == globalClient.ID).ToList();
            foreach (EF.ClientService service in services)
            {
                service.CountOfDocument = ClassHelper.AppData.Context.ClientServiceDocument.ToList().Where(i => i.ClientService.ID == service.ID).Count();
            }
            lvService.ItemsSource = services;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
