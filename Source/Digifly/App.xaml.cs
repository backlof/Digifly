using Digifly.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Digifly
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			if (e.Args.Length == 1)
			{
				var argument = e.Args[0];
				if (Directory.Exists(argument))
				{
					//if (!Resources.Contains("ViewModelLocator"))
					//{
					//	Resources.Add("ViewModelLocator", new ViewModelLocator());
					//}
					((ViewModelLocator)Resources["ViewModelLocator"]).Init(argument);
				}
				else
				{
					Current.Shutdown();
				}
			}
			else if (e.Args.Length == 0)
			{
				//if (!Resources.Contains("ViewModelLocator"))
				//{
				//	Resources.Add("ViewModelLocator", new ViewModelLocator());
				//}
				((ViewModelLocator)Resources["ViewModelLocator"]).Init(Directory.GetCurrentDirectory());
			}
			else
			{
				Current.Shutdown();
			}
		}
	}
}
