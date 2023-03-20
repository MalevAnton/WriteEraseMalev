using MalevPracticeMDK.Classes;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
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
using System.Diagnostics;

namespace MalevPracticeMDK.Pages
{
    /// <summary>
    /// Логика взаимодействия для TicketWindow.xaml
    /// </summary>
    public partial class TicketWindow : Window
    {
        Order order;

        List<PartialClass> partialClasses;

        double summa;

        double summaDiscount;

        int countDay;

        public TicketWindow(Order order, List<PartialClass> partialClasses, double summa, double summaDiscount, int countDay)
        {
            InitializeComponent();

            this.order = order;
            
            this.partialClasses = partialClasses;

            this.summa = summa;

            this.summaDiscount = summaDiscount;

            this.countDay = countDay;

            textBlockOrderNumber.Text = textBlockOrderNumber.Text + order.OrderID.ToString();

            textBlockOrderDate.Text = textBlockOrderDate.Text + order.OrderDate.ToString("d");

            foreach (PartialClass pс in partialClasses)
            {
                Product product = BaseClass.malevEntities.Product.FirstOrDefault(x => x.ProductManifacturer == pс.product.ProductProvider);

                OrderProduct productProduct = BaseClass.malevEntities.OrderProduct.FirstOrDefault(x => x.ProductArticleNumber == product.ProductArcticleNumber && x.OrderID == order.OrderID);

                textBlockOrders.Text = textBlockOrders.Text + product.nameProduct;
            }

            textBlockSumma.Text = textBlockSumma.Text + summa.ToString("0.00") + " руб.";

            textBlockSummaDiscount.Text = textBlockSummaDiscount.Text + summaDiscount.ToString("0.00") + " руб.";

            PickupPoint pickupPoint = BaseClass.malevEntities.PickupPoint.FirstOrDefault(x => x.PickupPointID == order.OrderPickupPoint);

            List<PickupPoint> pickupPoints = BaseClass.malevEntities.PickupPoint.Where(x => x.PickupPointID == order.OrderPickupPoint).ToList();

            string citys = "", street = "";

            foreach (PickupPoint pickupPointd in pickupPoints)
            {
                citys = pickupPointd.City.CityName;

                street = pickupPointd.Street.StreetName;
            }

            textBlockOrderPickupPoint.Text = textBlockOrderPickupPoint.Text + order.PickupPoint.PickupPointIndex + ", " + citys + ", " + street + ", " + order.PickupPoint.PickupPointHouse;

            textBlockCode.Text = textBlockCode.Text + order.OrderCode.ToString();
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonPDF_Click(object sender, RoutedEventArgs e)
        {
            PdfDocument document = new PdfDocument();

            int height = 0;

            document.Info.Title = "Талон для получения заказа";

            PdfPage page = document.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont fontHeader = new XFont("Comic Sans MS", 14, XFontStyle.Bold);

            gfx.DrawString("Талон для получения заказа", fontHeader, XBrushes.Black, new XRect(10, height, page.Width, page.Height), XStringFormats.TopCenter);

            XFont font = new XFont("Comic Sans MS", 14);

            height += 30;

            gfx.DrawString("Номер: " + order.OrderID, font, XBrushes.Black, new XRect(10, height, page.Width, page.Height), XStringFormats.TopLeft);

            height += 30;

            gfx.DrawString("Дата заказа: " + order.OrderDate.ToString("D"), font, XBrushes.Black, new XRect(10, height, page.Width, page.Height), XStringFormats.TopLeft);

            height += 30;

            if (countDay == 3)
            {
                gfx.DrawString("Заказ будет готов через 3 дня", font, XBrushes.Black, new XRect(10, height, page.Width, page.Height), XStringFormats.TopLeft);
            }

            else
            {
                gfx.DrawString("Заказ будет готов через 6 дней", font, XBrushes.Black, new XRect(10, height, page.Width, page.Height), XStringFormats.TopLeft);
            }

            height += 30;

            gfx.DrawString("Дата получения заказа: " + order.OrderDeliveryDate.ToString("D"), font, XBrushes.Black, new XRect(10, height, page.Width, page.Height), XStringFormats.TopLeft);

            height += 30;

            gfx.DrawString("Состав заказа: ", font, XBrushes.Black, new XRect(10, height, page.Width, page.Height), XStringFormats.TopLeft);

            foreach (PartialClass pb in partialClasses)
            {
                height += 30;

                Product product = BaseClass.malevEntities.Product.FirstOrDefault(x => x.ProductArcticleNumber == pb.product.ProductArcticleNumber);

                OrderProduct productProduct = BaseClass.malevEntities.OrderProduct.FirstOrDefault(x => x.ProductArticleNumber == product.ProductArcticleNumber && x.OrderID == order.OrderID);

                gfx.DrawString("" + product.NameProduct.Product + " Количество: " + productProduct.ProductCount + ";", font, XBrushes.Black, new XRect(30, height, page.Width, page.Height), XStringFormats.TopLeft);
            }

            height += 30;

            gfx.DrawString("Сумма заказа: " + summa.ToString("0.00") + " руб.", font, XBrushes.Black, new XRect(10, height, page.Width, page.Height), XStringFormats.TopLeft);

            height += 30;

            gfx.DrawString("Сумма скидки: " + summaDiscount.ToString("0.00") + " руб.", font, XBrushes.Black, new XRect(10, height, page.Width, page.Height), XStringFormats.TopLeft);

            height += 30;

            gfx.DrawString("Пункт выдачи: " + order.PickupPoint.PickupPointIndex + ", " + order.PickupPoint.City.CityName + ", " + order.PickupPoint.Street.StreetName + ", " + order.PickupPoint.PickupPointHouse, font, XBrushes.Black, new XRect(10, height, page.Width, page.Height), XStringFormats.TopLeft);

            height += 30;

            gfx.DrawString("Код для получения: " + order.OrderCode, fontHeader, XBrushes.Black, new XRect(10, height, page.Width, page.Height), XStringFormats.TopLeft);
            
            string filename = "TicketPDF.pdf";
            
            document.Save(filename);
            
            Process.Start(filename);
        }
    }
}
