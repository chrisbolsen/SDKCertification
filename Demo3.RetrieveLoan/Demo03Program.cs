using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.BusinessObjects.Loans;

namespace DemoApplication
{
    class Demo03Program
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
            //Create a Session object
            Session session = new Session();

            //Try to create an Encompass session and handle possible exceptions
            try
            {
                //Connect to our local Encompass system
                session.Start("http://localhost", "admin", "support");

                //Try to open the loan file and handle possible exceptions
                try
                {
                    //Retrieve the loan with the GUID from the previous example
                    Loan loan = session.Loans.Open("{915596ad-f07c-4a17-bc6e-d0eecbd9447f}");

                    if (loan != null)
                    {
                        //Write the borrower's name and loan number
                        Console.WriteLine("Borrower Name: {0} {1}",
                            loan.Fields["4000"].FormattedValue,
                            loan.Fields["4002"].FormattedValue);
                        Console.WriteLine("Loan Number:   {0}",
                            loan.LoanNumber);

                        //Close the loan
                        loan.Close();
                    }
                    else
                    {
                        Console.WriteLine("The loan was not opened. Press any key to continue.");
                        Console.ReadLine();
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
                //End the session to gracefully log out of the server
                session.End();

                //Wait for user input
                Console.WriteLine("The session has ended. Press any key to exit.");
                Console.ReadLine();
            }
        }
    }
}
