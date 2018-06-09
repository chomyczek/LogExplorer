// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.Xml;

using LogExplorer.Models;
using LogExplorer.Services.Helpers;
using LogExplorer.Services.OutputSystem;

#endregion

namespace LogExplorer.Services.Core
{
	public class Repository
	{
		#region Fields

		private readonly XmlDocument doc;

		private readonly Logger logger;

		private readonly string xmlPath;

		#endregion

		#region Constructors and Destructors

		public Repository(string xmlPath)
		{
			this.logger = Logger.Instance;
			this.doc = new XmlDocument();
			this.xmlPath = xmlPath;

			if (FileHelper.FileExist(xmlPath))
			{
				this.doc.Load(xmlPath);
				this.logger.AddDetailMessage(Messages.XmlFileLoadSuccess);
			}
			else
			{
				var xmldec = this.doc.CreateXmlDeclaration("1.0", "utf-8", null);
				this.doc.AppendChild(xmldec);
				var root = this.doc.CreateElement(nameof(this.Settings));
				this.doc.AppendChild(root);
				var defaultSettings = new Settings();

				foreach (var property in typeof(Settings).GetProperties())
				{
					var node = this.doc.CreateElement(property.Name);
					node.InnerText = property.GetValue(defaultSettings).ToString();
					root.AppendChild(node);
				}
			}

			this.GetSettings();
		}

		#endregion

		#region Public Properties

		public Settings Settings { get; private set; }

		#endregion

		#region Public Methods and Operators

		public void UpdateSettings()
		{
			foreach (var property in typeof(Settings).GetProperties())
			{
				var currentNode = this.doc.GetElementsByTagName(property.Name);

				if (currentNode.Count == 0)
				{
					var node = this.doc.CreateElement(property.Name);
					node.InnerText = property.GetValue(this.Settings).ToString();
					//FirstChild is header, second(last) is Settings
					this.doc.LastChild.AppendChild(node);
					this.logger.AddDetailMessage(Messages.GetPropertyAdded(property.Name));
				}
				else
				{
					currentNode[0].InnerText = property.GetValue(this.Settings).ToString();
					this.logger.AddDetailMessage(Messages.GetPropertyUpdated(property.Name));
				}
			}

			this.doc.Save(this.xmlPath);
			this.logger.AddMessage(Messages.SettingsSaved);
		}

		#endregion

		#region Methods

		private void GetSettings()
		{
			var settings = new Settings();

			foreach (var property in typeof(Settings).GetProperties())
			{
				var node = this.doc.GetElementsByTagName(property.Name);

				if (node.Count == 0)
				{
					continue;
				}

				var xmlValue = node[0].InnerText;

				if (property.PropertyType == typeof(string))
				{
					property.SetValue(settings, xmlValue);
				}
				else if (property.PropertyType == typeof(int))
				{
					int value;
					if (int.TryParse(xmlValue, out value))
					{
						property.SetValue(settings, value);
					}
					else
					{
						this.logger.AddMessage(Messages.GetProblemParsingValue(xmlValue, nameof(Int32)));
						property.SetValue(settings, 0);
					}
				}
				else if (property.PropertyType == typeof(bool))
				{
					property.SetValue(settings, xmlValue.Equals("True", StringComparison.CurrentCultureIgnoreCase));
				}
				else
				{
					this.logger.AddMessage(Messages.GetPropertyNotRecognized(property.Name));
				}
			}

			this.PrepareDefaultProperties(ref settings);

			this.Settings = settings;
			this.logger.AddMessage(Messages.SettingsLoaded);
		}

		private void PrepareDefaultProperties(ref Settings settings)
		{
			if (string.IsNullOrEmpty(settings.RootLogsPath))
			{
				settings.RootLogsPath = @"C:\History\";
			}
			if (string.IsNullOrEmpty(settings.ExportPath))
			{
				settings.ExportPath = @"C:\ExportedLogs\";
			}
			if (string.IsNullOrEmpty(settings.TesterPath))
			{
				settings.TesterPath = @"C:\Program Files (x86)\Intel\Multi Tester\";
			}
			if (settings.ConfigMode > 2
			    || settings.ConfigMode < 0)
			{
				settings.ConfigMode = 0;
			}
			if (settings.LoggerMemory < 0)
			{
				settings.LoggerMemory = 0;
			}
		}

		#endregion
	}
}