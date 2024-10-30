
using Repositories.Repos;
using Repositories.Repos.Interfaces;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SalesWPFApp
{
	
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private IMemberRepository _memberRepository;

		public MainWindow()
		{
			InitializeComponent();
			if (_memberRepository == null) _memberRepository = new MemberRepository();
			this.Title = "Log In";
		}

		private void Btn_Login_Click(object sender, RoutedEventArgs e)
		{
			string email = Txt_Email.Text;
			string password = Txt_Password.Password;

			var result = _memberRepository.Get(email, password);

			if (result == null)
			{
				MessageBox.Show("Incorrect username or password", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (!result.IsAdmin)
			{
				MessageBox.Show("You do not have access to this function!", "Unauthorized", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			ProductManagePage page = new();
			page.Show();
			this.Hide();
			this.Close();

			

			
		}

		private void Txt_Email_TextChanged(object sender, TextChangedEventArgs e)
		{

        }
    }
}