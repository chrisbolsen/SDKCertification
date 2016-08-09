using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.BusinessObjects.Loans;

namespace DemoApplication
{
    class Demo02Program
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

            //Try to create an Encompass session and handle possible exceptions
            try
            {
                //Connect to our local Encompass system
                session.Start("http://localhost", "admin", "support");

                //Create a new Loan object. After calling CreateNew(), the loan is in memory on the client
                //and has not been saved to the server.
                Loan loan = session.Loans.CreateNew();

                //Populate the first and last name of the borrower
                loan.Fields["4000"].Value = "John";
                loan.Fields["4002"].Value = "Homeowner";

                //Set the loan folder into which the loan will be saved
                loan.LoanFolder = "My Pipeline";

                //Save the loan
                loan.Commit();

                //Let us know it succeeded
                Console.WriteLine("Loan created with GUID = " + loan.Guid);

                //Unlock the loan
                loan.Unlock();

                //Close the loan
                loan.Close();

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
                Console.WriteLine("There was an exception other than a login exception. Press any key to exit.");
                Console.ReadLine();
            }

            finally
            {
                //End the session to gracefully log out of the server
                session.End();

                //Wait for user input
                Console.WriteLine("The session has ended. Press Enter to exit...");
                Console.ReadLine();
            }
        }
    }
}
