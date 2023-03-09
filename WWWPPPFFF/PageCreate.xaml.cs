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
    /// Логика взаимодействия для PageCreate.xaml
    /// </summary>
    public partial class PageCreate : Page
    {

        Agent PRO;  // объект, в котором будет хранится данные о новом или отредактированном продукте
        bool flagUpdate = false; // для определения, создаем мы новый объект или редактируем старый


        public void uploadFields()  // метод для заполнения списков
        {
            cmbType.ItemsSource = ClassBase.BD.AgentType.ToList();
            cmbType.SelectedValuePath = "ID";
            cmbType.DisplayMemberPath = "Title";
        }
        public PageCreate(Agent agent)
        {
            InitializeComponent();
            uploadFields();
            flagUpdate = true;
            PRO = agent;
            tbName.Text = agent.Title;
            cmbType.SelectedIndex = agent.AgentTypeID - 1;
            tbPriority.Text = Convert.ToString(agent.Priority);
        }


        public PageCreate()
        {
            InitializeComponent();
            uploadFields();
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
			try
			{

				// если флаг равен false, то создаем объект для добавления 
				if (flagUpdate == false)
				{
					PRO = new Agent();
				}

				// заполняем поля таблицы 
				PRO.Title = tbName.Text;
				PRO.AgentTypeID = cmbType.SelectedIndex + 1;
				PRO.Priority = Convert.ToInt32(tbPriority.Text);

				// если флаг равен false, то добавляем объект в базу
				if (flagUpdate == false)
				{
					ClassBase.BD.Agent.Add(PRO);
				}

				ClassBase.BD.SaveChanges();
				MessageBox.Show("Информация добавлена");
			}
			catch
			{
				MessageBox.Show("ОШИБКА");
			}

		}

        private void btnHom_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.Mfrm.Navigate(new Page1());
        }
    }
}
