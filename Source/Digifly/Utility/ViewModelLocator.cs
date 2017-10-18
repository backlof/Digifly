using Digifly.ViewModel;
using DigiflyService;
using DigiflyService.Repository;
using DigiflyService.Service;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digifly.Utility
{
	public class ViewModelLocator
	{
		private readonly IKernel _container;

		public ViewModelLocator()
		{
			_container = new StandardKernel();
		}

		public void Init(string location)
		{
			_container.Bind<DigiflyRepository>().To<DigiflyRepository>().InSingletonScope();
			_container.Bind<ApplicationService>().ToSelf().InSingletonScope().WithConstructorArgument("location", location);
			_container.Bind<MainViewModel>().ToSelf().InSingletonScope();
		}

		public MainViewModel MainViewModel
		{
			get
			{
				return _container.Get<MainViewModel>();
			}
		}
	}
}
