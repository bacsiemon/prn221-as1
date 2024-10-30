using Microsoft.Extensions.DependencyInjection;
using Repositories.Repos;
using Repositories.Repos.Interfaces;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
	{

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			var services = new ServiceCollection();

			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<IMemberRepository, MemberRepository>();
		}
    }

}
