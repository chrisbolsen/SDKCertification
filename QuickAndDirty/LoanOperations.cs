using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Configuration;


using EllieMae.Encompass.Client;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.Query;

namespace QuickAndDirty
{
    partial class LoanOperations
    {
        static void Main(string[] args)
        {

            //Initialize the Encompass API Runtime Services
            new EllieMae.Encompass.Runtime.RuntimeServices().Initialize();

            //Invoke your own internal "main" routine
            ExecuteApp();
        }

        static void ExecuteApp()
        {
         Session session = new Session();

         var appSettings = ConfigurationManager.AppSettings;

         string _URL = appSettings["EncompassURL"] ?? "local";
         string _UID = appSettings["UID"] ?? "admin";
         string _PWD = appSettings["PW"] ?? "password";
         string _LoanGUID = appSettings["LOANGUID"] ?? "";

            try
            {
                // Connect to our local Encompass system
                session.Start(_URL, _UID, _PWD);

                session.ServerEvents.SessionMonitor += ServerEvents_SessionMonitor;

                var newLoanNumber = "";
                int choice = int.Parse("0");

                do
                {
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("Select Your Loan Operation");
                    Console.WriteLine("1. Import a Loan");
                    Console.WriteLine("2. Create a New Loan");
                    Console.WriteLine("3. Run a Batch Update");
                    Console.WriteLine("4. Read A loan by GUID ");
                    Console.WriteLine("5. Query to File");
                    Console.WriteLine("");
                    Console.WriteLine("Hit 0 to Exit");

                    choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1: // Import a Loan
                            string importFile = @"C:\SDK Training\Resources\AliceFirstimer.fnm";
                            newLoanNumber = ImportLoan(session, importFile, LoanImportFormat.FNMA3X);
                            Console.WriteLine("Loan Import Complete");
                            break;

                        case 2: //Create a New Loan
                            newLoanNumber = _LoanGUID == null ? CreateLoan(session) : _LoanGUID;
                            break;
                        case 3: //Run a Batch Update
                            //QueryCriterion cri = new DateFieldCriterion("Loan.DateFileOpened", new DateTime(2013, 1, 1), OrdinalFieldMatchType.GreaterThanOrEquals, DateFieldMatchPrecision.Exact);
                            QueryCriterion cri = new NumericFieldCriterion("Loan.LoanAmount", (double)400000, OrdinalFieldMatchType.GreaterThanOrEquals);
                            BatchUpdate(session, cri); 
                            break;
                        case 4: //Read A loan by GUID
                            ///Read Loan
                            Console.WriteLine("Enter Loan GUID or Hit Enter for Default Load");
                            string inputGUID = Console.ReadLine();
                            newLoanNumber = inputGUID == "" ? _LoanGUID : inputGUID;
                            ReadLoan(session, newLoanNumber);
                            break;
                        case 5: //Query to File
                            FindLoans(session);
                            break;
                    }

                } while (choice != 0);


            }

            //Handle a login exception
            catch (LoginException ex)
            {
                Console.WriteLine("There was a login exception" + ex.Message + "Press any key to exit.");
                Console.ReadLine();
            }

            //Handle a connection exception
            catch (ConnectionException ex)
            {
                Console.WriteLine("The application was unable to establish a connection to the Encompass Server." + ex.Message);
                Console.WriteLine("Press any key to exit.");
                Console.ReadLine();
            }

            //Handle other exceptions
            catch (VersionException ex)
            {
                Console.WriteLine("There was an exception with the Version, not compatable with Encompass Server." + ex.Message);
                Console.WriteLine("Press any key to exit.");
                Console.ReadLine();
            }
            //Handle other exceptions
            catch (Exception ex)
            {
                Console.WriteLine("There was an exception other than a login or connection exception." + ex.Message);
                Console.WriteLine("Press any key to exit.");
                Console.ReadLine();
            }

            finally
            {
                // End the session to gracefully log out of the server
                if (session != null)
                    session.End();

                // Use the Session.IsConnected property to verify we're connected to the server
                //Console.WriteLine("Are we connected: " + session.IsConnected);

                // Wait for user input
                Console.WriteLine("The session has ended. Press any key to exit.");
                Console.ReadLine();
            }
        }

        static void ServerEvents_SessionMonitor(object sender, SessionMonitorEventArgs e)
        {
            Debug.WriteLine(e.ToString() + "Bad things happened");
        }


      
    }
}
