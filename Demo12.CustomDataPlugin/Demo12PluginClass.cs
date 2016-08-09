using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessObjects;

namespace Demo12.LoanEventPlugin
{
	/// <summary>
	/// Our plugin class, decorated with the Plugin attribute
	/// </summary>
	[EllieMae.Encompass.ComponentModel.Plugin]
	public class Demo12PluginClass
	{
		/// <summary>
		/// A plugin should have a parameterless constructor for subscribing to application events
		/// </summary>
		public Demo12PluginClass()
		{
			EncompassApplication.LoanOpened += EncompassApplication_LoanOpened;
		}

		// Event handler for the LoanOpened event
		void EncompassApplication_LoanOpened(object sender, EventArgs e)
		{
			// Retrieve the XML file from the server
			DataObject customObject = EncompassApplication.Session.DataExchange.GetCustomDataObject("StateFees.xml");
			
			// Parse the file as XML
			XDocument feeDocument = XDocument.Parse(customObject.ToString(Encoding.UTF8));
			
			// Check the property state
			string propertyState = EncompassApplication.CurrentLoan.Fields["14"].FormattedValue;

			// Find the matching fee element in the document, if any
			var fees = from element in feeDocument.Descendants("FeeItem")
					   where (string)element.Attribute("State") == propertyState
					   select (string) element.Attribute("Fee");

			string fee = fees.FirstOrDefault();

			// Save the fee into the loan
			if (fee != null)
				EncompassApplication.CurrentLoan.Fields["CUST02FV"].Value = fee;
		}
	}
}
