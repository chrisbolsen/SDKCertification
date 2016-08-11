using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.Forms;
using EllieMae.Encompass.BusinessObjects;
using System.Xml.Linq;
using EllieMae.Encompass.Client;


namespace Exam.Part1
{
    ///<summary>
    ///The Custom Form Codebase class
    ///</summary>
    public class Demo13Codebase : EllieMae.Encompass.Forms.Form
    {
        //Override the CreateControls method to subscribe to control events
        public override void CreateControls()
        {
            Button btnSend = (Button)FindControl("Button1");
            btnSend.Click += btnSend_Click;

            this.Load += Demo13Codebase_Load;

            this.Unload += Demo13Codebase_Unload;
        }



        void Demo13Codebase_Load(object sender, EventArgs e)
        {

            try
            {
                DataObject customObject = EncompassApplication.Session.DataExchange.GetCustomDataObject("SDKCert.xml");

                // Parse the file as XML
                XDocument feeDocument = XDocument.Parse(customObject.ToString(Encoding.UTF8));


                ApplicationLog.DebugEnabled = (feeDocument.Root.Value == "1");

                //If Debugging is enabled in Config File turn it on.
               // ApplicationLog.DebugEnabled = Convert.ToBoolean(isDebugEnabled);
                ApplicationLog.WriteDebug("SDKCert", "Debug value from XML Config is : " + ApplicationLog.DebugEnabled);
            }
            catch (Exception ex )
            {
                ApplicationLog.WriteDebug("SDKCert", "exception caught opening custom object : " + ex.Message);
            }
            
        }

        void Demo13Codebase_Unload(object sender, EventArgs e)
        {
            //Turn Off Debugging
            ApplicationLog.DebugEnabled = false;

            this.Load -= Demo13Codebase_Load;

            this.Unload -= Demo13Codebase_Unload;

        }

        //Event handler for Send button's Click event to show an alert stating which user clicked the button
        void btnSend_Click(object sender, EventArgs e)
        {
            ApplicationLog.WriteDebug("SDKCert", "About to update Field");

            EncompassApplication.CurrentLoan.Fields["2626"].Value = "Correspondent";


            ApplicationLog.WriteDebug("SDKCert", "Field 2626 Updated");

        }
    }
}
