using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;

namespace DemoApplication
{
    class Demo07Program
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
                        //Lock the loan for editing
                        loan.Lock();

                        //Wait for user input
                        Console.WriteLine("Loan is locked.");

                        //Load a PDF as an attachment
                        Attachment att = loan.Attachments.Add(@"C:\SDK Training\Bank Statement.pdf");

                        //Retrieve the Bank Statements document
                        LogEntryList statements = loan.Log.TrackedDocuments.GetDocumentsByTitle("Bank Statement");
                        TrackedDocument doc = (TrackedDocument)statements[0];

                        //Assign the attachment to the document
                        doc.Attach(att);

                        //Save the changes
                        loan.Commit();

                        //Unlock the loan
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
                //End the session to gracefully log out of the server
                session.End();

                //Wait for user input
                Console.WriteLine("Press any key to exit.");
                Console.ReadLine();
            }
        }
    }
}
