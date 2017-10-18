using DigiflyService.Repository.Context;
using DigiflyService.Repository.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigiflyService.Repository
{
	public class DigiflyRepository : IDisposable
	{
		protected readonly DigiflyContext _context;

		public EntryContextWrapper<Image> Images { get; private set; }
		public EntryContextWrapper<Rating> Ratings { get; private set; }

		public DigiflyRepository(DigiflyContext context)
		{
			_context = context;

			// TODO Ta imot mappe som parameter (lag shortcuts)
			// TODO Fjern endrede filter og fjernede filer ved oppstart
			// TODO Legg til nye filer ved oppstart

			Images = new EntryContextWrapper<Image>(context, c => c.Images);
			Ratings = new EntryContextWrapper<Rating>(context, c => c.Ratings);
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
