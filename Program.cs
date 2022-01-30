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
        static int threads = 1;
        static async Task Main(string[] args)
        {
            if (args.Length > 0)
            {
                threads = Convert.ToInt32(args[0]);
            }
            System.Console.WriteLine("Threads: {0}", threads);

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
            for (int i = 0; i < threads; i++)
            {
                tasks.Add(Task.Run(() => Messenger.Get(VersionCode.V2,
                new IPEndPoint(IPAddress.Parse("35.157.118.224"), 161),
                new OctetString("public"),
                new List<Variable> {
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.4.0")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.5.0")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.6.0")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.7.0")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.9.1.3.1")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.9.1.3.2")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.9.1.3.3")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.9.1.3.4")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.9.1.3.5")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.9.1.3.6")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.9.1.3.7")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.9.1.3.8")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.25.1.1.0")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.25.1.4.0")),
                    new Variable(new ObjectIdentifier("1.3.6.1.2.1.25.1.6.0")),
                },
                1000)));

                progress.Report((i * 100) / threads);
            }

            var results = await Task.WhenAll(tasks);

            watch.Stop();

            System.Console.WriteLine();

            System.Console.WriteLine("Run time: {0} ms", watch.ElapsedMilliseconds);
            System.Console.WriteLine("Variables fetched: {0}", (double)(threads * results[0].Count));
            System.Console.WriteLine("Variable fetch rate: {0} ms/variable", (double)watch.ElapsedMilliseconds / (double)(threads * results[0].Count));
        }
    }
}
