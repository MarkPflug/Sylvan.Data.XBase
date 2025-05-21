namespace Sylvan.Data.XBase
{
	//enum XBaseVersion : byte
	//{
	//	FoxBase = 0x02,
	//	FoxBasePlusNoMemo = 0x03,
	//	VisualFoxPro = 0x30,
	//	VisualFoxProAutoIncrement = 0x31,
	//	VisualFoxProVarField = 0x32,
	//	DBase4SqlTableFiles = 0x43,
	//	DBase4SqlSystemFiles = 0x63,
	//	FoxBasePlusMemo = 0x83,
	//	DBase4Memo = 0x8b,
	//	DBase4SqlTableFilesMemo = 0xcb,
	//	FoxProMemo = 0xf5,
	//	FoxBaseEx = 0xfb,
	//}

	[Flags]
	enum ColumnFlags
	{
		None = 0x00,
		SystemColumn = 0x01,
		Nullable = 0x02,
		Binary = 0x03,
		AutoIncrementing = 0x0c,
	}
}
