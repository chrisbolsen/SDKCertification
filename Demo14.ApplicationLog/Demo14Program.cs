using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.BusinessObjects;
using EllieMae.Encompass.BusinessObjects.Loans;

namespace Demo14.ApplicationLogging
{
    class Demo14Program
    {
        static void Main()
        {
            //Initialize runtime services
            new EllieMae.Encompass.Runtime.RuntimeServices().Initialize();

            //Invoke "main" method
            startApplication();
        }

        private static void startApplication()
        {
            StringBuilder tim1 = new StringBuilder();
            DateTime t1, t2;

            //Enable Application log debugging
            ApplicationLog.DebugEnabled = true;

            //create a new session
            Session session = new Session();

            // writes to the Session.log if Debug is enabled.
            ApplicationLog.WriteDebug("SDKCert", "Begin Session.Start()");
            t1 = DateTime.Now;

            //start the session
            session.Start("http://localhost/", "admin", "support");
            t2 = DateTime.Now;
            tim1.Clear();

            //   writes to Session.log how long it took to open a Session
            tim1.Append("Session.Start():" + (t2 - t1));
            ApplicationLog.WriteDebug("SDKCert", tim1.ToString());

            tim1.Clear();

            // see if debug is enable.  Write it to the Session.log whether or not Debug is enabled.  
            tim1.Append("DebugEnabled: " + ApplicationLog.DebugEnabled + ", Path = " + ApplicationLog.Path);
            Console.WriteLine(tim1.ToString());
            ApplicationLog.Write("SDKCert", tim1.ToString());

            // Open a loan for reading
            ApplicationLog.WriteDebug("SDKCert", "Begin Loan.Open()");
            t1 = DateTime.Now;
            Loan loan = session.Loans.Open("{915596ad-f07c-4a17-bc6e-d0eecbd9447f}");
            t2 = DateTime.Now;

            tim1.Clear();
            tim1.Append("Loan.Open():" + (t2 - t1));

            // write how long it took to open this loan.
            ApplicationLog.WriteDebug("SDKCert", tim1.ToString());
            Console.WriteLine(tim1.ToString());

            ApplicationLog.DebugEnabled = false;
            Console.ReadLine();

            //end the session
            session.End();
        }
    }
}
