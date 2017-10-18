using DigiflyService.Repository;
using DigiflyService.Repository.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiflyShared;
using System.Security.Cryptography;
using DigiflyService.Repository.Table;

namespace DigiflyService.Service
{
	public class ApplicationService
	{
		private DigiflyRepository _repository;
		private string _location;

		public ApplicationService(string location)
		{
			_repository = new DigiflyRepository(new DigiflyContext(location, "Ratings.db"));

			_location = location;
			// TODO EntryContext kan være et interface som kommer gjennom injection?

		}

		public Result UpdateStorage()
		{
			// Må bli async task

			try
			{

				using (var md5 = MD5.Create())
				{
					var filesFromDirectory = Directory.GetFiles(_location, "*", SearchOption.TopDirectoryOnly)
						.Where(filePath => filePath.ToLower().EndsWith(".jpg", ".jpeg", ".png"))
						.Select(filePath =>
						{
							using (var fileStream = File.OpenRead(filePath))
							{
								return new
								{
									FileName = Path.GetFileName(filePath),
									Checksum = md5.ComputeHash(fileStream)
								};
							}
						})
						.ToList();

					var filesFromDb = _repository.Images.Entries
						.ToList();

					var changedFiles = filesFromDb
						.Where(x => filesFromDirectory.Any(y => y.FileName == x.FileName && !y.Checksum.SequenceEqual(x.Checksum)))
						.ToList();

					var removedFiles = filesFromDb
						.Where(x => !filesFromDirectory.Any(y => y.FileName == x.FileName))
						.ToList();

					var filesToRemove = changedFiles.Concat(removedFiles).ToList();

					if (filesToRemove.Any())
					{
						_repository.Images.Remove(changedFiles.Concat(removedFiles).ToList());
						filesFromDb.RemoveAll(x => filesToRemove.Any(y => x.Id == y.Id));
					}

					var newFiles = filesFromDirectory
						.Where(x => !filesFromDb.Any(y => y.FileName == x.FileName))
						.Select(x => new Image
						{
							Added = DateTime.Now,
							FileName = x.FileName,
							Checksum = x.Checksum
						})
						.ToList();

					_repository.Images.Insert(newFiles);
				}
				return Result.Success;
			}
			catch (Exception)
			{
				return Result.Fail;
			}
		}

		public Result<Comparison> GetNewImageComparison()
		{
			try
			{
				var imagesToRate = _repository.Images.Entries
					.OrderBy(x => Guid.NewGuid())
					.Select(x => new
					{
						x.FileName,
						x.Id,
						Wins = x.Wins.Count(),
						Losses = x.Losses.Count()
					})
					.Take(2)
					.ToList();

				if (imagesToRate.Count != 2)
				{
					return Result<Comparison>.Error(ErrorType.NotEnoughImages);
				}

				var first = imagesToRate[0];
				var second = imagesToRate[1];

				return Result.Value(new Comparison
				{
					First = new ImageForComparison
					{
						Id = first.Id,
						Path = Path.Combine(_location, first.FileName),
						FileName = first.FileName,
						Rating = StarRater.Rate(first.Wins, first.Losses)
					},
					Second = new ImageForComparison
					{
						Id = second.Id,
						Path = Path.Combine(_location, second.FileName),
						FileName = second.FileName,
						Rating = StarRater.Rate(first.Wins, first.Losses)
					}
				});
			}
			catch (Exception)
			{
				return Result<Comparison>.Fail;
			}
		}

		public Result RateImage(int winnerId, int loserId)
		{
			try
			{
				_repository.Ratings.Insert(new Rating
				{
					WinnerId = winnerId,
					LoserId = loserId,
					Time = DateTime.Now
				});

				return Result.Success;
			}
			catch (Exception)
			{
				return Result.Fail;
			}
		}
	}
}
