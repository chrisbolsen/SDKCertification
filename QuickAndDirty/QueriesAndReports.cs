using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.Query;
using System.Diagnostics;

namespace QuickAndDirty
{
    partial class LoanOperations
    {


        private static void FindLoans(Session session)
        {
            try
            {
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
                //LoanIdentityList loanIds = session.Loans.Query(cri);
               
                SortCriterionList sort = new SortCriterionList();
                sort.Add(new SortCriterion("Loan.LoanAmount"));


                PipelineCursor pipeLineCursor = session.Loans.QueryPipelineEx(cri, sort);

                //Iterate over the matching loans
                try
                  {
                     // Using the foreach syntax will allow for efficient enumeration over the
                     // items in the cursor.
                      foreach (PipelineData data in pipeLineCursor)
                     {
                         WriteToFile("loanList.csv", @"C:\temp\Logs\", string.Concat(data["LoanNumber"] + "," + data["LoanAmount"] + "," +
                                data["State"]));
                     }
                  }
                  finally
                  {
                     // Close the cursor to ensure its resources are released
                      pipeLineCursor.Close();
                  }

                        //Console.WriteLine("{0}, {1}, {2}", loan.LoanNumber, loan.Fields["1109"].FormattedValue,
                        //    loan.Fields["14"].FormattedValue);



                //Display the elasped time
                Console.WriteLine("Elapsed time: " + timer.ElapsedMilliseconds);

            }
            finally
            {

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
