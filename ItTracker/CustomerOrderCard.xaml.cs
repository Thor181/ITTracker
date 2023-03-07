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
    /// Логика взаимодействия для CustomerOrderCard.xaml
    /// </summary>
    public partial class CustomerOrderCard : UserControl
    {
        OrderInfo OrderInfo { get; set; } = new OrderInfo();
        InnerManagerRequests InnerManagerRequests { get; set; } = new InnerManagerRequests();
        private CustomerOrdersView CustomerOrdersView { get; }

        public CustomerOrderCard(OrderInfo orderInfo, CustomerOrdersView customerOrdersView)
        {
            InitializeComponent();
            CustomerOrdersView = customerOrdersView;
            OrderInfo = orderInfo;
            cardContainerDataCard.Content = orderInfo;
            cardCustomerFullNameTextblock.Text = orderInfo.CustomerFullName;
            headerTextblock.Text = orderInfo.Status;
            cardFinalDateTextblock.Text = string.Format($"{OrderInfo.FinalDate:G}");
            cardDescriptionButton.Content = orderInfo.OrderDescription;
            if (orderInfo.IdOrderStatus == (int)EnumsStatus.StatusOrder.Open || orderInfo.IdOrderStatus == (int)EnumsStatus.StatusOrder.InWork)
            {
                cardClarifyInfoButton.IsEnabled = false;
                cardCompleteTaskButton.IsEnabled = false;
            }
            else if (orderInfo.IdOrderStatus == (int)EnumsStatus.StatusOrder.OnClarify)
            {
                cardCompleteTaskButton.IsEnabled = false;
            }
            else if (orderInfo.IdOrderStatus == (int)EnumsStatus.StatusOrder.OnConfirmation)
            {
                cardClarifyInfoButton.IsEnabled = false;
            }
            else if (orderInfo.IdOrderStatus == (int)EnumsStatus.StatusOrder.Closed)
            {
                this.IsEnabled = false;
            }
        }

        public CustomerOrderCard(InnerManagerRequests orderInfo, CustomerOrdersView customerOrdersView)
        {
            InitializeComponent();
            CustomerOrdersView = customerOrdersView;
            InnerManagerRequests = orderInfo;
            cardContainerDataCard.Content = orderInfo;
            cardCustomerFullNameTextblock.Text = orderInfo.CustomerFullName;
            headerTextblock.Text = orderInfo.Status;
            cardFinalDateTextblock.Text = string.Format($"{OrderInfo.FinalDate:G}");
            cardDescriptionButton.Content = orderInfo.OrderDescription;
            if (orderInfo.IdStatus == (int)EnumsStatus.StatusOrder.Open || orderInfo.IdStatus == (int)EnumsStatus.StatusOrder.InWork)
            {
                cardClarifyInfoButton.IsEnabled = false;
                cardCompleteTaskButton.IsEnabled = false;
            }
            else if (orderInfo.IdStatus == (int)EnumsStatus.StatusOrder.OnClarify)
            {
                cardCompleteTaskButton.IsEnabled = false;
            }
            else if (orderInfo.IdStatus == (int)EnumsStatus.StatusOrder.OnConfirmation)
            {
                cardClarifyInfoButton.IsEnabled = false;
            }
            else if (orderInfo.IdStatus == (int)EnumsStatus.StatusOrder.Closed)
            {
                this.IsEnabled = false;
            }
        }

        private void CardDescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrderInfo.OrderDescription != null && string.IsNullOrWhiteSpace(OrderInfo.OrderDescription) == false)
            {
                new FullDescriptionWindow(OrderInfo.OrderDescription ?? string.Empty).ShowDialog();
            }
            else if (OrderInfo.OrderDescription == null)
            {
                new FullDescriptionWindow(InnerManagerRequests.OrderDescription ?? string.Empty).ShowDialog();
            }
        }
        
        private void cardClarifyInfoButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (cardContainerDataCard.Content is InnerManagerRequests innerManagerRequests)
            {
                if (new ClarifationWindow(innerManagerRequests, EnumsStatus.ClarifationMode.Customer, CustomerOrdersView, true).ShowDialog() == false)
                {
                    UpdateStatusOrder(EnumsStatus.StatusOrder.Open);
                    Logging.Log($"{DateTime.Now:G} | Пользователь: {InnerManagerRequests.CustomerFullName} добавил пояснения в заказ ID: {InnerManagerRequests.Id}. Статус заявки: \"Открыто\"");
                }
            }
            else if (cardContainerDataCard.Content is OrderInfo order)
            {
                if (new ClarifationWindow(order, EnumsStatus.ClarifationMode.Customer, CustomerOrdersView).ShowDialog() == false)
                {
                    UpdateStatusOrder(EnumsStatus.StatusOrder.InWork);
                    Logging.Log($"{DateTime.Now:G} | Пользователь: {OrderInfo.CustomerFullName} добавил пояснения в заказ ID: {OrderInfo.Id}. Статус заявки: \"В работе\"");
                }
            }
        }

        private void cardCompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatusOrder(EnumsStatus.StatusOrder.Closed);
            Logging.Log($"{DateTime.Now:G} | Пользователь: {OrderInfo.CustomerFullName} утвердил заказ ID: {OrderInfo.Id}. Статус заказа: \"Закрыт\"");

        }

        private void UpdateStatusOrder(EnumsStatus.StatusOrder statusOrder)
        {
            var order = ConnectDb.db.Orders.FirstOrDefault(x => x.Id == OrderInfo.Id);
            if (order != null)
            {
                order.IdOrderStatus = (int)statusOrder;
                ConnectDb.db.Orders.Update(order);
                ConnectDb.db.SaveChanges();
                CustomerOrdersView.UpdateOrdersStackPanel();
            }
            
        }
    }
}
