﻿using System;
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
	/// Interaction logic for blank.xaml
	/// </summary>
	public partial class blank : Window
	{
		public blank()
		{
			InitializeComponent();
			ShowMainWindow();
		}

		public void ShowMainWindow()
		{
			MainWindow window = new();
			window.Show();
			this.Hide();
			this.Close();
		}
	}
}
