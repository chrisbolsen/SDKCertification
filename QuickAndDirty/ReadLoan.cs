using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EllieMae.Encompass.Client;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.Query;

namespace QuickAndDirty
{
    partial class LoanOperations
    {
        private static void ReadLoan(Session session, string loanGUID)
        {
            try
            {
                Loan loan = session.Loans.Open(loanGUID);
                if (loan != null)
                {
                    loan.Lock();
                    Console.WriteLine("Opened Loan for " + loan.Fields["4000"] + " " + loan.Fields["4002"]);
                    //Unlock the loan
                    loan.Unlock();

                    //Close the loan
                    loan.Close();
                }

            }

            catch (Exception ex)
            {

                Console.WriteLine("couldn't find Loan " + ex.Message);

            }
        }

    }
}
