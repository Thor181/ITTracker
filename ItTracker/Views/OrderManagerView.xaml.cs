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
    public partial class OrderManagerView : UserControl
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        List<InnerManagerRequests>? RequestsProp { get; set; }
        string EmployeeFullName { get; set; }
        public OrderManagerView(string employeeFullName)
        {
            InitializeComponent();
            FillListsOpenOrders();
            EmployeeFullName = employeeFullName;
        }

        internal void FillListsOpenOrders()
        {
            openOrdersStackPanel.Children.Clear();
            RequestsProp = ConnectDb.db.Requests.Where(x => x.IdStatus == (int)EnumsStatus.StatusOrder.Open)
                .Join(ConnectDb.db.Customers, x => x.IdCustomer, y => y.Id, (x, y) => new InnerManagerRequests
                {
                    Id = x.Id,
                    OrderDescription = x.Description,
                    IdCustomer = x.IdCustomer,
                    CustomerFullName = y.FullName,
                    IdStatus = x.IdStatus,
                    ClarifationText = x.ClarifationText,
                    CustomerAnswer = x.CustomerAnswer
                }).Join(ConnectDb.db.OrderStatuses, x => x.IdStatus, y => y.Id, (x, y) => new InnerManagerRequests
                {
                    Id = x.Id,
                    OrderDescription = x.OrderDescription,
                    IdCustomer = x.IdCustomer,
                    CustomerFullName = x.CustomerFullName,
                    IdStatus = x.IdStatus,
                    ClarifationText = x.ClarifationText,
                    CustomerAnswer = x.CustomerAnswer,
                    Status = y.Status
                }).ToList();
            foreach (var order in RequestsProp)
            {
                openOrdersStackPanel.Children.Add(new OrderCardManager(order, this, EmployeeFullName));
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.MainGrid.Children.Clear();
            mainWindow.MainGrid.Children.Add(new AuthView());
        }
        
    }
    public class InnerManagerRequests
    {
        public int Id { get; set; }
        public string? OrderDescription { get; set; }
        public int IdCustomer { get; set; }
        public string? CustomerFullName { get; set; }
        public int IdStatus { get; set; }
        public string? ClarifationText { get; set; }
        public string? CustomerAnswer { get; set; }
        public string? Status { get; set; }
    }
}
