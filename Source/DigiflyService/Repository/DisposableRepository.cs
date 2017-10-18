using DigiflyService.Repository.Context;
using System;

namespace DigiflyService.Repository
{
	public class DisposableRepository : DigiflyRepository
	{
		public DisposableRepository() : base(new DisposableContext()) { }
	}
}
