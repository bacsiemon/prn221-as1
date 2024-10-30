using DataAccess.Models;
using Repositories.Repos;
using Repositories.Repos.Interfaces;
using SalesWPFApp.MemberManager;
using SalesWPFApp.ProductManager;
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

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for ProductManagePage.xaml
    /// </summary>
    public partial class ProductManagePage : Window
	{
		private IProductRepository _productRepository;
		public ProductManagePage()
		{
			InitializeComponent();
			if (_productRepository == null) _productRepository = new ProductRepository();
			RefreshDataGrid();
			this.Title = "Product Manager";


		}
		#region product
		public void RefreshDataGrid()
		{
			Dg_Product.ItemsSource = _productRepository.GetAll();
			Dg_Product.SelectedIndex = 0;

		}

		public void SetTextBoxes()
		{
			try
			{
				Product selection = (Product)Dg_Product.SelectedItem;
				if (selection != null)
				{
					Txt_ProductId.Text = selection.ProductId.ToString();
					Txt_CategoryId.Text = selection.CategoryId.ToString();
					Txt_ProductName.Text = selection.ProductName;
					Txt_Weight.Text = selection.Weight;
					Txt_UnitPrice.Text = selection.UnitPrice.ToString();
					Txt_UnitsInStock.Text = selection.UnitsInStock.ToString();
				}
			} catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return;
			}
			
		}

		private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				SetTextBoxes();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return;
			}
			

		}

		private void Txt_ProductId_TextChanged(object sender, TextChangedEventArgs e)
		{

        }

		private void Txt_CategoryId_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void Txt_ProductName_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void Txt_Weight_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void Txt_UnitPrice_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void Txt_UnitsInStock_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void Btn_Create_Click(object sender, RoutedEventArgs e)
		{
			CreateProduct popup = new();
			popup.ShowDialog();
			RefreshDataGrid();
		}

		private void Btn_Update_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				CreateProduct popup = new((Product)Dg_Product.SelectedItem);
				popup.ShowDialog();
				RefreshDataGrid();
			}
			catch (Exception ex) 
			{ 
			
			}	
				
			
		}

		private void Btn_Delete_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				MessageBoxResult Result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

				if (Result == MessageBoxResult.Yes)
				{
					if (!_productRepository.Delete((Product)Dg_Product.SelectedItem))
					{
						MessageBox.Show("Delete failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
						return;
					}

					MessageBox.Show("Delete Successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
				};
				RefreshDataGrid();
				return;
			} catch (Exception ex)
			{
				MessageBox.Show("Delete failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			
		}

		private void Btn_Refresh_Click(object sender, RoutedEventArgs e)
		{
			RefreshDataGrid();
		}
		#endregion

		private void Btn_MemberManagePage_Click(object sender, RoutedEventArgs e)
		{
			MemberManagePage page = new();
			page.Show();
			this.Close();
		}
	}
}
