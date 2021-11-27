using System.Text;

namespace Sylvan.Data.XBase
{
	public sealed class XBaseDataReaderOptions
	{
		internal static readonly XBaseDataReaderOptions Default = new XBaseDataReaderOptions();


		public XBaseDataReaderOptions()
		{
			IgnoreMissingMemo = false;
			Encoding = null;
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
	}
}
