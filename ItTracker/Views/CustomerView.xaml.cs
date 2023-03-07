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
    public partial class CustomerView : UserControl
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public CustomerView()
        {
            InitializeComponent();
        }

        private void sendRequestButton_Click(object sender, RoutedEventArgs e)
        {
            string customerFullName = customerFullNameTextbox.Text;
            string descriptionOrder = descriptionTextbox.Text;
            if (string.IsNullOrWhiteSpace(customerFullName) != true && string.IsNullOrWhiteSpace(descriptionOrder) != true)
            {
                int idCustomer = 0;
                if (ConnectDb.db.Customers.Any(x => x.FullName == customerFullName) != true)
                {
                    ConnectDb.db.Add(new Customer()
                    {
                        FullName = customerFullNameTextbox.Text
                    });
                    ConnectDb.db.SaveChanges();
                    idCustomer = ConnectDb.db.Customers.OrderBy(x => x.Id).Last().Id;
                }
                else
                {
                    idCustomer = ConnectDb.db.Customers.Where(x => x.FullName == customerFullName).First().Id;
                }
                ConnectDb.db.Add(new Request()
                {
                    IdCustomer = idCustomer,
                    Description = descriptionTextbox.Text,
                    IdStatus = (int)EnumsStatus.StatusOrder.Open
                });
                ConnectDb.db.SaveChanges();
                descriptionTextbox.Text = $"Ваш Id: {idCustomer}";
                Logging.Log($"{DateTime.Now:G} | Пользователь: {customerFullName} создал заявку ID: {ConnectDb.db.Requests.OrderBy(x => x.Id).Last().Id}");

            }
            
        }

        private void checkOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.MainGrid.Children.Clear();
            mainWindow.MainGrid.Children.Add(new CustomerOrdersView());
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.MainGrid.Children.Clear();
            mainWindow.MainGrid.Children.Add(new AuthView());
        }
    }
}
