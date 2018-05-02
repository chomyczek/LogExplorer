﻿// LogExplorer
// Copyright(C) 2018
// Author Adam Kaszubowski

#region Usings

using System;
using System.Xml;

using LogExplorer.Models;
using LogExplorer.Services.Helpers;

#endregion

namespace LogExplorer.Services.Core
{
	public class Repository
	{
		#region Fields

		private readonly XmlDocument doc;

		private readonly string xmlPath;

		#endregion

		#region Constructors and Destructors

		public Repository(string xmlPath)
		{
			this.doc = new XmlDocument();
			this.xmlPath = xmlPath;

			if (FileHelper.FileExist(xmlPath))
			{
				this.doc.Load(xmlPath);
			}
			else
			{
				var xmldec = this.doc.CreateXmlDeclaration("1.0", "utf-8", null);
				this.doc.AppendChild(xmldec);
				var root = this.doc.CreateElement(nameof(Settings));
				this.doc.AppendChild(root);
				var defaultSettings = new Settings();

				foreach (var property in typeof(Settings).GetProperties())
				{
					var node = this.doc.CreateElement(property.Name);
					node.InnerText = property.GetValue(defaultSettings).ToString();
					root.AppendChild(node);
				}
			}
		}

		#endregion

		#region Public Methods and Operators

		public Settings GetSettings(
			
			//todo: change when message system will be updated
			//Action<string, string, string> showInfo
			)
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
                        //todo message
                        property.SetValue(settings, 0);
                    }
                    
                }
				else if (property.PropertyType == typeof(bool))
				{
					property.SetValue(settings, xmlValue.Equals("True", StringComparison.CurrentCultureIgnoreCase));
				}
				else
				{
					//todo: update message system
					//showInfo(null, null, $"Property '{property.Name}' was not recogniozed, during config was loaded.");
				}
			}

            this.PrepareDefaultProperties(ref settings);

			return settings;
		}

		public void UpdateSettings(Settings settings)
		{
			foreach (var property in typeof(Settings).GetProperties())
			{
				var currentNode = this.doc.GetElementsByTagName(property.Name);

				if (currentNode.Count == 0)
				{
					var node = this.doc.CreateElement(property.Name);
					node.InnerText = property.GetValue(settings).ToString();
					//FirstChild is header, second(last) is Settings
					this.doc.LastChild.AppendChild(node);
				}
				else
				{
					currentNode[0].InnerText = property.GetValue(settings).ToString();
				}
			}

			this.doc.Save(this.xmlPath);
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
	        if (settings.ConfigMode > 2 || settings.ConfigMode < 0)
	        {
	            settings.ConfigMode = 0;
	        }
        }

		#endregion
	}
}