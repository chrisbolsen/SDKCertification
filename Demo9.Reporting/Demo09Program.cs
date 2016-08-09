using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.Query;
using EllieMae.Encompass.Reporting;

namespace DemoApplication
{
	class Demo9Program
	{
		static void Main()
		{
			//Initialize the Encompass API Runtime Services
			new EllieMae.Encompass.Runtime.RuntimeServices().Initialize();

			//Invoke your own internal "main" routine
			ExecuteApp();
		}

		static void ExecuteApp()
		{
			//Create a Session object -- it will be "disconnected" to start
			Session session = new Session();

			try
			{
				//Connect to our local Encompass system
				session.Start("http://localhost", "admin", "support");

				//Create a query criterion for the Started milestone date to be greater than January 1, 2016
				DateFieldCriterion dateCri = new DateFieldCriterion();
				dateCri.FieldName = "Fields.MS.START";
				dateCri.MatchType = OrdinalFieldMatchType.GreaterThan;
				dateCri.Value = new DateTime(2016, 1, 1);

				/*DateFieldCriterion cri = new DateFieldCriterion("Fields.MS.START",
				new DateTime(2016, 1, 1), OrdinalFieldMatchType.GreaterThanOrEquals,
				DateFieldMatchPrecision.Day);*/

				//Create a query criterion for the borrower's last name to contain an "S"
				StringFieldCriterion nameCri = new StringFieldCriterion();
				nameCri.FieldName = "Fields.4002";
				nameCri.MatchType = StringFieldMatchType.Contains;
				nameCri.Value = "S";

				//(Started milestone date greater than January 1, 2016) or (Borrower's last name contains an "S")
				QueryCriterion cri = dateCri.Or(nameCri);

				//Build the list of desired fields, using canonical names and reporting database canonical names
				StringList fields = new StringList {
				"Loan.LoanNumber",
				"Loan.LoanAmount",
				"Loan.BorrowerLastName",
				"Fields.1240",
				"Loan.GUID" };

				//Return rows that contain loan data using the Loan Report Cursor
				LoanReportCursor cursor = session.Reports.OpenReportCursor(fields, cri);

				foreach (LoanReportData loanData in cursor)
				{
					Console.WriteLine("{0}, {1}, {2}, {3}, {4}",
						loanData["Loan.LoanNumber"],
						loanData["Loan.LoanAmount"],
						loanData["Loan.BorrowerLastName"],
						loanData["Fields.1240"],
						loanData["Loan.GUID"]);
				}

				cursor.Close();

				//Return rows that contain loan data using the Loan Report Cursor
				//LoanReportCursor cursor = session.Reports.OpenReportCursor(fields, cri);

				//using (StreamWriter outputFile = new StreamWriter("C:\\temp\\SDKreport09.CSV"))

				//foreach (LoanReportData loanData in cursor)
				//{
				//outputFile.WriteLine("{0}, {1}, {2}, {3}, {4}",
				//loanData["Loan.LoanNumber"],
				//loanData["Loan.LoanAmount"],
				//loanData["Loan.BorrowerLastName"],
				//loanData["Fields.MS.START"],
				//loanData["Loan.GUID"]);
				//}
				//cursor.Close();
			}

			//Handle a login exception
			catch (LoginException)
			{
				Console.WriteLine("There was a login exception. Press any key to exit.");
				Console.ReadLine();
			}

			//Handle a connection exception
			catch (ConnectionException)
			{
				Console.WriteLine("The application was unable to establish a connection to the Encompass Server.");
				Console.WriteLine("Press any key to exit.");
				Console.ReadLine();
			}

			//Handle other exceptions
			catch (Exception)
			{
				Console.WriteLine("There was an exception other than a login or connection exception.");
				Console.WriteLine("Press any key to exit.");
				Console.ReadLine();
			}

			finally
			{
				// End the session to gracefully log out of the server
				session.End();

				// Wait for user input
				Console.WriteLine("The session has ended. Press any key to exit.");
				Console.ReadLine();
			}
		}
	}
}
