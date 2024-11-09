using DataAccess.Models;
using Repositories.Repos.Interfaces;
using Repositories.Repos;
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
using SalesWPFApp.MemberManager;

namespace SalesWPFApp.OrderManager
{
	/// <summary>
	/// Interaction logic for OrderManagePage.xaml
	/// </summary>
	public partial class OrderManagePage : Window
	{
		private IOrderRepository _orderRepository;
		private IMemberRepository _memberRepository;
		private List<Order> _orders;
		private Member _loggedInMember;

		public OrderManagePage()
		{
			if (_orderRepository == null) _orderRepository = new OrderRepository();
			if (_memberRepository == null) _memberRepository = new MemberRepository();
			InitializeComponent();

			RefreshDataGrid();
			this.Title = "Order Manager";
		}

		public OrderManagePage(Member loggedInMember)
		{
			if (_orderRepository == null) _orderRepository = new OrderRepository();
			if (_memberRepository == null) _memberRepository = new MemberRepository();
			InitializeComponent();

			RefreshDataGrid();
			this.Title = "Order Manager";

			_loggedInMember = loggedInMember;
			if (!_loggedInMember.IsAdmin) DisableAdminButtons();
		}

		public void DisableAdminButtons()
		{
			Btn_MemberManagePage.IsEnabled = false;
		}


		private void RefreshDataGrid()
		{
			_orders = _orderRepository.GetAll();
			Dg_Order.ItemsSource = _orders;
			Dg_Order.SelectedIndex = 0;
		}


		private void Btn_Create_Click(object sender, RoutedEventArgs e)
		{

			try
			{
				if (!ValidateField()) return;

				var order = CreateOrderFromTextBox();
				if (order == null) return; // TODO: error message

				MessageBoxResult popup = MessageBox.Show("Do you want to create a new order using the credentials above?", "Create Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
				if (popup == MessageBoxResult.No) return;

				bool result = _orderRepository.Add(order);

				if (!result)
				{
					MessageBox.Show("Create failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
				MessageBox.Show("Member created", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
				RefreshDataGrid();


			} catch (Exception ex) 
			{
				return;
			}
			
		}

		private void Btn_Update_Click(object sender, RoutedEventArgs e)
		{
			
			try
			{
				if (!ValidateField()) return;

				var order = CreateOrderFromTextBox();
				if (order == null) return; // TODO: error message

				MessageBoxResult popup = MessageBox.Show($"Update the Order ID {Txt_Id.Text} using the credentials above?", "Update Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
				if (popup == MessageBoxResult.No) return;

				

				bool result = _orderRepository.Update(order);

				if (!result)
				{
					MessageBox.Show("Update Failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
				MessageBox.Show("Order Updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
				RefreshDataGrid();
			}
			catch (Exception ex)
			{
				return;
			}
		}

		private void Btn_Delete_Click(object sender, RoutedEventArgs e)
		{	
			try
			{
				if (!ValidateField()) return;

				MessageBoxResult popup = MessageBox.Show($"Delete the Order ID {Txt_Id.Text}?", "Update Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
				if (popup == MessageBoxResult.No) return;

				var order = CreateOrderFromTextBox();
				if (order == null) return; // TODO: error message

				bool result = _orderRepository.Delete(order.OrderId);
				
				if (!result)
				{
					MessageBox.Show("Delete failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
				MessageBox.Show("Deleted!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
				RefreshDataGrid();
			}
			catch (Exception ex)
			{
				return;
			}
		}

		private void Btn_Refresh_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Dg_Order_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SetTextBoxes();
			
		}


		private Order? CreateOrderFromTextBox()
		{
			
			try
			{
				Order order = new Order()
				{
					OrderId = int.Parse(Txt_Id.Text),
					MemberId = int.Parse(Txt_Member.Text),
					OrderDate = Dp_OrderDate.SelectedDate,
					RequiredDate = Dp_RequiredDate.SelectedDate,
					ShippedDate = Dp_ShippedDate.SelectedDate,
					Freight = int.Parse(Txt_Freight.Text),
				};
				return order;
			}
			finally
			{

			}

			
		}

		private void SetTextBoxes()
		{
			try
			{
				Order selected = (Order)Dg_Order.SelectedItem;
				if (selected == null) return;

				Txt_Id.Text = selected.OrderId.ToString();
				Txt_Member.Text = selected.MemberId.ToString();
				Dp_OrderDate.SelectedDate = selected.OrderDate;
				Dp_RequiredDate.SelectedDate = selected.RequiredDate;
				Dp_ShippedDate.SelectedDate = selected.ShippedDate;
				Txt_Freight.Text = selected.Freight.ToString();
			}
			catch (Exception ex)
			{
				return;
			}
		}

		private bool ValidateField()
		{
			try
			{
				if (Txt_Member.Text.Length == 0)
				{
					MessageBox.Show("Member Id must not be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return false;
				}

				if (int.Parse(Txt_Member.Text) <= 0)
				{
					MessageBox.Show("Member Id must be larger than 0", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return false;
				}

				if (_memberRepository.Get(int.Parse(Txt_Member.Text)) == null)
				{
					MessageBox.Show("Member Id not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return false;
				}

				

				if (Dp_OrderDate.SelectedDate == null)
				{
					MessageBox.Show("Order Date cannot be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return false;
				}

				if (Dp_RequiredDate.SelectedDate < Dp_OrderDate.SelectedDate) 
				{
					MessageBox.Show("Required Date must be after Order Date", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return false;
				}

				if (Dp_ShippedDate.SelectedDate < Dp_OrderDate.SelectedDate)
				{
					MessageBox.Show("Shipped Date must be after Order Date", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return false;
				}

				if (Txt_Freight.Text.Length == 0) 
				{
					MessageBox.Show("Freight must not be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return false;
				}

				if (int.Parse(Txt_Freight.Text) == 0)
				{
					MessageBox.Show("Freight cannot be negative.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return false;
				}
			}
			catch (Exception ex) 
			{
				MessageBox.Show("An error has occurred", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;

			}
			return true;
		}


		private void Btn_ProductManagePage_Click(object sender, RoutedEventArgs e)
		{
			ProductManagePage page = new(_loggedInMember);
			page.Show();
			this.Close();
		}

		private void Btn_MemberManagePage_Click(object sender, RoutedEventArgs e)
		{
			MemberManagePage page = new(_loggedInMember);
			page.Show();
			this.Close();
		}

		private void Btn_LogOut_Click(object sender, RoutedEventArgs e)
		{
			MainWindow window = new MainWindow();
			window.Show();
			this.Close();
        }
    }
}
