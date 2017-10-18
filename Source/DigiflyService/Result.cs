using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiflyService
{
	interface IResult
	{
		ErrorType? ErrorType { get; set; }
		bool HasFailed { get; set; }
	}

	public enum ErrorType
	{
		NotEnoughImages
	}

	public class Result : IResult
	{
		public bool HasFailed { get; set; }
		public ErrorType? ErrorType { get; set; }

		internal static Result Success => new Result { HasFailed = false };
		internal static Result Fail => new Result { HasFailed = true };

		private Result() { }

		internal static Result Error(ErrorType type)
		{
			return new Result { HasFailed = true, ErrorType = type };
		}

		internal static Result<T> Value<T>(T value)
		{
			return Result<T>.Success(value);
		}
	}

	public class Result<T> : IResult
	{
		public ErrorType? ErrorType { get; set; }
		public bool HasFailed { get; set; }
		public T Value { get; set; }

		internal static Result<T> Fail => new Result<T> { HasFailed = true };

		private Result() { }

		internal static Result<T> Success(T value)
		{
			return new Result<T> { HasFailed = false, Value = value };
		}

		internal static Result<T> Error(ErrorType type)
		{
			return new Result<T> { HasFailed = true, ErrorType = type };
		}
	}
}
