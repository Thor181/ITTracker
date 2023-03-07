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
    /// Логика взаимодействия для CustomerView.xaml
    /// </summary>
    public partial class SpecialistView : UserControl
    {
        List<OrderInfo>? OrdersFullInfo { get; set; }
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        string EmployeeFullName {get; set; }
        public SpecialistView(int idEmployee, string employeeFullName)
        {
            InitializeComponent();
            FillListsSpecialist(idEmployee);
            EmployeeFullName = employeeFullName;
        }
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.MainGrid.Children.Clear();
            mainWindow.MainGrid.Children.Add(new AuthView());
        }
        internal void FillListsSpecialist(int idEmployee)
        {
            workOrdersStackPanel.Children.Clear();
            confirmationOrdersStackPanel.Children.Clear();
            closedOrdersStackPanel.Children.Clear();

            #region LinqOrderToThisID
            OrdersFullInfo = ConnectDb.db.Orders.Where(x => x.IdEmployee == idEmployee)
                .Join(ConnectDb.db.OrderStatuses, x => x.IdOrderStatus, y => y.Id, (x, y) => new
                {
                    x.Id,
                    x.IdOrderStatus,
                    y.Status,
                    x.IdService,
                    x.IdRequest,
                    x.IdEmployee,
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
                    y.IdCustomer,
                    OrderDescription = y.Description,
                    FinalDate = x.Date,
                    Price = x.Price,
                    x.ClarifationText,
                    x.CustomerAnswer
                }).Join(ConnectDb.db.Customers, x => x.IdCustomer, y => y.Id, (x,y) => new OrderInfo
                {
                    Id = x.Id,
                    IdOrderStatus = x.IdOrderStatus,
                    Status = x.Status,
                    IdService = x.IdService,
                    Service = x.Service,
                    IdRequest = x.IdRequest,
                    IdEmployee = x.IdEmployee,
                    IdCustomer = x.IdCustomer,
                    CustomerFullName = y.FullName,
                    OrderDescription = x.OrderDescription,
                    FinalDate = x.FinalDate,
                    Price = x.Price,
                    ClarifationText = x.ClarifationText,
                    CustomerAnswer = x.CustomerAnswer
                }).ToList();
            #endregion

            foreach (var order in OrdersFullInfo.Where(x => x.IdOrderStatus == (int)EnumsStatus.StatusOrder.Open 
                                                         || x.IdOrderStatus == (int)EnumsStatus.StatusOrder.InWork))
            {
                workOrdersStackPanel.Children.Add(new OrderCard(order, this, idEmployee, EmployeeFullName));
            }
            foreach (var order in OrdersFullInfo.Where(x => x.IdOrderStatus == (int)EnumsStatus.StatusOrder.OnConfirmation))
            {
                confirmationOrdersStackPanel.Children.Add(new OrderCard(order, this, idEmployee, EmployeeFullName) { IsEnabled = false });
            }
            foreach (var order in OrdersFullInfo.Where(x => x.IdOrderStatus == (int)EnumsStatus.StatusOrder.Closed))
            {
                closedOrdersStackPanel.Children.Add(new OrderCard(order, this, idEmployee, EmployeeFullName) { IsEnabled = false });
            }
        }
    }
}
