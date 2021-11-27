using System;

namespace Sylvan.Data.XBase
{
	/// <summary>
	/// The exception that is thrown when an XBase file contains an unsupported text encoding.
	/// </summary>
	public sealed class EncodingNotSupportedException : NotSupportedException
	{
		/// <summary>
		/// The language code for the unsupported language.
		/// </summary>
		public int LanguageCode { get; }

		internal EncodingNotSupportedException(int code)
		{
			this.LanguageCode = code;
		}
	}

	/// <summary>
	/// The exception that is thrown when an XBase file contains an unsupported colum type.
	/// </summary>
	public sealed class UnsupportedColumnTypeException : NotSupportedException
	{
		/// <summary>
		/// Gets the ordinal of the column.
		/// </summary>
		public int Ordinal { get; }

		/// <summary>
		/// Gets the name of the column.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// The type code for the unsupported column.
		/// </summary>
		public byte TypeCode { get; }

		internal UnsupportedColumnTypeException(int ordinal, string name, byte code)
		{
			this.Ordinal = ordinal;
			this.Name = name;
			this.TypeCode = code;
		}
	}
}
