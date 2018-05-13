// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

using System;
using System.Linq.Expressions;

namespace LogExplorer.Services.OutputSystem
{
	public class Logger
	{
		#region Static Fields

		private static Logger instance;

		private static Action propertyChange;

		private string message;

		private static object instanceLocker= new object();

		private static object propertyChangeLocker = new object();

		public string Message => this.message;

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
		private Logger()
		{
			this.message = string.Empty;
		}

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

		public void AddMessage(string msg)
		{
			this.message = $"{msg}\n{this.message}";
			propertyChange?.Invoke();
		}

		#endregion
	}
}