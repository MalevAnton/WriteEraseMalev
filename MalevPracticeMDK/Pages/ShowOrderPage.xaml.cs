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

namespace MalevPracticeMDK.Pages
{
    /// <summary>
    /// Логика взаимодействия для ShowOrderPage.xaml
    /// </summary>
    public partial class ShowOrderPage : Page
    {
        User user;

        public ShowOrderPage()
        {
            InitializeComponent();

            CreateFile();
        }

        /// <summary>
        /// второй конструктор 
        /// </summary>
        /// <param name="user"></param>

        public ShowOrderPage(User user)
        {
            InitializeComponent();

            CreateFile();

            this.user = user;
        }

        public void CreateFile()
        {
            listViewOrder.ItemsSource = BaseClass.malevEntities.Order.ToList();

            comboBoxSort.SelectedIndex = 0;

            comboBoxFiltration.SelectedIndex = 0;
        }

        private void comboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtration();
        }

        private void comboBoxFiltration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtration();
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new ShowProductPage(user));
        }

        public void Filtration()
        {
            List<Order> orders = BaseClass.malevEntities.Order.ToList();

            if (comboBoxFiltration.SelectedIndex > 0) // Если фильрация выбрана
            {
                switch (comboBoxFiltration.SelectedIndex)
                {
                    case 1:

                        orders = orders.Where(x => x.DiscountProcent > 0 && x.DiscountProcent < 10).ToList();

                        break;

                    case 2:

                        orders = orders.Where(x => x.DiscountProcent >= 10 && x.DiscountProcent < 15).ToList();

                        break;

                    case 3:

                        orders = orders.Where(x => x.DiscountProcent >= 15).ToList();

                        break;
                }
            }

            if (comboBoxSort.SelectedIndex > 0) // Если выбрана сортировка
            {
                switch (comboBoxSort.SelectedIndex)
                {
                    case 1:

                        orders = orders.OrderBy(x => x.Summa).ToList();

                        break;

                    case 2:

                        orders = orders.OrderByDescending(x => x.Summa).ToList();

                        break;
                }
            }

            listViewOrder.ItemsSource = orders;

            if (orders.Count == 0)
            {
                MessageBox.Show("Данные не найдены");
            }
        }
    }
}
