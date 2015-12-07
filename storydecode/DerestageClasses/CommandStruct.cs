using System;
using System.Collections.Generic;

namespace Story.Data
{
	public struct CommandStruct
	{
		public string Name;

		public List<string> Args;

		public CommandCategory Category;
	}
}
