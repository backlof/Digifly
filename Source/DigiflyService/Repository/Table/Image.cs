using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DigiflyService.Repository.Table
{
	public partial class Image : ITable
	{
		public int Id { get; set; }
		public string FileName { get; set; }
		public byte[] Checksum { get; set; }
		public DateTime Added { get; set; }
	}

	public partial class Image
	{
		public virtual ICollection<Rating> Wins { get; set; }
		public virtual ICollection<Rating> Losses { get; set; }
	}
}
