using System;

namespace Story.Data
{
	public struct CommandConfig
	{
		public int ID;

		public string Summary;

		public string Name;

		public string Usage;

		public string ClassName;

		public CommandCategory Category;

		public int MinArgCount;

		public int MaxArgCount;
	}
}
