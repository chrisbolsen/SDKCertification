using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EllieMae.Encompass.Automation;

namespace Demo10.AppEventPlugin
{
	/// <summary>
	/// Our plugin class, decorated with the Plugin attribute
	/// </summary>
	[EllieMae.Encompass.ComponentModel.Plugin]
	public class Demo10PluginClass
	{
		///<summary>
		///A plugin should have a parameterless constructor for subscribing to application events
		///</summary>
		public Demo10PluginClass()
		{
			EncompassApplication.Login += EncompassApplication_Login;
		}

		// Event handler for the Application's Login event
		void EncompassApplication_Login(object sender, EventArgs e)
		{
			Macro.Alert("Welcome, " + EncompassApplication.CurrentUser.FullName + " (" 
				+ EncompassApplication.CurrentUser.ID + ")");
		}
	}
}
