using System;
using System.Collections.Generic;
using System.IO;
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

        public ClientAddWindow(EF.Client client)
        {
            InitializeComponent();

            tbID.Text = client.ID.ToString();
            txtLastName.Text = client.LastName;
            txtFirstName.Text = client.FirstName;
            txtPatronymic.Text = client.Patronymic;
            txtPhone.Text = client.Phone;
            txtEmail.Text = client.Email;
            dpDateOfBirth.SelectedDate = client.DateOfBirth;

            if (client.IDGender == 1)
            {
                rbMale.IsChecked = true;
            }
            else
            {
                rbFemale.IsChecked = true;
            }
                
            if (client.Photo != null)
            {
                try
                {
                    photoUser.Source = new BitmapImage(new Uri(client.Photo));
                }
                catch (Exception)
                {
                    var uriSource = new Uri(@"/Language_3ISP97_TuzhilovDvoryaninov;component/Resource/PhotoError.png", UriKind.Relative);
                    photoUser.Source = new BitmapImage(uriSource);
                    MessageBox.Show("Не удается найти путь фото!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }              
            }

            tbTitle.Text = "Изменение данных клиента";
            btnAdd.Content = "Сохранить";

            isEdit = true;

            editClient = client;
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

            if (txtPatronymic.Text != "")
            {
                if (ValidationClass.ValidationFIO(txtPatronymic.Text) == false)
                {
                    MessageBox.Show("Недопустимое отчество!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }         

            if (txtEmail.Text == "")
            {
                MessageBox.Show("Пустое поле Email!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

            if (dpDateOfBirth.SelectedDate.HasValue == false || 
                ValidationClass.ValidationDateBirthday(dpDateOfBirth.SelectedDate.Value) == false)
            {
                MessageBox.Show("Ошибка в поле даты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (isEdit)
            {
                var resClick = MessageBox.Show("Изменить пользователя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resClick == MessageBoxResult.No)
                {
                    return;
                }

                try
                {
                    editClient.LastName = txtLastName.Text;
                    editClient.FirstName = txtFirstName.Text;
                    if (txtPatronymic.Text != "")
                    {
                        editClient.Patronymic = txtPatronymic.Text;
                    }
                    editClient.Phone = txtPhone.Text;
                    editClient.Email = txtEmail.Text;
                    editClient.DateOfBirth = dpDateOfBirth.SelectedDate.Value;

                    if (rbMale.IsChecked == true)
                    {
                        editClient.IDGender = 1;
                    }
                    else
                    {
                        editClient.IDGender = 2;
                    }

                    if (pathPhoto != null)
                    {
                        editClient.Photo = pathPhoto;
                    }

                    ClassHelper.AppData.Context.SaveChanges();

                    MessageBox.Show("Пользователь изменен");
                    this.Close();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else
            {
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

                    if (txtPatronymic.Text != "")
                    {
                        newClient.Patronymic = txtPatronymic.Text;
                    }

                    newClient.Phone = txtPhone.Text;
                    newClient.Email = txtEmail.Text;
                    newClient.DateOfBirth = dpDateOfBirth.SelectedDate.Value;
                    newClient.RegistrationDate = DateTime.Now;

                    if (rbMale.IsChecked == true)
                    {
                        newClient.IDGender = 1;
                    }
                    else
                    {
                        newClient.IDGender = 2;
                    }

                    if (pathPhoto != null)
                    {
                        newClient.Photo = pathPhoto;
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
            
        }

        private void btnChoosePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Files|*.jpg;*.jpeg;*.png;";
            if (openFile.ShowDialog() == true)
            {
                photoUser.Source = new BitmapImage(new Uri(openFile.FileName));
                pathPhoto = openFile.FileName;
            }
        }

    }
    
}

