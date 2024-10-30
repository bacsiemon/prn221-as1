using DataAccess.Models;
using Repositories.Repos;
using Repositories.Repos.Interfaces;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace SalesWPFApp.ProductManager
{
	/// <summary>
	/// Interaction logic for CreateProduct.xaml
	/// </summary>
	public partial class CreateProduct : Window
	{
		private IProductRepository _productRepository;
		private Product _UpdatedProduct;

		public CreateProduct()
		{
			InitializeComponent();
			if (_productRepository == null) _productRepository = new ProductRepository();
			this.Title = "Add a Product";
		}

		public CreateProduct(Product p)
		{
			InitializeComponent();
			if (_productRepository == null) _productRepository = new ProductRepository();
			_UpdatedProduct = p;
			LoadData();
			this.Title = "Update a Product";
		}

		private void LoadData()
		{
			if (_UpdatedProduct != null)
			{
				Txt_CategoryId.Text = _UpdatedProduct.CategoryId.ToString();
				Txt_ProductName.Text = _UpdatedProduct.ProductName;
				Txt_Weight.Text = _UpdatedProduct.Weight;
				Txt_UnitPrice.Text = _UpdatedProduct.UnitPrice.ToString();
				Txt_UnitsInStock.Text = _UpdatedProduct.UnitsInStock.ToString();
			}
		}



		private void Btn_Confirm_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (Txt_CategoryId.Text.Length == 0)
				{
					MessageBox.Show("Category ID is empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				if (Txt_ProductName.Text.Length == 0 || Txt_ProductName.Text.Length > 40)
				{
					MessageBox.Show("Product Name must be between 1-40 characters.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;

				}

				if (Txt_Weight.Text.Length == 0 || Txt_Weight.Text.Length > 20)
				{
					MessageBox.Show("Weight must be between 1-20 characters.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				if (int.Parse(Txt_UnitPrice.Text) <= 0)
				{
					MessageBox.Show("Unit Price must be larger than 0", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				if (int.Parse(Txt_UnitsInStock.Text) < 0)
				{
					MessageBox.Show("Unit Price must be at least 0", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				bool result = false;

				if (_UpdatedProduct == null)
				{
					result = _productRepository.Add(new Product()
					{
						CategoryId = int.Parse(Txt_CategoryId.Text),
						ProductName = Txt_ProductName.Text,
						Weight = Txt_Weight.Text,
						UnitPrice = int.Parse(Txt_UnitPrice.Text),
						UnitsInStock = int.Parse(Txt_UnitsInStock.Text)
					});
				}
				else
				{
					_UpdatedProduct.CategoryId = int.Parse(Txt_CategoryId.Text);
					_UpdatedProduct.ProductName = Txt_ProductName.Text;
					_UpdatedProduct.Weight = Txt_Weight.Text;
					_UpdatedProduct.UnitPrice = int.Parse(Txt_UnitPrice.Text);
					_UpdatedProduct.UnitsInStock = int.Parse(Txt_UnitsInStock.Text);
						result = _productRepository.Update(_UpdatedProduct);
				}

				if (!result)
				{
					MessageBox.Show("Failed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				MessageBox.Show("Success!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
				this.Close();
				return;

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}	
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
	}
}
