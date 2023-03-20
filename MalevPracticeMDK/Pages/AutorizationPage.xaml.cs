using MalevPracticeMDK.Classes;
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
using System.Windows.Threading;

namespace MalevPracticeMDK.Pages
{
    /// <summary>
    /// Логика взаимодействия для AutorizationPage.xaml
    /// </summary>
    public partial class AutorizationPage : Page
    {
        int schet = 0;

        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public AutorizationPage()
        {
            InitializeComponent();

            textBoxLogin.Focus();

            if (schet == 0)
            {
                stackPanelCode.Visibility = Visibility.Collapsed;
            }

            else
            {
                stackPanelCode.Visibility = Visibility.Visible;
            }

            dispatcherTimer.Interval = TimeSpan.FromSeconds(10);

            dispatcherTimer.Tick += new EventHandler(Timer_Trick);
        }

        private void Timer_Trick(object sender, EventArgs e)
        {
            textBoxLogin.IsEnabled = true;

            textBoxLogin.Focus();
        }

        public AutorizationPage(int role)
        {
            InitializeComponent();

            schet = role;

            if (schet == 0)
            {
                stackPanelCode.Visibility = Visibility.Collapsed;
            }

            else
            {
                stackPanelCode.Visibility = Visibility.Visible;
            }

            dispatcherTimer.Interval = TimeSpan.FromSeconds(10);

            dispatcherTimer.Tick += new EventHandler(Timer_Trick);
        }

        public void Autorization()
        {
            User user = BaseClass.malevEntities.User.FirstOrDefault(x => x.UserLogin == textBoxLogin.Text && x.UserPassword == passwordBoxPassword.Password);

            if (user == null)
            {
                if (schet == 1)
                {
                    MessageBox.Show("Не удалось войти. Система входа заблокирована на 10 секунд.");

                    textBoxLogin.IsEnabled = false;

                    textBoxLogin.Text = "";

                    passwordBoxPassword.Password = "";

                    textBoxCode.Text = "";

                    textBoxCode.IsEnabled = false;

                    dispatcherTimer.Start();
                }

                else
                {
                    MessageBox.Show("Не удалось войти.");

                    textBoxLogin.Text = "";

                    passwordBoxPassword.Password = "";

                    schet = 1;

                    FrameClass.frame.Navigate(new AutorizationPage(schet));
                }
            }

            else
            {
                switch (user.UserRole)
                {
                    case 1:

                        MessageBox.Show("Здравствуйте, " + user.UserName + ", вы вошли как клиент.");

                        FrameClass.frame.Navigate(new ShowProductPage(user));

                        break;

                    case 2:

                        MessageBox.Show("Здравствуйте, " + user.UserName + ", вы вошли как менеджер.");

                        FrameClass.frame.Navigate(new ShowProductPage(user));

                        break;

                    case 3:

                        MessageBox.Show("Здравствуйте, " + user.UserName + ", вы вошли как администратор.");

                        FrameClass.frame.Navigate(new ShowProductPage(user));

                        break;
                }
            }
        }

        private void buttonSignIn_Click(object sender, RoutedEventArgs e)
        {
            Autorization();
        }

        private void buttonGuest_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new ShowProductPage());
        }

        private void textBoxLogin_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (schet != 1)
                {
                    Autorization();
                }

                else
                {
                    CAPTCHA();

                    textBoxCode.IsEnabled = true;

                    textBoxCode.Focus();
                }
            }
        }

        private void textBoxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxLogin.Text != "")
            {
                passwordBoxPassword.IsEnabled = true;
            }

            else
            {
                passwordBoxPassword.IsEnabled = false;
            }
        }

        private void passwordBoxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (passwordBoxPassword.Password != "")
            {
                if (schet != 1)
                {
                    buttonSignIn.IsEnabled = true;
                }

                else
                {
                    buttonSignIn.IsEnabled = false;
                }
            }

            else
            {
                buttonSignIn.IsEnabled = false;
            }
        }

        private void passwordBoxPassword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (schet != 1)
                {
                    Autorization();
                }

                else
                {
                    CAPTCHA();

                    textBoxCode.IsEnabled = true;

                    textBoxCode.Focus();
                }
            }
        }

        public void CAPTCHA()
        {
            CaptchaWindow captchaWindow = new CaptchaWindow();

            captchaWindow.ShowDialog(); 
        }

        private void textBoxCode_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBoxCode_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (CaptchaWindow.captcha == textBoxCode.Text)
                {
                    Autorization();
                }
                else
                {
                    MessageBox.Show("Не верный код! Система входа заблокирована на 10 секунд!");

                    textBoxLogin.IsEnabled = false;

                    textBoxLogin.Text = "";

                    passwordBoxPassword.Password = "";

                    textBoxCode.Text = "";

                    textBoxCode.IsEnabled = false;

                    dispatcherTimer.Start();
                }
            }
        }
    }
}
