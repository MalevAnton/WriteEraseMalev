using MalevPracticeMDK.Classes;
using MalevPracticeMDK.Pages;
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

namespace MalevPracticeMDK
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int role = 0;

        public MainWindow()
        {
            InitializeComponent();

            BaseClass.malevEntities = new MalevEntities();

            FrameClass.frame = frame;

            FrameClass.frame.Navigate(new AutorizationPage(role));
        }
    }
}
