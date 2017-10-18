using DigiflyService.Repository.Context;
using DigiflyService.Repository.Table;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DigiflyService.Repository
{
	public class EntryContextWrapper<TEntry> where TEntry : class, ITable
	{
		protected readonly DigiflyContext _context;
		protected readonly DbSet<TEntry> _dbSet;

		public EntryContextWrapper(DigiflyContext context, Func<DigiflyContext, DbSet<TEntry>> dbSetFunc)
		{
			_context = context;
			_dbSet = dbSetFunc(context);
		}

		public IQueryable<TEntry> Entries => _dbSet.AsNoTracking();

		public void DetachAll(ICollection<TEntry> entries)
		{
			foreach (var entry in entries)
			{
				_context.Entry(entry).State = EntityState.Detached;
			}
		}

		public void Insert(TEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}

			Insert(new[] { entry });
		}

		public void Insert(ICollection<TEntry> entries)
		{
			if (entries == null)
			{
				throw new ArgumentNullException("entries");
			}

			_dbSet.AddRange(entries);
			_context.SaveChanges();

			DetachAll(entries);
		}

		public void Update(TEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}

			Update(new[] { entry });
		}

		public void Update(ICollection<TEntry> entries)
		{
			if (entries == null)
			{
				throw new ArgumentNullException("entries");
			}

			_dbSet.UpdateRange(entries);
			_context.SaveChanges();

			DetachAll(entries);
		}

		public void Remove(ICollection<TEntry> entries)
		{
			if (entries == null)
			{
				throw new ArgumentNullException("entries");
			}

			_dbSet.RemoveRange(entries);
			_context.SaveChanges();

			DetachAll(entries);
		}

		public void Remove(TEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}

			Remove(new[] { entry });
		}

		public void RemoveById(int id)
		{
			if (id < 1)
			{
				throw new ArgumentOutOfRangeException("id");
			}

			Remove(x => x.Id == id);
		}

		public void RemoveById(ICollection<int> ids)
		{
			if (ids == null)
			{
				throw new ArgumentNullException("ids");
			}

			Remove(x => ids.Contains(x.Id));
		}

		public void Remove(Expression<Func<TEntry, bool>> condition)
		{
			if (condition == null)
			{
				throw new ArgumentNullException("condition");
			}

			Remove(Entries.Where(condition).ToList());
		}

		public void Update(Action<TEntry> changes, Expression<Func<TEntry, bool>> condition)
		{
			if (condition == null)
			{
				throw new ArgumentNullException("condition");
			}
			if (changes == null)
			{
				throw new ArgumentNullException("changes");
			}

			Update(Entries.Where(condition).ToList().Select(x =>
			{
				changes(x);
				return x;

			}).ToList());
		}
	}
}
