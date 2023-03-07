using Microsoft.EntityFrameworkCore;
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
    /// Логика взаимодействия для CustomerOrdersView.xaml
    /// </summary>
    public partial class CustomerOrdersView : UserControl
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        List<OrderInfo> Orders = new List<OrderInfo>();
        List<InnerManagerRequests> Requests = new List<InnerManagerRequests>();
        public CustomerOrdersView()
        {
            InitializeComponent();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.MainGrid.Children.Clear();
            mainWindow.MainGrid.Children.Add(new CustomerView());
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateOrdersStackPanel();
        }

        internal void UpdateOrdersStackPanel()
        {
            customerOrdersStackPanel.Children.Clear();
            Orders = ConnectDb.db.Orders.Join(ConnectDb.db.OrderStatuses, x => x.IdOrderStatus, y => y.Id, (x, y) => new
            {
                x.Id,
                x.IdOrderStatus,
                y.Status,
                x.IdService,
                x.IdRequest,
                x.IdEmployee,
                x.IdCustomer,
                x.Date,
                x.Price,
                x.ClarifationText,
                x.CustomerAnswer
            }).Join(ConnectDb.db.Services, x => x.IdService, y => y.Id, (x, y) => new
            {
                x.Id,
                x.IdOrderStatus,
                x.IdService,
                x.Status,
                y.Name,
                x.IdRequest,
                x.IdEmployee,
                x.IdCustomer,
                x.Date,
                x.Price,
                x.ClarifationText,
                x.CustomerAnswer
            }).Join(ConnectDb.db.Requests, x => x.IdRequest, y => y.Id, (x, y) => new
            {
                Id = x.Id,
                IdOrderStatus = x.IdOrderStatus,
                Status = x.Status,
                IdService = x.IdService,
                Service = x.Name,
                IdRequest = x.IdRequest,
                IdEmployee = x.IdEmployee,
                x.IdCustomer,
                OrderDescription = y.Description,
                FinalDate = x.Date,
                Price = x.Price,
                x.ClarifationText,
                x.CustomerAnswer
            }).Join(ConnectDb.db.Customers, x => x.IdCustomer, y => y.Id, (x, y) => new OrderInfo
            {
                Id = x.Id,
                IdOrderStatus = x.IdOrderStatus,
                Status = x.Status,
                IdService = x.IdService,
                Service = x.Service,
                IdRequest = x.IdRequest,
                CustomerFullName = y.FullName,
                IdEmployee = x.IdEmployee,
                IdCustomer = x.IdCustomer,
                OrderDescription = x.OrderDescription,
                FinalDate = x.FinalDate,
                Price = x.Price,
                ClarifationText = x.ClarifationText,
                CustomerAnswer = x.CustomerAnswer
            }).Where(x => x.IdCustomer.ToString() == searchTextbox.Text).ToList();
            foreach (var order in Orders.OrderByDescending(x => x.Id))
            {
                customerOrdersStackPanel.Children.Add(new CustomerOrderCard(order, this));
            }
            Requests = ConnectDb.db.Requests.Where(x => x.IdCustomer.ToString() == searchTextbox.Text)
                .Join(ConnectDb.db.Customers, x => x.IdCustomer, y => y.Id, (x, y) => new InnerManagerRequests
                {
                    Id = x.Id,
                    OrderDescription = x.Description,
                    IdCustomer = x.IdCustomer,
                    CustomerFullName = y.FullName,
                    IdStatus = x.IdStatus,
                    ClarifationText = x.ClarifationText,
                    CustomerAnswer = x.CustomerAnswer
                }).Join(ConnectDb.db.OrderStatuses, x => x.IdStatus, y => y.Id, (x,y) => new InnerManagerRequests
                {
                    Id = x.Id,
                    OrderDescription = x.OrderDescription,
                    IdCustomer = x.IdCustomer,
                    CustomerFullName = x.CustomerFullName,
                    IdStatus = x.IdStatus,
                    ClarifationText = x.ClarifationText,
                    CustomerAnswer = x.CustomerAnswer,
                    Status = y.Status,
                }).ToList();
            foreach (var request in Requests)
            {
                customerOrdersStackPanel.Children.Add(new CustomerOrderCard(request, this));
            }
        }
    }
}
