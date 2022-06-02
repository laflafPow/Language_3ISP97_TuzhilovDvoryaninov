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
        int numberPage = 0;
        int numberPageText = 1;
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

            tbCountPage.Text = list.Count.ToString();

            var listView = list;

            switch (cbPage.SelectedIndex)
            {
                case 1:                  
                    listView = list.Skip(numberPage * 10).Take(10).ToList();                 
                    break;
                case 2:
                    listView = list.Skip(numberPage * 50).Take(50).ToList();
                    break;
                case 3:
                    listView = list.Skip(numberPage * 200).Take(200).ToList();
                    break;
            }

            if (cbPage.SelectedIndex != 0)
            {
                if (listView.Count < Convert.ToInt32(cbPage.SelectedItem))
                {
                    tbNumberPage.Text = Convert.ToString(list.Count);
                }
                else
                {
                    tbNumberPage.Text = Convert.ToString(numberPageText * Convert.ToInt32(cbPage.SelectedItem));
                }
            }
            else
            {
                tbNumberPage.Text = Convert.ToString(list.Count);
            }

            //if (listView.Count() % 10 != 0)
            //{
            //    tbNumberPage.Text = Convert.ToString(list.Count);
            //}
            //else
            //{
            //    tbNumberPage.Text = Convert.ToString(numberPageText * 10);
            //}

            if (numberPage == 0)
            {
                btnBack.IsEnabled = false;
            }
            else
            {
                btnBack.IsEnabled = true;

            }
            if (tbCountPage.Text == tbNumberPage.Text)
            {
                btnNext.IsEnabled = false;
            }
            else
            {
                btnNext.IsEnabled = true;
            }

            LVClientList.ItemsSource = listView;
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            numberPageText = 1;
            numberPage = 0;
            Filter();
        }

        private void cbGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            numberPageText = 1;
            numberPage = 0;
            Filter();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            numberPageText = 1;
            numberPage = 0;
            Filter();
        }

        private void chbBirthday_Checked(object sender, RoutedEventArgs e)
        {
            numberPageText = 1;
            numberPage = 0;
            Filter();
        }

        private void chbBirthday_Unchecked(object sender, RoutedEventArgs e)
        {
            numberPageText = 1;
            numberPage = 0;
            Filter();
        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            numberPageText = 1;
            numberPage = 0;
            tbSearch.Text = "";
            cbGender.SelectedIndex = 0;
            cbSort.SelectedIndex = 0;
            chbBirthday.IsChecked = false;
            Filter();
        }

        private void cbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            numberPageText = 1;
            numberPage = 0;
            Filter();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            numberPageText += 1;
            numberPage += 1;
            Filter();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            numberPageText -= 1;
            numberPage -= 1;
            Filter();
        }

        private void lvClient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                EF.Client clientID = LVClientList.SelectedItem as EF.Client;

                if (LVClientList.SelectedItem is EF.Client)
                {
                    if (ClassHelper.AppData.Context.ClientService.Where(i => i.IDClient == clientID.ID).ToList().FirstOrDefault() != null)
                    {
                        MessageBox.Show("Нельзя удалить клиента, так как он имеет информацию о посещении", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    else
                    {
                        var result = MessageBox.Show("Вы уверены, что хотите удалить клиента?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Information);
                        if (result.ToString() == "Yes")
                        {
                            ClassHelper.AppData.Context.Client.Remove(clientID);
                            ClassHelper.AppData.Context.SaveChanges();
                            Filter();
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
        }

        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {
            ClientAddWindow clientAddWindow = new ClientAddWindow();
            clientAddWindow.ShowDialog();
            Filter();
        }

        private void LVClientList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LVClientList.SelectedItem is EF.Client)
            {
                var client = LVClientList.SelectedItem as EF.Client;

                ClientAddWindow clientAddWindow = new ClientAddWindow(client);
                clientAddWindow.ShowDialog();
                Filter();

            }
        }

        private void btnService_Click(object sender, RoutedEventArgs e)
        {
            if (LVClientList.SelectedItem is EF.Client)
            {
                EF.Client client = LVClientList.SelectedItem as EF.Client;

                ListOfClientServices services = new ListOfClientServices(client);
                services.ShowDialog();
            }
        }
    }
}
