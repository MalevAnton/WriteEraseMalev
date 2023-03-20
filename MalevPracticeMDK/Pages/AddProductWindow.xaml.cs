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
using System.Windows.Shapes;

namespace MalevPracticeMDK.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        User user;

        List<PartialClass> partialClasses;

        List<PickupPoint> pickupPoints;

        double summa;

        double summaDiscount;

        public AddProductWindow(List<PartialClass> partialClasses, User user)
        {
            InitializeComponent();

            this.user = user;

            this.partialClasses = partialClasses;

            listViewProduct.ItemsSource = partialClasses;

            if (user != null)
            {
                textBlockFIO.Text = "" + user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
            }

            Calculation();

            pickupPoints = BaseClass.malevEntities.PickupPoint.ToList();

            for (int i = 0; i < pickupPoints.Count; i++)
            {
                comboBoxPickupPoint.Items.Add(pickupPoints[i].PickupPointID + ", " + pickupPoints[i].PickupPointIndex + ", " + pickupPoints[i].City.CityName + ", " + pickupPoints[i].Street.StreetName + ", " + pickupPoints[i].PickupPointHouse);
            }

            comboBoxPickupPoint.SelectedIndex = 0;
        }

        private void Calculation()
        {
            summa = 0;

            summaDiscount = 0;

            foreach (PartialClass partialClass in partialClasses)
            {
                summa += partialClass.count * partialClass.product.costWithDiscount;

                summaDiscount += partialClass.count * ((double)partialClass.product.ProductCost - partialClass.product.costWithDiscount);
            }

            textBlockSumma.Text = "Сумма заказа: " + summa.ToString("0.00") + " руб.";

            textBlockSummaDiscount.Text = "Сумма скидки: " + summaDiscount.ToString("0.00") + " руб.";
        }

        private void textBoxCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            string index = textBox.Uid;

            PartialClass partialClass = partialClasses.FirstOrDefault(x => x.product.ProductArcticleNumber == index);

            if (textBox.Text.Replace(" ", "") == "")
            {
                partialClass.count = 0;
            }

            else
            {
                partialClass.count = Convert.ToInt32(textBox.Text);
            }

            if (partialClass.count == 0)
            {
                partialClasses.Remove(partialClass);
            }

            if (partialClasses.Count == 0)
            {
                this.Close();
            }

            listViewProduct.Items.Refresh();

            Calculation();
        }

        private void textBoxCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            string index = btn.Uid;

            PartialClass partialBask = partialClasses.FirstOrDefault(x => x.product.ProductArcticleNumber == index);

            partialClasses.Remove(partialBask);

            if (partialClasses.Count == 0)
            {
                this.Close();
            }

            listViewProduct.Items.Refresh();

            Calculation();
        }

        private void buttonAddProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Order order = new Order();

                int countDay = 0;

                List<Order> orderLast = BaseClass.malevEntities.Order.OrderBy(x => x.OrderID).ToList();

                order.OrderID = orderLast[orderLast.Count - 1].OrderID + 1;

                order.OrderStatus = BaseClass.malevEntities.OrderStatus.FirstOrDefault(x => x.OrderStatusName == "Новый").OrderStatusID;

                order.OrderDate = DateTime.Now;

                if (getDeliveryTime())
                {
                    countDay = 6;
                }

                else
                {
                    countDay = 3;
                }

                order.OrderDeliveryDate = order.OrderDate.AddDays(countDay);

                order.OrderPickupPoint = comboBoxPickupPoint.SelectedIndex + 1;

                if (user != null)
                {
                    order.OrderClient = user.UserID;
                }

                Random rand = new Random();

                string textCode = "";

                for (int i = 0; i < 3; i++)
                {
                    textCode = textCode + rand.Next(10).ToString();
                }

                order.OrderCode = Convert.ToInt32(textCode);

                BaseClass.malevEntities.Order.Add(order);

                BaseClass.malevEntities.SaveChanges();

                foreach (PartialClass partialBask in partialClasses)
                {
                    OrderProduct orderProduct = new OrderProduct();

                    orderProduct.OrderID = order.OrderID;

                    orderProduct.ProductArticleNumber = partialBask.product.ProductArcticleNumber;

                    orderProduct.ProductCount = partialBask.count;

                    BaseClass.malevEntities.OrderProduct.Add(orderProduct);
                }

                BaseClass.malevEntities.SaveChanges();

                MessageBox.Show("Заказ успешно создан");

                TicketWindow ticketWindow = new TicketWindow(order, partialClasses, summa, summaDiscount, countDay);

                ticketWindow.ShowDialog();

                partialClasses.Clear();

                this.Close();
            }

            catch
            {
                MessageBox.Show("При создание заказа возникла ошибка!");
            }
        }

        private bool getDeliveryTime()
        {
            foreach (PartialClass partialClass in partialClasses)
            {
                if (partialClass.product.ProductQuantityStock < 3 || partialClass.product.ProductQuantityStock < partialClass.count)
                {
                    return true;
                }
            }

            return false;
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
