using Digifly.Utility;
using DigiflyService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digifly.ViewModel
{
	public class MainViewModel : NotifyBase
	{
		private readonly ApplicationService _application;

		public MainViewModel(ApplicationService application)
		{
			_application = application;

			_application.UpdateStorage();


			//for (int i = 0; i < 500; i++)
			//{
			//	var res = _application.GetNewImageComparison();
			//	if (res.HasFailed)
			//	{
			//		var dwg = "";
			//	}
			//	else
			//	{
			//		_application.RateImage(res.Value.First.Id, res.Value.Second.Id);
			//	}







			//}

			StatusText = "Test";
			StatusTime = DateTime.Now;


		}

		#region Property

		private bool _IsLoading = false;
		public bool IsLoading
		{
			get
			{
				return _IsLoading;
			}
			set
			{
				_IsLoading = value;
				onPropertyChanged("IsLoading");
			}
		}

		private string _StatusText;
		public string StatusText
		{
			get
			{
				return _StatusText;
			}
			set
			{
				_StatusText = value;
				onPropertyChanged("StatusText");
			}
		}

		private DateTime? _StatusTime;
		public DateTime? StatusTime
		{
			get
			{
				return _StatusTime;
			}
			set
			{
				_StatusTime = value;
				onPropertyChanged("StatusTime");
			}
		}

		#endregion
	}
}
