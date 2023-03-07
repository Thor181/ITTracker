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

namespace ITTracker
{
    /// <summary>
    /// Логика взаимодействия для OrderCardManager.xaml
    /// </summary>
    public partial class OrderCardManager : UserControl
    {
        InnerManagerRequests OrderFullInfo { get; set; }
        OrderManagerView OrderManagerView { get; set; }
        private int? IdSenderEmployee { get; set; }
        string EmployeeFullName { get; set; }
        public OrderCardManager(InnerManagerRequests orderInfo, OrderManagerView orderManagerView, string employeeFullName)
        {
            InitializeComponent();
            EmployeeFullName = employeeFullName;
            OrderFullInfo = orderInfo;
            OrderManagerView = orderManagerView;
            FillCard();
        }

        private void FillCard()
        {
            if (OrderFullInfo.OrderDescription != null && string.IsNullOrWhiteSpace(OrderFullInfo.OrderDescription) != true)
            {
                cardCustomerFullNameTextblock.Text = $"{OrderFullInfo.CustomerFullName}";
                cardDescriptionButton.Content = OrderFullInfo.OrderDescription;
                cardContainerDataCard.Content = OrderFullInfo;
            }
        }
        private void cardClarifyInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (new ClarifationWindow(OrderFullInfo, EnumsStatus.ClarifationMode.Specialist, OrderManagerView).ShowDialog() == true)
            {
                UpdateOrder(EnumsStatus.StatusOrder.OnClarify);
                OrderManagerView.FillListsOpenOrders();
                Logging.Log($@"{DateTime.Now:G} | Диспетчер заявок: {EmployeeFullName} отправил заявку ID: {OrderFullInfo.Id} на уточнение");
            }
        }

        private void cardAppointTaskButton_Click(object sender, RoutedEventArgs e)
        {
            new AppointSpecialistWindow(OrderFullInfo, OrderManagerView, EmployeeFullName).ShowDialog();
            

        }

        private void cardDescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrderFullInfo.OrderDescription != null && string.IsNullOrWhiteSpace(OrderFullInfo.OrderDescription) != true)
            {
                new FullDescriptionWindow(OrderFullInfo.OrderDescription ?? string.Empty).ShowDialog();
            }
        }
        private void UpdateOrder(EnumsStatus.StatusOrder status)
        {
            InnerManagerRequests order = (InnerManagerRequests)cardContainerDataCard.Content;
            Request updatedOrder = ConnectDb.db.Requests.First(x => x.Id == order.Id);
            updatedOrder.IdStatus = ((int)status);
            ConnectDb.db.Update(updatedOrder);
            ConnectDb.db.SaveChanges();
        }
    }
}
