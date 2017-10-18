using DigiflyService.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiflyTest
{
	[TestClass]
	public class RatingTests
	{
		[TestMethod]
		public void ShouldReturnNullWhenThereAreNoRatings()
		{
			Assert.IsNull(StarRater.Rate(0, 0));
		}

		[TestMethod]
		public void ShouldReturnFiveWhenThereAreNoLosses()
		{
			Assert.AreEqual(5,StarRater.Rate(1, 0));
			Assert.AreEqual(5,StarRater.Rate(10, 0));
			Assert.AreEqual(5,StarRater.Rate(100, 0));
		}


		[TestMethod]
		public void ShouldReturnOneWhenThereAreNoWins()
		{
			Assert.AreEqual(1, StarRater.Rate(0, 1));
			Assert.AreEqual(1, StarRater.Rate(0, 10));
			Assert.AreEqual(1, StarRater.Rate(0, 100));
		}

		[TestMethod]
		public void ShouldScaleCorrectly()
		{
			Assert.AreEqual(1, StarRater.Rate(10, 80));
			Assert.AreEqual(1, StarRater.Rate(10, 20));
			Assert.AreEqual(2, StarRater.Rate(11, 20));
			Assert.AreEqual(2, StarRater.Rate(10, 10));
			Assert.AreEqual(3, StarRater.Rate(11, 10));
			Assert.AreEqual(3, StarRater.Rate(20, 10));
			Assert.AreEqual(4, StarRater.Rate(25, 10));
			Assert.AreEqual(4, StarRater.Rate(50, 10));
			Assert.AreEqual(5, StarRater.Rate(60, 10));
		}
	}
}
