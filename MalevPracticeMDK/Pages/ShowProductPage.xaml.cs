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
    /// Логика взаимодействия для ShowProductPage.xaml
    /// </summary>
    public partial class ShowProductPage : Page
    {
        List<PartialClass> partialClasses = new List<PartialClass>();

        User user;

        public ShowProductPage()
        {
            InitializeComponent();

            CreateFile();
        }

        public ShowProductPage(User user)
        {
            InitializeComponent();

            this.user = user;

            CreateFile();

            textBlockFIO.Text = "" + user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;

            if (user.Role.RoleName == "Менеджер" || user.Role.RoleName == "Администратор")
            {
                buttonOformlenie.Visibility = Visibility.Visible;
            }
        }

        public void CreateFile()
        {
            listViewProduct.ItemsSource = BaseClass.malevEntities.Product.ToList();

            comboBoxFiltration.SelectedIndex = 0;

            comboBoxSorting.SelectedIndex = 0;

            textBlockCountProduct.Text = "" + BaseClass.malevEntities.Product.ToList().Count() + " из " + BaseClass.malevEntities.Product.ToList().Count();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new AutorizationPage());
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filtration();
        }

        private void comboBoxSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtration();
        }

        private void comboBoxFiltration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtration();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы хотите удалить элемент?", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Button button = (Button)sender;

                    string index = button.Uid;

                    Product product = BaseClass.malevEntities.Product.FirstOrDefault(x => x.ProductArcticleNumber == index);

                    List<OrderProduct> orderProducts = BaseClass.malevEntities.OrderProduct.Where(x => x.ProductArticleNumber == index).ToList();

                    if (orderProducts.Count == 0)
                    {
                        foreach (OrderProduct orderProduct in orderProducts)
                        {
                            BaseClass.malevEntities.OrderProduct.Remove(orderProduct);
                        }

                        BaseClass.malevEntities.Product.Remove(product);

                        BaseClass.malevEntities.SaveChanges();

                        FrameClass.frame.Navigate(new ShowProductPage(user));
                    }

                    else
                    {
                        MessageBox.Show("Нельзя удалить товар, он указан в заказе.");
                    }
                }
            }

            catch
            {
                MessageBox.Show("Oшибка");
            }
        }

        private void buttonDelete_Loaded(object sender, RoutedEventArgs e)
        {
            if (user == null)
            {
                return;
            }

            Button buttonDelete = sender as Button;

            if (user.Role.RoleName == "Менеджер" || user.Role.RoleName == "Администратор")
            {
                buttonDelete.Visibility = Visibility.Visible;
            }

            else
            {
                buttonDelete.Visibility = Visibility.Collapsed;
            }
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            Product product = (Product)listViewProduct.SelectedItem;
            bool s = false;
            foreach (PartialClass partialClass in partialClasses)
            {
                if (partialClass.product == product)
                {
                    partialClass.count = partialClass.count += 1;

                    s = true;
                }
            }

            if (!s)
            {
                PartialClass partialClasse = new PartialClass();

                partialClasse.product = product;

                partialClasse.article = product.ProductArcticleNumber;

                partialClasse.count = 1;

                partialClasses.Add(partialClasse);
            }

            buttonShow.Visibility = Visibility.Visible;
        }

        private void buttonShow_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow addProductWindow = new AddProductWindow(partialClasses, user);

            addProductWindow.ShowDialog();

            if (partialClasses.Count == 0)
            {
                buttonShow.Visibility = Visibility.Collapsed;
            }
        }

        private void buttonOformlenie_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new ShowOrderPage(user));
        }

        public void Filtration()
        {
            List<Product> products = BaseClass.malevEntities.Product.ToList();

            if (textBoxSearch.Text.Length > 0)
            {
                products = products.Where(x => x.NameProduct.Product.ToLower().Contains(textBoxSearch.Text.ToLower())).ToList();
            }

            if (comboBoxFiltration.SelectedIndex > 0)
            {
                switch (comboBoxFiltration.SelectedIndex)
                {
                    case 1:

                        products = products.Where(x => x.ProductDiscountAmount > 0 && x.ProductDiscountAmount < 9.99).ToList();

                        break;

                    case 2:

                        products = products.Where(x => x.ProductDiscountAmount > 10 && x.ProductDiscountAmount < 14.99).ToList();

                        break;

                    case 3:

                        products = products.Where(x => x.ProductDiscountAmount > 15).ToList();

                        break;
                }
            }

            if (comboBoxSorting.SelectedIndex > 0)
            {
                switch (comboBoxSorting.SelectedIndex)
                {
                    case 1:

                        products = products.OrderBy(x => x.costWithDiscount).ToList();

                        break;

                    case 2:

                        products = products.OrderByDescending(x => x.costWithDiscount).ToList();

                        break;
                }
            }

            listViewProduct.ItemsSource = products;

            if (products.Count == 0)
            {
                MessageBox.Show("Данные не найдены");
            }

            textBlockCountProduct.Text = "" + products.Count() + " из " + BaseClass.malevEntities.Product.ToList().Count();
        }
    }
}
