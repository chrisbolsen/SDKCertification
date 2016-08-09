using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.BusinessObjects.Loans;

namespace DemoApplication
{
    class Demo04Program
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
            // Create a Session object -- it will be "disconnected" to start
            Session session = new Session();

            try
            {
                // Connect to our local Encompass system
                session.Start("http://localhost", "admin", "support");

                try
                {
                    // Retrieve the loan with the GUID from the previous example
                    Loan loan = session.Loans.Open("{915596ad-f07c-4a17-bc6e-d0eecbd9447f}");

                    if (loan != null)
                    {
                        // Lock the loan for editing
                        loan.Lock();

                        //Disable calculations on a loan when doing numerous field updates that 
                        //will cause loan calculations to run and slow down the application
                        loan.CalculationsEnabled = false;

                        //Notify user the loan is locked with calculations disabled
                        Console.WriteLine("Loan is locked and calculations have been disabled.");

                        //Modify the borrower's phone number, email, note rate, lock date and number of days
                        loan.Fields["66"].Value = "1234567890";
                        loan.Fields["1240"].Value = "msmith@email.com";
                        loan.Fields["3"].Value = "6";
                        loan.Fields["761"].Value = DateTime.Today;
                        loan.Fields["432"].Value = "30";

                        //Since calculations were disabled, enable them and do recalculate to force calculations to run
                        loan.CalculationsEnabled = true;
                        loan.Recalculate();

                        //Save the changes
                        loan.Commit();

                        //Notify user the loan has been updated and saved
                        Console.WriteLine("Loan has been updated, recalculated and saved.");

                        //Remove lock
                        loan.Unlock();

                        //Close the loan
                        loan.Close();
                    }
                }

                //Handle a lock exception
                catch (LockException)
                {
                    Console.WriteLine("The loan is locked by another user. Press any key to exit.");
                    Console.ReadLine();
                }

                //Handle a system exception
                catch (SystemException)
                {
                    Console.WriteLine("The Application user does not have the required access to the specified loan.");
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadLine();
                }

                //Handle other exceptions
                catch (Exception)
                {
                    Console.WriteLine("An exception occurred other than a Lock or System Exception.");
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadLine();
                }

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
                Console.WriteLine("The session has ended.");
                Console.WriteLine("Press any key to exit.");
                Console.ReadLine();
            }

        }
    }
}
