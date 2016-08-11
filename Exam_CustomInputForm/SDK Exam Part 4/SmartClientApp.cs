using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


using EllieMae.Encompass.Query;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.BusinessObjects;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Reporting;

namespace SDK_Exam_Part_4
{
    public class SmartClientApp
    {
        static void Main()
        {
            //Initialize runtime services
            new EllieMae.Encompass.Runtime.RuntimeServices().Initialize();

            //Invoke "main" method
            startApplication();
        }

        private static void startApplication()
        {
            ApplicationLog.DebugEnabled = true;
            ApplicationLog.WriteDebug("SDK StandAlone APP", "Report Application Started");

            var appSettings = ConfigurationManager.AppSettings;

            string _URL = appSettings["EncompassURL"] ?? "local";
            string _UID = appSettings["UID"] ?? "admin";
            string _PWD = appSettings["PW"] ?? "password";
            string _LoanGUID = appSettings["LOANGUID"] ?? "";


            Session session = new Session();

            try
            {
                //Connect to our local Encompass system
                session.Start(_URL, _UID, _PWD);
                
                runReport(session);
            }
            //Handle a login exception
            catch (LoginException ex)
            {
                ApplicationLog.WriteDebug("SDK StandAlone APP", "There was a login exception" + ex.Message + "Press any key to exit.");
            }

            //Handle a connection exception
            catch (ConnectionException ex)
            {
                ApplicationLog.WriteDebug("SDK StandAlone APP", "The application was unable to establish a connection to the Encompass Server." + ex.Message);
            }

            //Handle other exceptions
            catch (VersionException ex)
            {
                ApplicationLog.WriteDebug("SDK StandAlone APP", "There was an exception with the Version, not compatable with Encompass Server." + ex.Message);

            }
            //Handle other exceptions
            catch (Exception ex)
            {
                ApplicationLog.WriteDebug("SDK StandAlone APP", "There was an exception other than a login or connection exception." + ex.Message);

            }

            finally
            {
                ApplicationLog.WriteDebug("SDK StandAlone APP", "Application Completed");
                ApplicationLog.DebugEnabled = false;
                // End the session to gracefully log out of the server
                if (session != null)
                    session.End();

               
            }

            
        }

        private static void runReport(Session session)
        {
            //Create a query criterion for the property state equal to the state of CA or ca
            StringFieldCriterion strCri = new StringFieldCriterion();
            strCri.FieldName = "Fields.2626";
            strCri.MatchType = StringFieldMatchType.CaseInsensitive;
            strCri.Value = "Correspondent";

            QueryCriterion cri = strCri;

            //Build the list of desired fields, using canonical names and reporting database canonical names
            StringList fields = new StringList {
				"Loan.LoanNumber",
				"Loan.BorrowerLastName",
                "Loan.LoanAmount",
				"Fields.2626",
				"Loan.GUID" };

            LoanReportCursor cursor = null;
            try
            {
                cursor = session.Reports.OpenReportCursor(fields, cri);
                // Using the foreach syntax will allow for efficient enumeration over the
                // items in the cursor.

                string fileForThisRun = "loanList" + DateTime.Now.ToLongDateString() + ".txt";

                foreach (LoanReportData loanData in cursor)
                {


                    //Console.WriteLine("{0}, {1}, {2}, {3}, {4}",
                     //   loanData["Loan.LoanNumber"],
                    //    loanData["Loan.LoanAmount"],
                    //    loanData["Loan.BorrowerLastName"],
                    //    loanData["Fields.2626"],
                    //    loanData["Loan.GUID"]);

                    WriteToFile(fileForThisRun, @"C:\temp\Logs\", string.Concat(loanData["Loan.LoanNumber"] + "," + loanData["Loan.BorrowerLastName"] + "," +
                          loanData["Loan.LoanAmount"] + "," + loanData["Fields.2626"]));
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // Close the cursor to ensure its resources are released
                if (cursor != null)
                {
                    cursor.Close();
                }
            }


        }

        private static bool WriteToFile(string fileName, string filePath, string data)
        {

            using (System.IO.StreamWriter file =
             new System.IO.StreamWriter(filePath + fileName, true))
            {
                file.WriteAsync(data + "\r\n");
            }

            return true;
        }

    }
}
