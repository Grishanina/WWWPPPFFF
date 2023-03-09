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

namespace WWWPPPFFF
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {

        List<Agent> AgFilter = new List<Agent>();
        public Page1()
        {
            InitializeComponent();
            AgFilter = ClassBase.BD.Agent.ToList();
            listProduct.ItemsSource = ClassBase.BD.Agent.ToList();

            List<AgentType> TT = ClassBase.BD.AgentType.ToList();

            // программное заполнение выпадающего списка
            cmbType.Items.Add("Все типы");  // первый элемент выпадающего списка, который сбрасывает фильтрацию
            for (int i = 0; i < TT.Count; i++)  // цикл для записи в выпадающий список всех типов оваров из БД
            {
                cmbType.Items.Add(TT[i].Title);
            }

            cmbType.SelectedIndex = 0;  // выбранное по умолчанию значение в списке с типами товаров ("Все типы товаров")
            cmbSort.SelectedIndex = 0;  // выбранное по умолчанию значение в списке с видами сортировки ("Без сортировки")

            tbCount.Text = "По данным запросам найдено количество записей: " + ClassBase.BD.Agent.ToList().Count;
        }

        void Filter()  // метод для одновременной фильтрации, поиска и сортировки
        {
            List<Agent> productList = new List<Agent>();  // пустой список, который далее будет заполнять элементами, удавлетворяющими условиям фильтрации, поиска и сортировки

            string type = cmbType.SelectedValue.ToString();  // выбранное пользователем название типа
            int index = cmbType.SelectedIndex;

            // поиск значений, удовлетворяющих условия фильтра
            if (index != 0)
            {
                productList = ClassBase.BD.Agent.Where(x => x.AgentType.Title == type).ToList();
            }
            else  // если выбран пункт "Все типы", то сбрасываем фильтрацию:
            {
                productList = ClassBase.BD.Agent.ToList();
            }


            // поиск совпадений по названию продукта
            if (!string.IsNullOrWhiteSpace(tbSearch.Text))  // если строка не пустая и если она не состоит из пробелов
            {
                productList = productList.Where(x => x.Title.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
            }


            // сортировка
            switch (cmbSort.SelectedIndex)
            {
                case 1:
                    {
                        productList.Sort((x, y) => x.Priority.CompareTo(y.Priority));
                    }
                    break;
                case 2:
                    {
                        productList.Sort((x, y) => x.Priority.CompareTo(y.Priority));
                        productList.Reverse();
                    }
                    break;
            }

            listProduct.ItemsSource = productList;
            if (productList.Count == 0)
            {
                MessageBox.Show("нет записей");
            }
            tbCount.Text = "По данным запросам найдено количество записей: " + productList.Count;
        }

        private void tbSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;  // получаем доступ к Button из шаблона
            int index = Convert.ToInt32(btn.Uid);

            Agent agent = ClassBase.BD.Agent.FirstOrDefault(x => x.ID == index);

            ClassBase.BD.Agent.Remove(agent); // удаление кота из базы            
            ClassBase.BD.SaveChanges();  // сохранение изменений в базе данных

            ClassFrame.Mfrm.Navigate(new Page1());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;  // получаем доступ к Button из шаблона
            int index = Convert.ToInt32(btn.Uid);   // получаем числовой Uid элемента списка (в разметке предварительно нужно связать номер ячейки с номером кота в базе данных)

            // создаем объект, который содержит кота, информацию о котором нужно отредактировать
            Agent agent = ClassBase.BD.Agent.FirstOrDefault(x => x.ID == index);

            // переход на страницу с редактированием (на ту же самую, где и добавляли кота)
            ClassFrame.Mfrm.Navigate(new PageCreate(agent));
        }
    }
}
