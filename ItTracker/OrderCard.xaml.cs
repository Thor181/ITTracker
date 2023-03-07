using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace ITTracker
{
    /// <summary>
    /// Логика взаимодействия для OrderCard.xaml
    /// </summary>
    public partial class OrderCard : UserControl
    { 
        OrderInfo OrderFullInfo { get; set; }
        SpecialistView SpecialistViewProp { get; set; }
        private int IdSenderEmployee {get; set;}
        string EmployeeFullName { get; set; }
        public OrderCard(OrderInfo orderInfo, SpecialistView specialistView, int idSenderEmployee, string employeeFullName)
        {
            InitializeComponent();
            IdSenderEmployee = idSenderEmployee;
            OrderFullInfo = orderInfo;
            SpecialistViewProp = specialistView;
            EmployeeFullName = employeeFullName;
            FillCard();
        }

        private void FillCard()
        {
            if (OrderFullInfo.OrderDescription != null && string.IsNullOrWhiteSpace(OrderFullInfo.OrderDescription) != true)
            {
                cardCustomerFullNameTextblock.Text = $"{OrderFullInfo.CustomerFullName}";
                cardDescriptionButton.Content = OrderFullInfo.OrderDescription;
                cardFinalDateTextblock.Text = string.Format($"{OrderFullInfo.FinalDate:G}");
                cardContainerDataCard.Content = OrderFullInfo;
            }  
        }

        private void cardDescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrderFullInfo.OrderDescription != null && string.IsNullOrWhiteSpace(OrderFullInfo.OrderDescription) != true)
            {
                new FullDescriptionWindow(OrderFullInfo.OrderDescription ?? string.Empty).ShowDialog();
            }  
        }

        private void cardCompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateOrder(EnumsStatus.StatusOrder.OnConfirmation);
            Logging.Log($@"{DateTime.Now:G} | Специалист: {EmployeeFullName} отправил заказ ID: {OrderFullInfo.Id} на утверждение");
        }

        private void cardClarifyInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (new ClarifationWindow(OrderFullInfo, EnumsStatus.ClarifationMode.Specialist, SpecialistViewProp, IdSenderEmployee).ShowDialog() != false)
            {
                UpdateOrder(EnumsStatus.StatusOrder.OnClarify);
                Logging.Log($@"{DateTime.Now:G} | Специалист: {EmployeeFullName} отправил заказ ID: {OrderFullInfo.Id} на уточнение");
            }
        }

        private void UpdateOrder(EnumsStatus.StatusOrder status)
        {
            OrderInfo order = (OrderInfo)cardContainerDataCard.Content;
            Order updatedOrder = ConnectDb.db.Orders.First(x => x.Id == order.Id);
            updatedOrder.IdOrderStatus = ((int)status);
            ConnectDb.db.Update(updatedOrder);
            ConnectDb.db.SaveChanges();
            SpecialistViewProp.FillListsSpecialist(OrderFullInfo.IdEmployee);
        }
    }
}
