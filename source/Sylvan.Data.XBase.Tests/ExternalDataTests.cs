using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Sylvan.Data.XBase;

// Set the `SylvanExcelTestData` env var to point to a directory
// containing files that will be tested by this set of tests.
public abstract class ExternalDataTests
{
	ITestOutputHelper o;

	public ExternalDataTests(ITestOutputHelper o)
	{
		this.o = o;
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
	}

	public void TestOutput(string str)
	{
		o.WriteLine(str);
	}

	public static string GetRootPath()
	{
		return Environment.GetEnvironmentVariable("SylvanXBaseTestData");
	}

	public static string GetFullPath(string file)
	{
		return Path.Combine(GetRootPath(), file);
	}

	public static IEnumerable<object[]> GetDbfFiles()
	{
		return GetTestFiles("*.dbf");
	}

	public static IEnumerable<object[]> GetTestFiles(string pattern)
	{
		var path = GetRootPath();
		if (string.IsNullOrEmpty(path))
		{
			yield return new object[] { null };
			yield break;
		}

		foreach (var file in Directory.EnumerateFiles(path, pattern, SearchOption.AllDirectories))
		{
			var rel = Path.GetRelativePath(path, file);
			yield return new object[] { rel };
		}
	}
}

public class ExternalDbfTests : ExternalDataTests
{
	public ExternalDbfTests(ITestOutputHelper o) : base(o)
	{
	}

	[Theory]
	[MemberData(nameof(GetDbfFiles))]
	public void Test(string path)
	{
		if (path == null) return;

		var fullPath = GetFullPath(path);

		var o = new XBaseDataReaderOptions { IgnoreUnsupportedTypes = true };
		using var x = XBaseDataReader.Create(fullPath, o);
		XBbaseDataReaderTests.Process(x);

	}
}