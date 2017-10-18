using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigiflyService.Repository;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DigiflyService.Repository.Table;
using System.Collections.Generic;

namespace DigiflyTest
{
	[TestClass]
	public class ContextTest
	{
		[TestMethod]
		public void ShouldBeAbleToCountWinsAndLosses()
		{
			using (var repository = new DisposableRepository())
			{
				repository.Images.Insert(MakeImages(2).ToList());

				repository.Ratings.Insert(new Rating
				{
					WinnerId = 1,
					LoserId = 2,
					Time = DateTime.Now
				});

				var images = repository.Images.Entries.Select(image => new
				{
					image.Id,
					WinCount = image.Wins.Count(),
					LossCount = image.Losses.Count()

				}).ToList();

				Assert.AreEqual(1, images.First(image => image.Id == 1).WinCount);
				Assert.AreEqual(0, images.First(image => image.Id == 1).LossCount);
				Assert.AreEqual(0, images.First(image => image.Id == 2).WinCount);
				Assert.AreEqual(1, images.First(image => image.Id == 2).LossCount);
			}
		}

		[TestMethod]
		public void ShouldBeAbleToUpdate()
		{
			using (var repository = new DisposableRepository())
			{
				// Should be able to update one item

				var date = DateTime.Now;

				repository.Images.Insert(MakeImages(1, date).ToList());

				repository.Images.Update(repository.Images.Entries.ToList().Select(x => { x.Added = x.Added.AddDays(1); return x; }).First());

				Assert.IsTrue(repository.Images.Entries.All(x => x.Added > date));
			}

			using (var repository = new DisposableRepository())
			{
				// Should be able to update where criteria matches

				var date = DateTime.Now;

				repository.Images.Insert(MakeImages(20, date).ToList());

				repository.Images.Update(image =>
				{
					image.Added = image.Added.AddDays(1);

				}, image => image.Id == 1);

				Assert.AreEqual(1, repository.Images.Entries.Count(x => x.Added > date));
			}

			using (var repository = new DisposableRepository())
			{
				// Should be able to update multiple items

				var date = DateTime.Now;

				repository.Images.Insert(MakeImages(20, date).ToList());

				repository.Images.Update(repository.Images.Entries.ToList().Select(image =>
				{
					image.Added = image.Added.AddDays(1);
					return image;

				}).ToList());

				Assert.AreEqual(20, repository.Images.Entries.Count(x => x.Added > date));
			}
		}

		[TestMethod]
		public void ShouldAutomaticallyDeleteRequiredRelationships()
		{
			using (var repository = new DisposableRepository())
			{
				repository.Images.Insert(MakeImages(2).ToList());

				repository.Ratings.Insert(new Rating
				{
					Time = DateTime.Now,
					LoserId = 1,
					WinnerId = 2
				});

				repository.Images.Remove(x => x.Id == 1);

				Assert.IsFalse(repository.Ratings.Entries.Any());
			}
		}

		[TestMethod]
		public void ShouldBeAbleToRemove()
		{
			using (var repository = new DisposableRepository())
			{
				// Should be able to remove by criteria

				repository.Images.Insert(MakeImages(20).ToList());

				repository.Images.Remove(x => x.Id > 10);

				Assert.AreEqual(10, repository.Images.Entries.Count());
			}

			using (var repository = new DisposableRepository())
			{
				// Should be able to remove by id

				repository.Images.Insert(MakeImages(1).ToList());

				repository.Images.RemoveById(1);

				Assert.IsFalse(repository.Images.Entries.Any());
			}

			using (var repository = new DisposableRepository())
			{
				// Should be able to remove by multiple ids

				repository.Images.Insert(MakeImages(5).ToList());

				repository.Images.RemoveById(new[] { 1, 2, 3, 4, 5 });

				Assert.IsFalse(repository.Images.Entries.Any());
			}

			using (var repository = new DisposableRepository())
			{
				// Should be able to remove multiple items

				repository.Images.Insert(MakeImages(5).ToList());

				repository.Images.Remove(repository.Images.Entries.ToList());

				Assert.IsFalse(repository.Images.Entries.Any());
			}

			using (var repository = new DisposableRepository())
			{
				// Should be able to remove one item

				repository.Images.Insert(MakeImages(1).ToList());

				repository.Images.Remove(repository.Images.Entries.First());

				Assert.IsFalse(repository.Images.Entries.Any());
			}
		}

		public IEnumerable<Image> MakeImages(int count, DateTime? date = null)
		{
			using (MD5 md5 = MD5.Create())
			{
				for (int i = 0; i < count; i++)
				{
					var padded = count.ToString().PadLeft(20, '0');

					yield return new Image
					{
						FileName = $"{padded}.jpg",
						Checksum = md5.ComputeHash(Encoding.UTF8.GetBytes(padded)),
						Added = date ?? DateTime.Now
					};
				}
			}
		}
	}
}
