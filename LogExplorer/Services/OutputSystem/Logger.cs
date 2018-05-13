// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;

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

		private string message;

		#endregion

		#region Constructors and Destructors

		private Logger()
		{
			this.message = string.Empty;
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

		public void AddMessage(string msg)
		{
			this.message = $"[{DateTime.Now.ToString("T")}]: {msg}\n{this.message}".TrimEnd();
			propertyChange?.Invoke();
		}

		#endregion
	}
}