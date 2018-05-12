// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;

#endregion

namespace LogExplorer.Customs
{
	public class RegionAttribute : Attribute
	{
		#region Constructors and Destructors

		public RegionAttribute(string name)
		{
			this.Name = name;
		}

		#endregion

		#region Public Properties

		public string Name { get; private set; }

		#endregion
	}
}