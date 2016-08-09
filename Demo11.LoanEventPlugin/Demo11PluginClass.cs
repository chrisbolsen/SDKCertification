using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EllieMae.Encompass.Automation;

namespace Demo11.LoanEventPlugin
{
	///<summary>
	///Our plugin class, decorated with the Plugin attribute
	///</summary>
	[EllieMae.Encompass.ComponentModel.Plugin]
	public class Demo11PluginClass
	{
		///<summary>
		///A plugin should have a parameterless constructor for subscribing to application events
		///</summary>
		public Demo11PluginClass()
		{
			EncompassApplication.LoanOpened += EncompassApplication_LoanOpened;
		}

		//Event handler for the LoanOpened event
		void EncompassApplication_LoanOpened(object sender, EventArgs e)
		{
			//Check to see if the Application Date is empty
			if (EncompassApplication.CurrentLoan.Fields["745"].IsEmpty())
			{
				DialogResult res = MessageBox.Show(EncompassApplication.Screens,
					"This loan does not have an application date. Fill in today's date?",
					"Loan Event Plugin", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (res == DialogResult.Yes)
				{
					//Set the Application Date field to today's date when the user clicks Yes
					EncompassApplication.CurrentLoan.Fields["745"].Value = DateTime.Today;

					//Refresh the Loans screen
					LoansScreen screen = (LoansScreen)EncompassApplication.Screens[EncompassScreen.Loans];

					if (screen.CurrentForm != null)
						screen.CurrentForm.Refresh();
				}
			}
		}
	}
}
