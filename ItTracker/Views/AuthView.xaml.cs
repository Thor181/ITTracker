using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class AuthView : UserControl
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        List<AccessEmployee> AccessEmployees { get; set; }
            = ConnectDb.db.Employees.Join(ConnectDb.db.AccessRoles, x => x.IdRole, y => y.Id, (x, y) => new AccessEmployee
            {
                Id = x.Id,
                Role = y.Role,
                IdRole = x.IdRole,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Password = x.Password
            }).ToList();

        public AuthView()
        {
            InitializeComponent();
            employeesCombobox.ItemsSource = AccessEmployees;
        }
    
        private void enterButton_Click(object sender, RoutedEventArgs e)
        {
            
            var selectedEmployeeId = ((AccessEmployee)employeesCombobox.SelectedItem).IdRole;
            var selectedEmployeeFullName = $"{((AccessEmployee)employeesCombobox.SelectedItem).LastName} {((AccessEmployee)employeesCombobox.SelectedItem).FirstName}";
            var selectedEmployeePassword = ((AccessEmployee)employeesCombobox.SelectedItem).Password;
            if (selectedEmployeePassword == passwordBox.Password)
            {
                mainWindow.MainGrid.Children.Clear();
                switch (selectedEmployeeId)
                {
                    case 1:
                        mainWindow.MainGrid.Children.Add(new OrderManagerView(selectedEmployeeFullName));
                        break;
                    case 2:
                        mainWindow.MainGrid.Children.Add(new SpecialistView(selectedEmployeeId, selectedEmployeeFullName));
                        break;
                    default:
                        break;
                }
            }
            else
            {
                passwordBox.Password = default(string);
            }        
        }

        private void enterAsGuestButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.MainGrid.Children.Clear();
            mainWindow.MainGrid.Children.Add(new CustomerView());
        }
    }
    public class AccessEmployee
    {
        public int Id { get; set; }
        public int IdRole { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Role { get; set; }
        public string? Password { get; set; }
    }
}
