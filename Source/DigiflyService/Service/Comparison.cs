using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiflyService.Service
{
	public class Comparison
	{
		public ImageForComparison First { get; set; }
		public ImageForComparison Second { get; set; }
	}

	public class ImageForComparison
	{
		public int Id { get; set; }
		public string FileName { get; set; }
		public string Path { get; set; }
		public int? Rating { get; set; }
	}
}
