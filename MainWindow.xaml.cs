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
        int numberPage = 1;
        int countClient;

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
            List<EF.Client> list = new List<EF.Client>();

            list = Context.Client.ToList();

            list = list.
                Where(i => i.LastName.ToLower().Contains(tbSearch.Text.ToLower())
                || i.FirstName.ToLower().Contains(tbSearch.Text.ToLower())
                || i.Patronymic.ToLower().Contains(tbSearch.Text.ToLower())
                || i.FIO.ToLower().Contains(tbSearch.Text.ToLower())
                || i.Phone.ToLower().Contains(tbSearch.Text.ToLower())
                || i.Email.ToLower().Contains(tbSearch.Text.ToLower())).
                ToList();

            if (cbGender.SelectedIndex != 0)
            {
                var gender = cbGender.SelectedItem as Gender;
                list = list.Where(i => i.IDGender == gender.ID).ToList();
            }

            switch (cbSort.SelectedIndex)
            {
                case 0:
                    list = list.OrderBy(i => i.ID).ToList();
                    break;
                case 1:
                    list = list.OrderBy(i => i.LastName).ToList();
                    break;
                case 2:
                    list = list.OrderByDescending(i => i.LastVisit).ToList();
                    break;
                case 3:
                    list = list.OrderByDescending(i => i.CountVisit).ToList();
                    break;
            }

            if (chbBirthday.IsChecked == false)
            {
                
            }
            else
            {
                list = list.Where(i => i.DateOfBirth.Month == DateTime.Today.Month).ToList();
            }

            var listView = list;

            switch (cbPage.SelectedIndex)
            {
                case 0:
                    tbCountPage.Text = "1";
                    break;
                case 1:
                    tbCountPage.Text = ((countClient / 10) - 1).ToString();
                    listView = list.Skip(numberPage * 10).Take(10).ToList();
                    break;
                case 2:
                    tbCountPage.Text = (countClient / 50).ToString();
                    listView = list.Skip(numberPage * 50).Take(50).ToList();
                    break;
                case 3:
                    tbCountPage.Text = (countClient / 200).ToString();
                    listView = list.Skip(numberPage * 200).Take(200).ToList();
                    break;
            }

            countClient = list.Count;
            
            tbNumberPage.Text = (numberPage + 1).ToString();

            if (numberPage == 0)
            {
                btnBack.IsEnabled = false;
            }
            else
            {
                btnBack.IsEnabled = true;

            }

            if (numberPage == Convert.ToInt32(tbCountPage.Text) - 1)
            {
                btnNext.IsEnabled = false;
            }
            else
            {
                btnNext.IsEnabled = true;
            }

            LVClientList.ItemsSource = listView;

            tbNumberPage.Text = (listView.Count * numberPage).ToString();
            tbCountPage.Text = list.Count.ToString();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void cbGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void chbBirthday_Checked(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        private void chbBirthday_Unchecked(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            cbGender.SelectedIndex = 0;
            cbSort.SelectedIndex = 0;
            chbBirthday.IsChecked = false;
            Filter();
        }

        private void cbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            numberPage += 1;
            Filter();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            numberPage -= 1;
            Filter();
        }

        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {
            ClientAddWindow clientAddWindow = new ClientAddWindow();
            clientAddWindow.ShowDialog();
        }
    }
}
