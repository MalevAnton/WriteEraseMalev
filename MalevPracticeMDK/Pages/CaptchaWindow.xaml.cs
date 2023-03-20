using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для CaptchaWindow.xaml
    /// </summary>
    public partial class CaptchaWindow : Window
    {
        public static string captcha;

        public CaptchaWindow()
        {
            InitializeComponent();

            Regex regex1 = new Regex("[0-9]+");

            Regex regex2 = new Regex("[A-z]+");

            metka: captcha = "";

            Random random = new Random();

            string captch = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            string[] captc = new string[4];


            for (int i = 0; i < 4; i++)
            {
                captc[i] = Convert.ToString(captch[random.Next(captch.Length)]);

                captcha += captc[i];
            }

            bool regex11 = regex1.IsMatch(captcha);

            bool regex22 = regex2.IsMatch(captcha);


            if (regex11)
            {
                if (regex22)
                {
                    goto met;
                }

                else
                {
                    goto metka;
                }
            }

            else
            {
                goto metka;
            }

            met:

            TextBlock textBlock = new TextBlock()
            {
                Text = Convert.ToString(captc[0].ToString()),

                Margin = new Thickness(10),

                Padding = new Thickness(25),

                FontSize = random.Next(13, 18),

                FontStyle = FontStyles.Italic,

                TextDecorations = TextDecorations.Strikethrough
            };

            canvas1.Children.Add(textBlock);

            TextBlock textBlock1 = new TextBlock()
            {
                Text = Convert.ToString(captc[1].ToString()),

                Margin = new Thickness(10),

                Padding = new Thickness(25),

                FontSize = random.Next(13, 18),

                FontStyle = FontStyles.Italic,

                FontWeight = FontWeights.Bold,

                TextDecorations = TextDecorations.Strikethrough
            };

            canvas2.Children.Add(textBlock1);

            TextBlock textBlock2 = new TextBlock()
            {
                Text = Convert.ToString(captc[2].ToString()),

                Margin = new Thickness(10),

                Padding = new Thickness(25),

                FontSize = random.Next(13, 18),

                FontWeight = FontWeights.Bold,

                TextDecorations = TextDecorations.Strikethrough
            };

            canvas3.Children.Add(textBlock2);

            TextBlock textBlock3 = new TextBlock()
            {

                Text = Convert.ToString(captc[3].ToString()),

                Margin = new Thickness(10),

                Padding = new Thickness(25),

                FontSize = random.Next(13, 18),

                FontStyle = FontStyles.Italic,

                FontWeight = FontWeights.Bold,

                TextDecorations = TextDecorations.Strikethrough
            };

            canvas4.Children.Add(textBlock3);

            Line line1 = new Line()
            {
                X1 = random.Next(225),

                Y1 = random.Next(125),

                Stroke = Brushes.Violet,

                StrokeThickness = random.Next(2, 7),
            };

            canvas.Children.Add(line1);

            Line line2 = new Line()
            {
                X1 = random.Next(225),

                Y1 = random.Next(125),

                Stroke = Brushes.SpringGreen,

                StrokeThickness = random.Next(2, 7),
            };

            canvas.Children.Add(line2);

            Line line3 = new Line()
            {
                X1 = random.Next(-325, 0),

                Y1 = random.Next(10, 70),

                Stroke = Brushes.SteelBlue,

                StrokeThickness = random.Next(2, 7),
            };

            canvas.Children.Add(line3);

            Line l4 = new Line()
            {
                X1 = random.Next(355, 399),

                Y1 = random.Next(40, 100),

                Stroke = Brushes.Tan,

                StrokeThickness = random.Next(2, 7),
            };

            canvas.Children.Add(l4);

            Line line5 = new Line()
            {
                X1 = random.Next(-225, 0),

                Y1 = random.Next(125),

                Stroke = Brushes.Tomato,

                Fill = Brushes.Tomato,

                StrokeThickness = random.Next(2, 7),
            };

            canvas.Children.Add(line5);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
