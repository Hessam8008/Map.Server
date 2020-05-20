// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Golriz">
//   Copy-right © 2020
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GpsServer
{
    using GpsServer.Teltonika.Server;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        public static void Main(string[] args)
        {
            try
            {
                var server = new TeltonikaServer();
                var task = new Task(async () => { await server.Start(); });
                task.Start();

                Console.ReadKey();

                server.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
        }
    }
}
