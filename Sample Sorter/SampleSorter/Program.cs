using SampleSorter.Models;
using SampleSorter.Repository;
using SampleSorter.Workers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SampleSorter
{
    class Program
    {
        private static string _connectionString = @"Data Source=localhost;Initial Catalog=SampleDatabase;Integrated Security=True"; //replace if different conection string
        private static readonly SampleDataCommand _sampleDataCommand = new SampleDataCommand(_connectionString);
        private static readonly SampleDataChartCreator _sampleDataChartCreator = new SampleDataChartCreator();
        private static int _tableSizeOneHundred = 100;
        private static int _tableSizeOneThousand = 1000;
        private static int _tableSizeTenThousand = 10000;
        private static int _tableSizeOneHundredThousand = 100000;
        private static int _rowsDisplayed = 100;
        private static int[] _tests = new[] { _tableSizeOneHundred, _tableSizeOneThousand, _tableSizeTenThousand, _tableSizeOneHundredThousand };

        static void Main(string[] args)
        {
            try
            {
                RecreateTableAndInsert(_tableSizeOneThousand);
                GetSamples();
                var continueManaging = true;

                do
                {
                    Console.WriteLine("1 - Get list | 2 - DB sort by SampleText | 3 - Bubble sort by SampleText | 4 - Analysis and report | 5 - Exit");
                    int option = Convert.ToInt32(Console.ReadLine());

                    switch (option)
                    {
                        case 1:
                            GetSamples();
                            break;
                        case 2:
                            GetSamplesSortedDB();
                            break;
                        case 3:
                            GetSamplesSortedBubble();
                            break;
                        case 4:
                            AnalysePerformanceAndPlot();
                            RecreateTableAndInsert(_tableSizeOneThousand);
                            break;
                        case 5:
                            continueManaging = false;
                            break;
                        default:
                            Console.WriteLine("Option not found.");
                            break;
                    }

                } while (continueManaging == true);            
            }

            catch(Exception ex)
            {
                Console.WriteLine("Something went wrong: {0}", ex.Message);
            }            
        }

        private static void RecreateTableAndInsert(int tableSize)
        {
            Console.WriteLine("Creating table ({0} rows)...", tableSize);
            var timeOne = System.Diagnostics.Stopwatch.StartNew();
            _sampleDataCommand.RecreateTable();
            _sampleDataCommand.BulkInsert(tableSize);
            timeOne.Stop();
            var timeOneMs = timeOne.ElapsedMilliseconds;
            Console.WriteLine("Table creation execution time: {0}ms", timeOneMs);
        }

        private static void GetSamples()
        {
            IList<SampleDataModel> samples = _sampleDataCommand.GetList();

            DisplaySamples(samples);
        }

        private static void GetSamplesSortedDB()
        {
            var timeOne = System.Diagnostics.Stopwatch.StartNew();

            IList<SampleDataModel> samples = _sampleDataCommand.GetListSorted();

            timeOne.Stop();
            var timeOneMs = timeOne.ElapsedMilliseconds;

            DisplaySamples(samples);

            Console.WriteLine("\nDB sort execution time: {0}ms", timeOneMs);
        }


        private static void GetSamplesSortedBubble()
        {
            var timeOne = System.Diagnostics.Stopwatch.StartNew();

            IList<SampleDataModel> samples = _sampleDataCommand.GetListBubbleSorted();

            timeOne.Stop();
            var TimeOneMs = timeOne.ElapsedMilliseconds;

            DisplaySamples(samples);

            Console.WriteLine("\nBubble sort execution time: {0}ms", TimeOneMs);
        }


        public static void AnalysePerformanceAndPlot()
        {
            List<List<double>> performanceTimes = new List<List<double>>();

            foreach(int test in _tests)
            {
                RecreateTableAndInsert(test);
                var performanceTimesTest = AnalysePerformance(test);
                performanceTimes.Add(performanceTimesTest);
            }

            string workingDirectory = Environment.CurrentDirectory;
            Console.WriteLine("Creating PerformanceReport.xls in {0}", Directory.GetParent(workingDirectory).Parent.Parent.FullName);

            _sampleDataChartCreator.ChartCreator(performanceTimes);

            Console.WriteLine("Analysis completed.");
        }

        private static List<double> AnalysePerformance(int tableSize)
        {
            Console.WriteLine("Testing with DB sort...");
            var timeDb = System.Diagnostics.Stopwatch.StartNew();

            _sampleDataCommand.GetListSorted();

            timeDb.Stop();
            var timeDbMs = timeDb.ElapsedMilliseconds;
            Console.WriteLine("DB sort execution time: {0}ms", timeDbMs);

            Console.WriteLine("Testing with Bubble sort...");
            var timeBubble = System.Diagnostics.Stopwatch.StartNew();

            _sampleDataCommand.GetListBubbleSorted();

            timeBubble.Stop();
            var timeBubbleMs = timeBubble.ElapsedMilliseconds;
            Console.WriteLine("Bubble sort execution time: {0}ms", timeBubbleMs);

            List<double> performanceTimes = new List<double>{ Convert.ToDouble(tableSize), Convert.ToDouble(timeDbMs), Convert.ToDouble(timeBubbleMs) };
            return performanceTimes;
        }

        private static void DisplaySamples(IList<SampleDataModel> samples)
        {
            if (samples.Any())
            {
                
                if (_rowsDisplayed >= samples.Count) //special case
                {
                    Console.WriteLine("ID  Text");
                    foreach (SampleDataModel sample in samples)
                    {
                        Console.WriteLine("{0} {1}",
                            sample.SampleId,
                            sample.SampleText);
                    }
                    Console.WriteLine("\nAll rows are displayed in the console.", _rowsDisplayed);
                }
                else
                {
                    Console.WriteLine("ID  Text");
                    for (int i = 0; i < _rowsDisplayed; i++)
                    {
                        Console.WriteLine("{0} {1}",
                            samples[i].SampleId,
                            samples[i].SampleText);
                    }
                    Console.WriteLine("\nOnly first {0} rows are displayed in the console.", _rowsDisplayed);
                }      
            }
        }
    }
}
