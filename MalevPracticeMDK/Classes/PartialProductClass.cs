using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MalevPracticeMDK
{
    public partial class Product
    {
        public SolidColorBrush colorBackground
        {
            get
            {
                if (ProductDiscountAmount > 15)
                {
                    SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFromString("#7fff00");

                    return color;
                }

                return Brushes.White;
            }
        }

        public double costWithDiscount
        {
            get
            {
                if (ProductDiscountAmount != null)
                {
                    return (double)(Convert.ToDouble(ProductCost) - (Convert.ToDouble(ProductCost) * ProductDiscountAmount / 100));
                }

                else
                {
                    return 0;
                }

            }
        }

        public string costWithDiscountString
        {
            get
            {
                if (ProductDiscountAmount != null)
                {
                    return costWithDiscount.ToString("0.00");
                }

                else
                {
                    return "";
                }
            }
        }

        public string nameProduct
        {
            get
            {
                return NameProduct.Product;
            }
        }

        public string textDecoration
        {
            get
            {
                if (ProductDiscountAmount != null)
                {
                    return "Strikethrough";
                }

                else
                {
                    return "Baseline";
                }
            }
        }
    }
}
