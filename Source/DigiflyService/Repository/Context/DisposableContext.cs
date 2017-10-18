using System;
using System.IO;

namespace DigiflyService.Repository.Context
{
	public class DisposableContext : DigiflyContext
	{
		public DisposableContext() : base(Directory.GetCurrentDirectory(), "Test.db") { }

		public void DeleteDbFile()
		{
			if (File.Exists(_location))
			{
				File.Delete(_location);
			}
		}

		public override void Dispose()
		{
			DeleteDbFile();
			base.Dispose();
		}
	}
}
