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
        private static void BatchUpdate(Session session, QueryCriterion cri)
        {
            //cri = new DateFieldCriterion("Loan.DateFileOpened", new DateTime(2013, 1, 1), OrdinalFieldMatchType.GreaterThanOrEquals, DateFieldMatchPrecision.Exact);
            BatchUpdate loanBatch = new BatchUpdate(cri);


            loanBatch.Fields.Add("319", "1234 Main Street");
            loanBatch.Fields.Add("313", "Las Vegas");
            loanBatch.Fields.Add("321", "NV");
            loanBatch.Fields.Add("323", "89032");

            session.Loans.SubmitBatchUpdate(loanBatch);



            Console.WriteLine("Batch Update Complete!");
        }

    }
}
