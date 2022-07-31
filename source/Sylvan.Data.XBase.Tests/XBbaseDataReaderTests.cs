using System;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Sylvan.Data.XBase
{
	public class EncodingsFixture
	{
		public EncodingsFixture()
		{
#if NETCOREAPP1_0_OR_GREATER
			// encodings are available by default on net461
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
		}
	}

	public class XBbaseDataReaderTests : IClassFixture<EncodingsFixture>
	{
		const string DataSetUrl = "https://www2.census.gov/geo/tiger/GENZ2018/shp/cb_2018_us_county_20m.zip";
		const string BigDataSetUrl = "https://prd-tnm.s3.amazonaws.com/StagedProducts/GovtUnit/Shape/GOVTUNIT_Oregon_State_Shape.zip";

		(string shapeFile, string dbfFile) Cache(string url, string dbfFileName = null)
		{
			var uri = new Uri(url);
			var shapeFileName = Path.GetFileName(uri.PathAndQuery);

			if (!File.Exists(shapeFileName))
			{
				using var oStream = File.Create(shapeFileName);
				using var iStream = new HttpClient().GetStreamAsync(url).Result;
				iStream.CopyTo(oStream);
			}

			dbfFileName = dbfFileName ?? Path.GetFileNameWithoutExtension(shapeFileName) + ".dbf";

			if (!File.Exists(dbfFileName))
			{
				var za = ZipFile.OpenRead(shapeFileName);
				var entry = za.GetEntry(dbfFileName);
				if (entry == null)
				{
					entry = za.GetEntry("Shape/" + dbfFileName);
				}

				entry.ExtractToFile(dbfFileName);
			}
			return (shapeFileName, dbfFileName);
		}

		[Fact]
		public void TestBig()
		{
			var (sf, df) = Cache(BigDataSetUrl, "GU_PLSSFirstDivision.dbf");
			var r = XBaseDataReader.Create(df);
			var schema = r.GetColumnSchema();

			while (r.Read())
			{
				ProcessRecord(r);
			}
		}				

		[Fact]
		public void TestOnline()
		{
			var (shapeFile, fileName) = Cache(DataSetUrl);
			var dr = XBaseDataReader.Create(fileName);

			var dt = new DataTable();
			dt.TableName = Path.GetFileNameWithoutExtension(fileName);
			dt.Load(dr);
			var sw = new StringWriter();
			dt.WriteXml(sw);
			var str = sw.ToString();
		}

		[Fact]
		public void TestEnc()
		{
			using var stream = File.OpenRead(@"Data/vc2.dbf");
			var r = XBaseDataReader.Create(stream);
			Process(r);
		}

		[Fact]
		public void TestZip()
		{
			var (sf, df) = Cache(DataSetUrl);
			using var zs = File.OpenRead(sf);
			var za = new ZipArchive(zs, ZipArchiveMode.Read);
			var entry = za.GetEntry("cb_2018_us_county_20m.dbf");

			using var stream = entry.Open();
			var r = XBaseDataReader.Create(stream);
			Process(r);
		}

		[Fact]
		public void Sample()
		{
			var r = XBaseDataReader.Create("Data/Sample.dbf", "Data/Sample.fpt");
			while (r.Read())
			{
				ProcessRecord(r);
			}
		}

		[Fact]
		public void UnsupportedType()
		{
			Assert.Throws<UnsupportedColumnTypeException>(() => XBaseDataReader.Create("Data/FolderRoot.dbf"));
		}

		[Fact]
		public void UnsupportedTypeAsBinary()
		{
			var opts = new XBaseDataReaderOptions { IgnoreUnsupportedTypes = true };
			var r = XBaseDataReader.Create("Data/FolderRoot.dbf", opts);

			while (r.Read())
			{
				var str = r.GetValue(9);
			}
		}

		[Fact]
		public void Test3()
		{
			Proc("Data/data2.dbf", "Data/data2.FPT");
		}

		[Fact]
		public void Numbers()
		{
			Proc("Data/Numbers.dbf");
		}

		[Fact]
		public void Numbers2()
		{
			Proc("Data/Number2.dbf");
		}

		[Fact]
		public void Curr()
		{
			Proc("Data/cur.dbf");
		}

		[Fact]
		public void Numbers4()
		{
			var name = "Data/num3.dbf";
			using var stream = File.OpenRead(name);
			var r = XBaseDataReader.Create(stream);
			while (r.Read())
			{
				var a = r.GetDecimal(0);
				var b = r.GetDecimal(1);
				var c = r.GetDecimal(2);
			}
		}

		[Fact]
		public void Varchar()
		{
			Proc("Data/NullTest.dbf");
		}

		[Fact]
		public void MemoTest()
		{
			Proc("Data/NullTest.dbf");
		}

		[Fact]
		public void A()
		{
			var ds = File.OpenRead("Data/memobintest.dbf");
			var ms = File.OpenRead("Data/memobintest.FPT");
			var dr = XBaseDataReader.Create(ds, ms);
			while (dr.Read())
			{
				var str = dr.GetString(0);

				var mss = new MemoryStream();
				var bin = dr.GetStream(1);
				bin.CopyTo(mss);
				var ss = Encoding.ASCII.GetString(mss.GetBuffer());

			}
		}

		void Proc(string name, string memoName = null)
		{
			using var stream = File.OpenRead(name);
			using var memoStream = memoName == null ? null : File.OpenRead(memoName);
			var r = XBaseDataReader.Create(stream, memoStream);
			Process(r);
		}

		int Process(XBaseDataReader r)
		{
			var schema = r.GetColumnSchema();
			var c = 0;
			while (r.Read())
			{
				ProcessRecord(r);
				c++;
			}
			return c;
		}

		static void ProcessRecord(IDataRecord record)
		{
			for (int i = 0; i < record.FieldCount; i++)
			{
				if (record.IsDBNull(i))
					continue;

				var type = record.GetFieldType(i);
				var tc = Type.GetTypeCode(type);

				switch (tc)
				{
					case TypeCode.Boolean:
						record.GetBoolean(i);
						break;
					case TypeCode.Int32:
						record.GetInt32(i);
						break;
					case TypeCode.DateTime:
						record.GetDateTime(i);
						break;
					case TypeCode.Double:
						record.GetDouble(i);
						break;
					case TypeCode.Decimal:
						record.GetDecimal(i);
						break;
					case TypeCode.String:
						record.GetString(i);
						break;
					default:
						continue;
						//throw new NotSupportedException();
				}
			}
		}
	}
}
