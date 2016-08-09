using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using EllieMae.Encompass.Client;

namespace DemoApplication
{
    class Demo01Program
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
            // Create a Session object -- it will be "disconnected" to start
            Session session = new Session();          

            try
            {
                // Use the Session.IsConnected property to verify we're connected to the server
                Console.WriteLine("Are we connected: " + session.IsConnected);
                Stopwatch s = new Stopwatch();
                s.Start();

                // Connect to our local Encompass system
                session.Start("http://localhost", "admin", "support");
                s.Stop();

                // Use the Session.IsConnected property to verify we're connected to the server
                Console.WriteLine("Are we connected: " + session.IsConnected);
                Console.WriteLine("Start: " + s.Elapsed.ToString());

            }

            //Handle a login exception
            catch (LoginException ex)
            {
                Console.WriteLine("There was a login exception. Press any key to exit." + ex.Message);
                Console.ReadLine();
            }

            //Handle a connection exception
            catch (ConnectionException ex)
            {
                Console.WriteLine("The application was unable to establish a connection to the Encompass Server." + ex.Message);
                Console.WriteLine("Press any key to exit.");
                Console.ReadLine();
            }

            //Handle other exceptions
            catch (VersionException ex)
            {
                Console.WriteLine("There was an exception with the Version, not compatable with Encompass Server." + ex.Message);
                Console.WriteLine("Press any key to exit.");
                Console.ReadLine();
            }
            //Handle other exceptions
            catch (Exception ex)
            {
                Console.WriteLine("There was an exception other than a login or connection exception." + ex.Message);
                Console.WriteLine("Press any key to exit.");
                Console.ReadLine();
            }

            finally
            {
                // End the session to gracefully log out of the server
                if (session != null)
                    session.End();

                // Use the Session.IsConnected property to verify we're connected to the server
                Console.WriteLine("Are we connected: " + session.IsConnected);

                // Wait for user input
                Console.WriteLine("The session has ended. Press any key to exit.");
                Console.ReadLine();
            }
        }
    }
}
