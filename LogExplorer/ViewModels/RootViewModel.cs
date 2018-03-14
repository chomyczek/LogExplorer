// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System.Collections.Generic;

using LogExplorer.Models;
using LogExplorer.Services.Interfaces;

using MvvmCross.Core.ViewModels;

#endregion

namespace LogExplorer.ViewModels
{
	public class RootViewModel : MvxViewModel
	{
		#region Fields

		private readonly IExplorer explorer;

		public List<Log> Logs { get; set; }

		#endregion

		#region Constructors and Destructors

		public RootViewModel(IExplorer explorer)
		{
			//TODO: Set start directory
			this.explorer = explorer;
			this.Logs = new List<Log>();
		}

		#endregion

		#region Public Methods and Operators

		public override void Start()
		{
			this.Refresh();
			base.Start();
		}

		#endregion

		#region Methods

		private void Refresh()
		{
			this.Logs = this.explorer.GetAllLogs();
		}

		#endregion
	}
}