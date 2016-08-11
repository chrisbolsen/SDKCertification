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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="importPath"></param>
        /// <param name="loanImportFormat"></param>
        /// <returns>Returns New Loan GUID</returns>
        private static string ImportLoan(Session session, string importPath, LoanImportFormat loanImportFormat)
        {


            Loan loan = session.Loans.Import(importPath, loanImportFormat);

            loan.LoanFolder = "My Pipeline";

            loan.Fields["2024"].Value = "My SDK App";
            // Commit the changes and unlock the loan
            loan.Commit();

            Console.WriteLine("New Loan Imported  " + loan.Guid);
            string loanGUID = loan.Guid;


            //Unlock the loan
            loan.Unlock();

            //Close the loan
            loan.Close();

            return loanGUID;

        }



        private static string CreateLoan(Session session)
        {
            Loan loan = session.Loans.CreateNew();

            //Populate the first and last name of the borrower
            loan.Fields["4000"].Value = "John";
            loan.Fields["4002"].Value = "Homeowner";

            //Set the loan folder into which the loan will be saved
            loan.LoanFolder = "My Pipeline";

            //Save the loan
            loan.Commit();

            string loanGUID = loan.Guid;


            //Unlock the loan
            loan.Unlock();

            //Close the loan
            loan.Close();
            //Let us know it succeeded
            return loanGUID;
        }

    }
}
