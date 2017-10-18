using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigiflyShared
{
	public static class StringExtensions
	{
		public static bool EndsWith(this string value, params string[] endings)
		{
			if (value == null) throw new ArgumentNullException("value");
			if (endings == null) throw new ArgumentNullException("endings");

			return endings.Any(x => value.EndsWith(x));
		}
	}

	public static class DateTimeExtensions
	{
		public static bool IsPast(this DateTime date)
		{
			return DateTime.Now > date;
		}
	}

	public static class EnumerableExtensions
	{
		public static IEnumerable<T> ShuffleEnumerator<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				throw new ArgumentNullException("enumerable");
			}

			Random random = new Random();
			var array = enumerable.ToArray();
			var length = array.Length;

			for (int i = 0; i < length; i++)
			{
				int j = random.Next(i, length);
				yield return array[j];
				array[j] = array[i];
			}
		}
	}

	public static class ListExtensions
	{
		public static T Random<T>(this IList<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}

			return list[new Random().Next(list.Count())];
		}

		public static IList<T> ShuffleCollection<T>(this IList<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}

			Random random = new Random();
			var array = list;
			var length = array.Count();

			for (int i = 0; i < length; i++)
			{
				var x = random.Next(0, length);
				if (x == i) continue;
				var temp = array[i];
				array[i] = array[x];
				array[x] = temp;
			}

			return array;
		}
	}
}
