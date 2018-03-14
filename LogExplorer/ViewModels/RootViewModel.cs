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

		private List<Log> logs;

		#endregion

		#region Constructors and Destructors

		public RootViewModel(IExplorer explorer)
		{
			this.explorer = explorer;
			this.logs = new List<Log>();
		}

		#endregion

		#region Public Methods and Operators

		public void Init(string path)
		{
			//TODO: Set start directory
		}

		public override void Start()
		{
			this.Refresh();
			base.Start();
		}

		#endregion

		#region Methods

		private void Refresh()
		{
			this.logs = this.explorer.GetAllLogs();
		}

		#endregion
	}
}