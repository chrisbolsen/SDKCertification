using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.Forms;

namespace Demo13.EventHandling
{
    ///<summary>
    ///The Custom Form Codebase class
    ///</summary>
    public class Demo13Codebase : EllieMae.Encompass.Forms.Form
    {
        //Override the CreateControls method to subscribe to control events
        public override void CreateControls()
        {
            Button btnSend = (Button)FindControl("btnSend");
            btnSend.Click += btnSend_Click;
        }

        //Event handler for Send button's Click event to show an alert stating which user clicked the button
        void btnSend_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show(EncompassApplication.Screens, "Thank you, " + EncompassApplication.CurrentUser + ", for sending this information.");
        }
    }
}
