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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Language_3ISP97_TuzhilovDvoryaninov.ClassHelper.AppData;
using Language_3ISP97_TuzhilovDvoryaninov.EF;

namespace Language_3ISP97_TuzhilovDvoryaninov
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LVClientList.ItemsSource = Context.Client.ToList();
            List<Gender> genders = Context.Gender.ToList();
            genders.Insert(0, new Gender() { Title = "Все" });
            cbGender.ItemsSource = genders;
            cbGender.DisplayMemberPath = "Title";
            cbGender.SelectedIndex = 0;

            cbSort.SelectedIndex = 0;
            cbSort.ItemsSource = new List<String>
            {
                "По умолчанию",
                "Фамилия (в алфавитном порядке)",
                "Дате последнего посещения (от новых к старым)",
                "Количеству посещений (от большего к меньшему)"
            };
            cbPage.ItemsSource = new List<String>
            {
                "Все",
                "10",
                "50",
                "200"
            };
            cbPage.SelectedIndex = 0;
        }

        public void Filter()
        {
            var list = Context.Client.ToList();

            if (cbGender.SelectedIndex != 0)
            {
                var gender = cbGender.SelectedItem as Gender;
                list = list.Where(i => i.IDGender == gender.ID).ToList();
            }

            switch (cbSort.SelectedIndex)
            {
                case 1:
                    list = list.OrderBy(i => i.LastName).ToList();
                    break;
                
            }

        }
    }
}
