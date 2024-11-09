using DataAccess.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Repositories.Repos;
using Repositories.Repos.Interfaces;
using SalesWPFApp.OrderManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
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

namespace SalesWPFApp.MemberManager
{
    /// <summary>
    /// Interaction logic for MemberManagePage.xaml
    /// </summary>
    public partial class MemberManagePage : Window
    {
		private IMemberRepository _memberRepository;
		private List<Member> _members;
		private Member _loggedInMember;

		public MemberManagePage()
        {
			if (_memberRepository == null) _memberRepository = new MemberRepository();
            InitializeComponent();
			this.Title = "Member Manager";
			RefreshDataGrid();
        }

		public MemberManagePage( Member loggedInMember)
		{
			if (_memberRepository == null) _memberRepository = new MemberRepository();
			InitializeComponent();
			this.Title = "Member Manager";
			RefreshDataGrid();
			_loggedInMember = loggedInMember;
		}




		public void RefreshDataGrid()
		{
			_members = _memberRepository.GetAll();

			Dg_Member.ItemsSource = _members.Select(m => new Member
			{
				MemberId = m.MemberId,
				Email = m.Email,
				CompanyName = m.CompanyName,
				City = m.City,
				Country = m.Country,
				IsAdmin = m.IsAdmin,
			});
			Dg_Member.SelectedIndex = 0;
		}

		public void SetSelectedValue()
		{
			try
			{
				Member selected = (Member)Dg_Member.SelectedItem;
				if (selected != null)
				{
					Member result = _members.FirstOrDefault(m => m.MemberId == selected.MemberId);
					Txt_Id.Text = result.MemberId.ToString();
					Txt_Email.Text = result.Email;
					Txt_CompanyName.Text = result.CompanyName;
					Txt_City.Text = result.City;
					Txt_Country.Text = result.Country;
					Chk_IsAdmin.IsChecked = result.IsAdmin;
					Txt_Password.Password = result.Password;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return;
			}
		}

		private void Dg_Member_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SetSelectedValue();
		}

		private void Btn_Refresh_Click(object sender, RoutedEventArgs e)
		{
			RefreshDataGrid();
		}

		private void Btn_Delete_Click(object sender, RoutedEventArgs e)
		{
			Member selected = (Member)Dg_Member.SelectedItem;
			if (selected == null)
			{
				MessageBox.Show("An error has occured.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (selected.MemberId == _loggedInMember.MemberId)
			{
				MessageBox.Show("This Member is currently logged in. Cannot delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			MessageBoxResult popup = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (popup == MessageBoxResult.No) return;

			bool result = _memberRepository.Delete(selected.MemberId);
			if (!result)
			{
				MessageBox.Show("An error has occured.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			MessageBox.Show("Deleted Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
			RefreshDataGrid();
			return;
		}

		private void Btn_Update_Click(object sender, RoutedEventArgs e)
		{
			if (!ValidateField(true)) return;

			MessageBoxResult popup = MessageBox.Show($"Update the member ID {Txt_Id.Text} using the credentials above?", "Update Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (popup == MessageBoxResult.No) return;

			bool result = _memberRepository.Update(new Member
			{
				MemberId = int.Parse(Txt_Id.Text),
				Email = Txt_Email.Text,
				CompanyName = Txt_CompanyName.Text,
				City = Txt_City.Text,
				Country = Txt_Country.Text,
				IsAdmin = Chk_IsAdmin.IsChecked == null ? false : (bool)Chk_IsAdmin.IsChecked,
				Password = Txt_Password.Password,
			});

            if (!result)
            {
				MessageBox.Show("Update Failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			MessageBox.Show("Member Updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
			RefreshDataGrid();

		}


		private bool ValidateField(bool isUpdate)
		{
			if (Txt_Email.Text.Length < 1 || Txt_Email.Text.Length>100)
			{
				MessageBox.Show("Email must be between 1-100 characters.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			if (!new EmailAddressAttribute().IsValid(Txt_Email.Text))
			{
				MessageBox.Show("Invalid Email Format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			if (!isUpdate) 
			{
				if (_memberRepository.Get(Txt_Email.Text) != null)
				{
					MessageBox.Show("Email already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return false;
				}
			}


			if (Txt_CompanyName.Text.Length < 1 || Txt_CompanyName.Text.Length > 40)
			{
				MessageBox.Show("Company Name must be between 1-40 characters.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			if (Txt_City.Text.Length < 1 || Txt_City.Text.Length > 15)
			{
				MessageBox.Show("City must be between 1-15 characters.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			if (Txt_Country.Text.Length < 1 || Txt_Country.Text.Length > 15)
			{
				MessageBox.Show("Country must be between 1-15 characters.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			if (Txt_Password.Password.Length < 1 || Txt_Password.Password.Length > 30)
			{
				MessageBox.Show("Password must be between 1-30 characters.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}
			return true;
		}

		private void Btn_Create_Click(object sender, RoutedEventArgs e)
		{
			if (!ValidateField(false)) return;

			MessageBoxResult popup = MessageBox.Show("Do you want to create a new member using the credentials above?", "Create Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);


			if (popup == MessageBoxResult.No) return;

			bool result = _memberRepository.Add(new Member()
			{
				Email = Txt_Email.Text,
				CompanyName = Txt_CompanyName.Text,
				City = Txt_City.Text,
				Country = Txt_Country.Text,
				IsAdmin = Chk_IsAdmin.IsChecked == null ? false : (bool)Chk_IsAdmin.IsChecked,
				Password = Txt_Password.Password,
			});

			if (!result)
			{
				MessageBox.Show("Create failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			MessageBox.Show("Member created", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
			RefreshDataGrid();
		}

		private void Btn_ProductManagePage_Click(object sender, RoutedEventArgs e)
		{
			ProductManagePage page = new ProductManagePage(_loggedInMember);
			page.Show();
			this.Close();
		}

		private void Btn_OrderManagePage_Click(object sender, RoutedEventArgs e)
		{
			OrderManagePage page = new OrderManagePage(_loggedInMember) ;
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
