using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.BusinessObjects.Loans;

namespace DemoApplication
{
    class Demo05Program
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

                try
                {
                    //Retrieve the loan with the GUID from the previous example
                    Loan loan = session.Loans.Open("{915596ad-f07c-4a17-bc6e-d0eecbd9447f}");

                    if (loan != null)
                    {
                        //For the "current" borrower pair, retrieve the employer name from each VOE
                        for (int i = 1; i <= loan.BorrowerEmployers.Count; i++)
                            Console.WriteLine("{0}: {1}", i, loan.Fields.GetFieldAt("BE02", i).FormattedValue);

                        //Use the "static" field IDs instead of the dynamically generated Field IDs from above
                        Console.WriteLine("{0}: {1}", 1, loan.Fields["BE0102"].FormattedValue);
                        Console.WriteLine("{0}: {1}", 2, loan.Fields["BE0202"].FormattedValue);
                        Console.WriteLine("{0}: {1}", 3, loan.Fields["BE0302"].FormattedValue);
                        Console.WriteLine();

                        //Iterate over the borrower pairs
                        for (int i = 0; i < loan.BorrowerPairs.Count; i++)
                        {
                            // Print the Borrower First Name field for the pair
                            BorrowerPair pair = loan.BorrowerPairs[i];
                            Console.WriteLine("Pair #{0}: {1}", i, loan.Fields["4000"].GetValueForBorrowerPair(pair));
                        }

                        Console.WriteLine();

                        //Iterate over the borrower pairs using foreach
                        foreach (BorrowerPair pair in loan.BorrowerPairs)
                        {
                            // Make the pair "current"
                            loan.BorrowerPairs.Current = pair;

                            // For the "current" borrower pair, retrieve the employer name from each VOE
                            for (int i = 1; i <= loan.BorrowerEmployers.Count; i++)
                                Console.WriteLine("{0}/{1}: {2}", i, pair.Borrower, loan.Fields.GetFieldAt("BE02", i).FormattedValue);
                        }

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
                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();
            }
        }
    }
}
