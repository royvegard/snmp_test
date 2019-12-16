using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;

namespace snmp_test
{
    class Program
    {
        static int fetches = 1;
        static async Task Main(string[] args)
        {
            if (args.Length > 0)
            {
                fetches = Convert.ToInt32(args[0]);
            }
            System.Console.WriteLine("{0}", fetches);

            Progress<int> progress = new Progress<int>();
            progress.ProgressChanged += ReportProgress;
            await GetSNMPAsync(progress);
        }

        static private void ReportProgress(object sender, int e)
        {
            System.Console.Write("\rPercentage complete: {0}", e);
        }

        static private async Task GetSNMPAsync(IProgress<int> progress)
        {
            List<Task<IList<Variable>>> tasks = new List<Task<IList<Variable>>>();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < fetches; i++)
            {
                tasks.Add(Task.Run(() => Messenger.Get(VersionCode.V2,
                new IPEndPoint(IPAddress.Parse("104.236.166.95"), 161),
                new OctetString("public"),
                new List<Variable> {
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.2.0")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.3.0")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.4.0")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.5.0")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.6.0")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.7.0")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.8.0")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.9.1.3.1")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.9.1.3.4")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.9.1.3.6")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.9.1.3.8")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.2.2.1.1.1")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.2.2.1.1.2")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.2.2.1.2.1")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.2.2.1.2.2")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.2.2.1.3.1")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.2.2.1.3.2")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.2.2.1.4.1")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.2.2.1.4.2")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.2.2.1.5.1")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.2.2.1.5.2")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.2.2.1.6.1")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.2.2.1.6.2")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.2.2.1.7.1")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.2.2.1.7.2")),
                },
                60000)));

                progress.Report((i * 100) / fetches);
            }

            var results = await Task.WhenAll(tasks);

            watch.Stop();

            System.Console.WriteLine();

            System.Console.WriteLine("Run time: {0} ms", watch.ElapsedMilliseconds);
            System.Console.WriteLine("Variable fetch rate: {0} ms/variable", (double)watch.ElapsedMilliseconds / (double)(fetches * 26) );
        }
    }
}
