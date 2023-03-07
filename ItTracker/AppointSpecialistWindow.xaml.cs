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
using System.Windows.Shapes;

namespace ITTracker
{
    /// <summary>
    /// Логика взаимодействия для AppointSpecialistWindow.xaml
    /// </summary>
    public partial class AppointSpecialistWindow : Window
    {
        InnerManagerRequests Request { get; set; }
        OrderManagerView OrderManagerView { get; set; }
        string EmployeeFullName { get; set; }
        public AppointSpecialistWindow(InnerManagerRequests request, OrderManagerView orderManagerView, string employeeFullName)
        {
            InitializeComponent();
            Request = request;
            EmployeeFullName = employeeFullName;
            OrderManagerView = orderManagerView;
            specialistsCombobox.ItemsSource = ConnectDb.db.Employees.Where(x => x.IdRole == 2).AsNoTracking().ToList();
            serviceCombobox.ItemsSource = ConnectDb.db.Services.AsNoTracking().ToList();
        }

        private void appointButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                ConnectDb.db.Add(new Order()
                {
                    IdOrderStatus = (int)EnumsStatus.StatusOrder.Open,
                    IdService = ((Service)serviceCombobox.SelectedItem).Id,
                    IdRequest = Request.Id,
                    IdEmployee = ((Employee)specialistsCombobox.SelectedItem).Id,
                    Date = finalDatePicker.SelectedDate ?? DateTime.Now,
                    Price = decimal.Parse(priceTextbox.Text),
                    ClarifationText = null,
                    IdCustomer = Request.IdCustomer,
                    CustomerAnswer = null
                });
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                ConnectDb.db.SaveChanges();
                var requestBb = ConnectDb.db.Requests.FirstOrDefault(x => x.Id == Request.Id);
                if (requestBb != null)
                {
                    requestBb.IdStatus = (int)EnumsStatus.StatusOrder.Closed;
                    ConnectDb.db.Update(requestBb);
                    ConnectDb.db.SaveChanges();
                    Logging.Log($@"{DateTime.Now:G} | Диспетчер заявок: {EmployeeFullName} назначил специалиста {((Employee)specialistsCombobox.SelectedItem).LastName} {((Employee)specialistsCombobox.SelectedItem).FirstName} на заказ ID: {requestBb.Id}");
                }
                OrderManagerView.FillListsOpenOrders();
                
                Close();
            }
        }
    }
}
