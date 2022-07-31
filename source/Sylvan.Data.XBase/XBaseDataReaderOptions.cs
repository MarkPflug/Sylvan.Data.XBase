using System.Text;

namespace Sylvan.Data.XBase
{
	/// <summary>
	/// Options for creating <see cref="XBaseDataReader"/>.
	/// </summary>
	public sealed class XBaseDataReaderOptions
	{
		internal static readonly XBaseDataReaderOptions Default = new XBaseDataReaderOptions();

		/// <summary>
		/// Creates a new <see cref="XBaseDataReaderOptions"/>.
		/// </summary>
		public XBaseDataReaderOptions()
		{
			IgnoreMissingMemo = false;
			Encoding = null;
			this.StringFactory = null;
		}

		/// <summary>
		/// Ignore a missing memo stream.
		/// </summary>
		/// <remarks>
		/// When false, the default, the DBaseDataReader will throw an exception
		/// during initialization if a memo stream is expected but not provided.
		/// When true, any data access that requires the memo stream will produce an exception
		/// upon acccess. Non-memo fields are all available.
		/// </remarks>
		public bool IgnoreMissingMemo { get; set; }


		/// <summary>
		/// Ignores columns of unknown types.
		/// </summary>
		/// <remarks>
		/// When this is disabled, the data reader will throw an exception during
		/// construction when an unknown type is encountered.
		/// 
		/// When this is enabled, columns of unknown type will be accessable as
		/// binary data via GetBytes or GetValue.
		/// </remarks>
		public bool IgnoreUnsupportedTypes { get; set; }

		/// <summary>
		/// Indicates if deleted records should be read.
		/// </summary>
		public bool ReadDeletedRecords { get; set; }

		/// <summary>
		/// Specifies an explicit encoding.
		/// </summary>
		/// <remarks>
		/// By default, the encoding specified in the file header will be used.
		/// </remarks>
		public Encoding? Encoding { get; set; }

		/// <summary>
		/// A string factory function which can de-dupe strings on construction. Defaults to null.
		/// </summary>
		public StringFactory? StringFactory { get; set; }
	}
}
