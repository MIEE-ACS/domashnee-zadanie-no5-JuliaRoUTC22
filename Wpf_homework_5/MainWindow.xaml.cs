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



namespace Wpf_homework_5
{

    class Tovar
    {
        protected static Random rand = new Random();

        public string Denomination
        {
            set;
            get;
        }

        public int Article
        {
            set;
            get;
        }

        public decimal Price
        {
            set;
            get;
        }

        public string DataOfManufacture
        {
            set;
            get;
        }

        public int Threshold
        {
            set;
            get;
        }

        public override string ToString()
        {
            return $"Наименование: {Denomination}; артикул: {Article}; стоимость: {Price} рублей; дата выпуска: {DataOfManufacture}; срок годности: {Threshold} месяцев.";
        }
    }



    public partial class MainWindow : Window
    {

       List<Tovar> products = new List<Tovar>
       {
        new Tovar() { Denomination = "Товар 1", Article = 19834, Price = 193, DataOfManufacture = "14.10.2019", Threshold = 35 },
        new Tovar() { Denomination = "Товар 2", Article = 83945, Price = 326, DataOfManufacture = "23.04.2017", Threshold = 7 },
        new Tovar() { Denomination = "Товар 3", Article = 21767, Price = 512, DataOfManufacture = "05.11.2018", Threshold = 13 },
        new Tovar() { Denomination = "Товар 4", Article = 35869, Price = 478, DataOfManufacture = "19.06.2020", Threshold = 28 },
       };

        public void updateListBox()
        {
            lb.Items.Clear();
            foreach (var product in products)
            {
                lb.Items.Add(product);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            updateListBox();

        }


        private void Btn_add_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                    Tovar tovar = new Tovar();

                    string d = tb_name.Text.Replace('.', ',');
                    tovar.Denomination = d;

                    int a = Int32.Parse(tb_article.Text.Replace('.', ','));
                    tovar.Article = a;

                    decimal p = Decimal.Parse(tb_price.Text.Replace('.', ','));
                    tovar.Price = p;

                    DateTime y = DateTime.Parse(tb_date.Text.Replace('/', '.'));
                   
                   string s = Convert.ToString(tb_date.Text);
                   s = DateTime.Parse(s).ToShortDateString();
                   tovar.DataOfManufacture = Convert.ToString(s);


                int t = Int32.Parse(tb_storage.Text.Replace('.', ','));
                    tovar.Threshold = t;

                if (y > DateTime.Now)
                {
                    MessageBox.Show("Некорректный ввод. Товар не может быть выпущен позже текущего года.",
                "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                else if (y < new DateTime(1800, 1, 1))
                {
                    MessageBox.Show("А не давний ли товар для 21-го века?",
                "Извините, антиквариаты не продаём.", MessageBoxButton.OK, MessageBoxImage.Question);
                }

                else if ((tb_name.Text == "") || (tb_article.Text == String.Empty) || (tb_price.Text == String.Empty) || (tb_date.Text == String.Empty) || (tb_storage.Text == String.Empty))
                {
                    MessageBox.Show("Пожалуйста, заполните все сведения о товаре, который хотите внести в список.",
                       "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                else if ((a > 0) && (p > 0) && (t > 0))
                {
                    products.Add(tovar);
                    updateListBox();

                    tb_name.Clear();
                    tb_article.Clear();
                    tb_price.Clear();
                    tb_date.Clear();
                    tb_storage.Clear();
                }


                else
                {
                    MessageBox.Show("Некорректный ввод. Все поля должны быть неотрицательными.",
                   "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Некорректный ввод. Пожалуйста, заполните все поля по образцу.",
                    "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Ошибка." + ex.Message,
                    "Неизвестная ошибка.", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Btn_delete_Click(object sender, RoutedEventArgs e)
        {
            if (lb.SelectedIndex >= 0)
                lb.Items.RemoveAt(lb.SelectedIndex);
        }

        private void Btn_inf_Click(object sender, RoutedEventArgs e)
        {     
            if (lb.SelectedItem != null)
            {
                Tovar tovar = lb.SelectedItem as Tovar;
                DateTime dt_now = DateTime.Now;

                DateTime dt_1 = Convert.ToDateTime(tovar.DataOfManufacture);
                var days = (dt_now - dt_1).Days;

                var date1 = DateTime.Now; 
                var date2 = date1.AddDays(days);

                var m = 0;
                var y = 0;

                var day = date2.Day - date1.Day;

                if (day < 0)
                {
                    day = day + DateTime.DaysInMonth(date1.Year, date1.Month);
                    m = 1;
                }

                var months = date2.Month - date1.Month - m;
                if (months < 0)
                {
                    months = months + 12;
                    y = 1;
                }
                var years = date2.Year - date1.Year - y;

                if (((years > 0) && (tovar.Threshold < (years*12 + months))) || ((years == 0) && (months > tovar.Threshold)))
                {
                    MessageBox.Show($"Дата изготовления товара: {tovar.DataOfManufacture:00.00.0000}.\n" +
                        $"Срок хранения товара: {tovar.Threshold} месяцев.\n" +
                        $"Текущая дата и время: {dt_now.ToShortDateString()}.\n" +
                        $"С выпуска товара прошло: {years} лет, {months} месяцев.\n" +
                        $"\nСрок годности истёк {years*12 + months} месяцев назад.",
                        "Свойства", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                else if (((years > 0) && (tovar.Threshold >= (years * 12 + months))) || ((years == 0) && (months <= tovar.Threshold)))
                {
                    MessageBox.Show($"Дата изготовления товара: {tovar.DataOfManufacture:00.00.0000}.\n" +
                        $"Срок хранения товара: {tovar.Threshold} месяцев.\n" +
                        $"Текущая дата и время: {dt_now.ToShortDateString()}.\n" +
                        $"С выпуска товара прошло: {years} лет, {months} месяцев.\n" +
                        $"\nТовар годен к использованию.",
                        "Свойства", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                lb.SelectedItem = null;
            }

            else if (lb.SelectedItem == null)
            {
                MessageBox.Show("Выберите из списка товар, информацию о сроке годности которого хотите увидеть.",
                   "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
    }

}

