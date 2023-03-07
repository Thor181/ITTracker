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
    /// Логика взаимодействия для ClarifationWindow.xaml
    /// </summary>
    public partial class ClarifationWindow : Window
    {
        private OrderInfo OrderInfo { get; set; } = new OrderInfo();
        private InnerManagerRequests InnerManagerRequests { get; set; } = new InnerManagerRequests();
        private UserControl SenderView { get;}
        private int IdSender { get; }
        private bool IsSpecialistClarify { get; set; } = false;
        private bool IsCustomerRequest { get; set; }
        public ClarifationWindow(OrderInfo orderInfo, EnumsStatus.ClarifationMode sender, UserControl senderView, int idSender = 0)
        {
            InitializeComponent();
            IdSender = idSender;
            OrderInfo = orderInfo;
            SenderView = senderView;
            if (sender == EnumsStatus.ClarifationMode.Customer)
            {
                clarifySpecialistTextbox.IsEnabled = false;
            }
            else
            {
                answerCustomerTextbox.IsEnabled = false;
            }

            if (orderInfo.ClarifationText != null)
            {
                clarifySpecialistTextbox.Text = orderInfo.ClarifationText;
            }
            if (orderInfo.CustomerAnswer != null)
            {
                answerCustomerTextbox.Text = orderInfo.CustomerAnswer;
            }
        }
        public ClarifationWindow(InnerManagerRequests orderInfo, EnumsStatus.ClarifationMode sender, UserControl senderView, bool isCustomerRequest = false, int idSender = 0)
        {
            InitializeComponent();
            IdSender = idSender;
            InnerManagerRequests = orderInfo;
            SenderView = senderView;
            IsCustomerRequest = isCustomerRequest;
            if (sender == EnumsStatus.ClarifationMode.Customer)
            {
                clarifySpecialistTextbox.IsEnabled = false;
            }
            else
            {
                answerCustomerTextbox.IsEnabled = false;
            }

            if (orderInfo.ClarifationText != null)
            {
                clarifySpecialistTextbox.Text = orderInfo.ClarifationText;
            }
            if (orderInfo.CustomerAnswer != null)
            {
                answerCustomerTextbox.Text = orderInfo.CustomerAnswer;
            }
        }
        

        private void sendAnswerClarifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (SenderView is SpecialistView || SenderView is CustomerOrdersView && IsCustomerRequest == false)
            {
                var order = ConnectDb.db.Orders.FirstOrDefault(x => x.Id == OrderInfo.Id);

                if (order != null)
                {
                    order.CustomerAnswer = answerCustomerTextbox.Text;
                    order.ClarifationText = clarifySpecialistTextbox.Text;
                    ConnectDb.db.Update(order);
                    ConnectDb.db.SaveChanges();
                    Close();
                    if (SenderView is SpecialistView specialistView)
                    {
                        specialistView.FillListsSpecialist(IdSender);
                        IsSpecialistClarify = true;
                    }
                }
            }
            else if (SenderView is OrderManagerView || SenderView is CustomerOrdersView  && IsCustomerRequest == true)
            {
                var request = ConnectDb.db.Requests.First(x => x.Id == InnerManagerRequests.Id);
                request.CustomerAnswer = answerCustomerTextbox.Text;
                request.ClarifationText = clarifySpecialistTextbox.Text;
                request.IdStatus = (int)EnumsStatus.StatusOrder.Open;
                ConnectDb.db.Update(request);
                ConnectDb.db.SaveChanges();
                Close();
                if (SenderView is OrderManagerView managerView)
                {
                    managerView.FillListsOpenOrders();
                    IsSpecialistClarify = true;
                }
                else if (SenderView is CustomerOrdersView customerOrdersView && IsCustomerRequest == true)
                {
                    customerOrdersView.UpdateOrdersStackPanel();
                }
            } 
        }

        public new bool ShowDialog()
        {
            base.ShowDialog();
            return IsSpecialistClarify;
        }
    }
}
