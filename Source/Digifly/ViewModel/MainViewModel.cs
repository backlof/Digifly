using Digifly.Utility;
using DigiflyService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Digifly.ViewModel
{
	public class MainViewModel : NotifyBase
	{
		private readonly ApplicationService _application;

		public ImageForComparison First { get; set; }
		public ImageForComparison Second { get; set; }

		public MainViewModel(ApplicationService application)
		{
			_application = application;

			StatusText = "Updating database...";
			StatusTime = DateTime.Now;

			var updateResult = _application.UpdateStorage();

			if (updateResult.HasFailed)
			{
				// Shut down application
			}

			StatusText = "Finished updating database";
			StatusTime = DateTime.Now;

			LoadNewComparison();

		}

		public void LoadNewComparison()
		{
			var comp = _application.GetNewImageComparison();
			if (!comp.HasFailed)
			{
				First = comp.Value.First;
				Second = comp.Value.Second;


				FirstImage = First.Path;
				SecondImage = Second.Path;
				FirstFileName = First.FileName;
				SecondFileName = Second.FileName;

				FirstRating = First.Rating.HasValue ? First.Rating.Value == 1 ? "1 star" : $"{First.Rating.Value} stars" : "Not rated";
				SecondRating = Second.Rating.HasValue ? Second.Rating.Value == 1 ? "1 star" : $"{Second.Rating.Value} stars" : "Not rated";
			}
		}


		public ICommand RateFirstImage
		{
			get
			{
				return new RelayCommand(ExecuteRateFirstImage, CanRateFirstImage);
			}
		}

		public void ExecuteRateFirstImage(object parameter)
		{
			_application.RateImage(First.Id, Second.Id);
			LoadNewComparison();
		}

		public bool CanRateFirstImage(object parameter)
		{
			return true;
		}



		public ICommand Close
		{
			get
			{
				return new RelayCommand(ExecuteClose, CanClose);
			}
		}

		public void ExecuteClose(object parameter)
		{
			_application.RenameFilesAfterPlacement();
		}

		public bool CanClose(object parameter)
		{
			return true;
		}



		public ICommand RateSecondImage
		{
			get
			{
				return new RelayCommand(ExecuteRateSecondImage, CanRateSecondImage);
			}
		}

		public void ExecuteRateSecondImage(object parameter)
		{
			_application.RateImage(Second.Id, First.Id);
			LoadNewComparison();
		}

		public bool CanRateSecondImage(object parameter)
		{
			return true;
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

		private string _FirstImage;
		public string FirstImage
		{
			get
			{
				return _FirstImage;
			}
			set
			{
				_FirstImage = value;
				onPropertyChanged("FirstImage");
			}
		}

		private string _SecondImage;
		public string SecondImage
		{
			get
			{
				return _SecondImage;
			}
			set
			{
				_SecondImage = value;
				onPropertyChanged("SecondImage");
			}
		}

		private string _FirstFileName;
		public string FirstFileName
		{
			get
			{
				return _FirstFileName;
			}
			set
			{
				_FirstFileName = value;
				onPropertyChanged("FirstFileName");
			}
		}

		private string _SecondFileName;
		public string SecondFileName
		{
			get
			{
				return _SecondFileName;
			}
			set
			{
				_SecondFileName = value;
				onPropertyChanged("SecondFileName");
			}
		}

		private string _FirstRating;
		public string FirstRating
		{
			get
			{
				return _FirstRating;
			}
			set
			{
				_FirstRating = value;
				onPropertyChanged("FirstRating");
			}
		}


		private string _SecondRating;
		public string SecondRating
		{
			get
			{
				return _SecondRating;
			}
			set
			{
				_SecondRating = value;
				onPropertyChanged("SecondRating");
			}
		}


		#endregion
	}
}
