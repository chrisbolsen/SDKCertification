using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.Query;

namespace DemoApplication
{
    class Demo08Program
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

                //Create a query criterion for a loan amount greater than $200,000
                NumericFieldCriterion amtCri = new NumericFieldCriterion();
                amtCri.FieldName = "Loan.LoanAmount";
                amtCri.MatchType = OrdinalFieldMatchType.GreaterThan;
                amtCri.Value = 200000;

                //Create a query criterion for the property state equal to the state of CA or ca
                StringFieldCriterion stateCri = new StringFieldCriterion();
                stateCri.FieldName = "Loan.State";
                stateCri.MatchType = StringFieldMatchType.CaseInsensitive;
                stateCri.Value = "CA";

                //(Loan Amount greater than $200,000) and (Property state equal to the state of CA or ca)
                QueryCriterion cri = amtCri.And(stateCri);

                //Start a stopwatch
                Stopwatch timer = Stopwatch.StartNew();

                //Run an object that displays a list of objects that has the GUID and loan number
                LoanIdentityList loanIds = session.Loans.Query(cri);

                //Iterate over the matching loans
                foreach (LoanIdentity id in loanIds)
                {
                    using (Loan loan = session.Loans.Open(id.Guid))
                        Console.WriteLine("{0}, {1}, {2}", loan.LoanNumber, loan.Fields["1109"].FormattedValue,
                            loan.Fields["14"].FormattedValue);
                }

                //Display the elasped time
                Console.WriteLine("Elapsed time: " + timer.ElapsedMilliseconds);

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
                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();
            }
        }
    }
}
