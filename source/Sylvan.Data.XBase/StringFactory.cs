namespace Sylvan.Data.XBase;

/// <summary>
/// A function that can be used to de-dupe strings during construction directly from internal buffers.
/// </summary>
/// <remarks>
/// The Sylvan.Common library can provide an implementation of this method via the Sylvan.StringPool type.
/// </remarks>
public delegate string StringFactory(char[] buffer, int offset, int length);