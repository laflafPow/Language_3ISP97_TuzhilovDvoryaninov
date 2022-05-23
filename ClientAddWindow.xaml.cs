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
using Language_3ISP97_TuzhilovDvoryaninov.ClassHelper;
using Microsoft.Win32;

namespace Language_3ISP97_TuzhilovDvoryaninov
{
    /// <summary>
    /// Логика взаимодействия для ClientAddWindow.xaml
    /// </summary>
    public partial class ClientAddWindow : Window
    {
        bool isEdit = false;
        EF.Client editClient = new EF.Client();
        string pathPhoto = null;

        public ClientAddWindow()
        {
            InitializeComponent();
            var lastUser = AppData.Context.Client.ToList().Last();
            tbID.Text = (lastUser.ID + 1).ToString();

            isEdit = false;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ValidationClass.ValidationFIO(txtLastName.Text) == false)
            {
                MessageBox.Show("Недопустимая фамилия!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ValidationClass.ValidationFIO(txtFirstName.Text) == false)
            {
                MessageBox.Show("Недопустимое имя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ValidationClass.ValidationFIO(txtPatronymic.Text) == false)
            {
                MessageBox.Show("Недопустимое отчество!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(ValidationClass.ValidationEmail(txtEmail.Text) == false)
            {
                MessageBox.Show("Недопустимый Email!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ValidationClass.ValidationPhone(txtPhone.Text) == false)
            {
                MessageBox.Show("Недопустимый номер телефона!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var resClick = MessageBox.Show("Добавить пользователя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resClick == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                EF.Client newClient = new EF.Client();
                newClient.LastName = txtLastName.Text;
                newClient.FirstName = txtFirstName.Text;
                newClient.Patronymic = txtPatronymic.Text;
                newClient.Phone = txtPhone.Text;
                newClient.Email = txtEmail.Text;
                newClient.DateOfBirth = dpDateOfBirth.SelectedDate.Value;

                if (rbMale.IsChecked == true)
                {
                    newClient.IDGender = 1;
                }
                else
                {
                    newClient.IDGender = 2;
                }

                ClassHelper.AppData.Context.Client.Add(newClient);
                ClassHelper.AppData.Context.SaveChanges();

                MessageBox.Show("Пользователь добавлен");

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnChoosePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
            {
                photoUser.Source = new BitmapImage(new Uri(openFile.FileName));
                pathPhoto = openFile.FileName;
            }
        }

    }
    
}

