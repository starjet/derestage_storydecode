using System;
using System.Runtime.InteropServices;

namespace Story.Data
{
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface ICommandConfig
	{
		void Init();

		int GetCommandID(ref string name);

		string GetCommandName(int id);

		string GetCommandSummary(int id);

		string GetCommandUsage(int id);

		int GetCommandConfigListCount();

		string GetSpacer();
	}
}
