// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.Collections.Generic;
using System.Text;

using LogExplorer.Models;
using LogExplorer.Services.Core;

using MvvmCross.Platform;

#endregion

namespace LogExplorer.Services.OutputSystem
{
	public class Logger
	{
		#region Static Fields

		private static Logger instance;

		private static object instanceLocker = new object();

		private static Action propertyChange;

		private static object propertyChangeLocker = new object();

		#endregion

		#region Fields

		private readonly List<string> history;

		private readonly StringBuilder stringBuilder;

		private string message;

		private Settings settings;

		#endregion

		#region Constructors and Destructors

		private Logger()
		{
			this.message = string.Empty;
			this.stringBuilder = new StringBuilder();
			this.history = new List<string>();
			this.TryGetSettings();
		}

		#endregion

		#region Public Properties

		public static Logger Instance
		{
			get
			{
				if (instance != null)
				{
					return instance;
				}
				lock (instanceLocker)
				{
					if (instance == null)
					{
						instance = new Logger();
					}
				}

				return instance;
			}
		}

		public string Message => this.message;

		#endregion

		#region Public Methods and Operators

		public static void PrepareInstance(Action propertyChangeAction)
		{
			if (propertyChange != null)
			{
				return;
			}
			lock (propertyChangeLocker)
			{
				if (propertyChange == null)
				{
					propertyChange = propertyChangeAction;
					propertyChange.Invoke();
				}
			}
		}

		public void AddDetailMessage(string msg)
		{
			if (this.settings == null && !this.TryGetSettings())
			{
				return;
			}
			if (this.settings.LoggerShowDetails)
			{
				this.AddMessage(msg);
			}
		}

		public void AddMessage(string msg)
		{
			this.stringBuilder.Clear();
			msg = $"[{DateTime.Now.ToString("T")}]: {msg}";
			this.UpdateHistory(msg);

			foreach (var line in this.history)
			{
				this.stringBuilder.AppendLine(line);
			}

			this.message = this.stringBuilder.ToString().TrimEnd();
			propertyChange?.Invoke();
		}

		private bool TryGetSettings()
		{
			if (!Mvx.CanResolve<Repository>())
			{
				return false;
			}

			this.settings = Mvx.Resolve<Repository>().Settings;
			return true;
		}

		#endregion

		#region Methods

		private void UpdateHistory(string msg)
		{
			this.history.Insert(0, msg);
			if (this.settings == null&& !this.TryGetSettings())
			{
				return;
			}
			if (this.history.Count > this.settings.LoggerMemmory)
			{
				this.history.RemoveAt(this.settings.LoggerMemmory);
			}
		}

		#endregion
	}
}