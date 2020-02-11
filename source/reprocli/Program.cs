﻿using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace reprocli
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var count = int.Parse(args[0]);
                var connectionString = args[1];

                var total = Stopwatch.StartNew();


               PrepareData(connectionString);
                total.Restart();
                Enumerable.Range(0,count)
                    .AsParallel()
                    .WithDegreeOfParallelism(count)
                    .ForAll(n => Scenario4(connectionString, n));
                Console.WriteLine($"Total: {total.Elapsed}");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void Scenario4(string connString, int number)
        {
            var userStopWatch = Stopwatch.StartNew();

            var buffer = new object[100];
            for (var i = 0; i < 210; i++)
            {
                var queryStopWatch = Stopwatch.StartNew();

                var query = @"SELECT * From TestTable";

                using (var connection = new SqlConnection(connString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        using (var command = new SqlCommand(query, connection, transaction))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    reader.GetValues(buffer);
                                }
                            }
                        }

                        transaction.Commit();
                    }
                }

                queryStopWatch.Stop();
                Console.WriteLine($"Number: {number}. Query: {i} Time: {queryStopWatch.Elapsed}");
            }

            userStopWatch.Stop();
            Console.WriteLine($"Number: {number}. All Queries. Time: {userStopWatch.Elapsed}");
        }



        static void PrepareData(string connString)
        {
            var createTable = @"
                DROP TABLE IF EXISTS TestTable;
                CREATE TABLE TestTable
                (
                    [Id] [nvarchar](50) NOT NULL PRIMARY KEY,
                    [Name] [nvarchar](20) NOT NULL
                );";

            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    using (var command = new SqlCommand(createTable, connection, transaction))
                    {
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }

        }
    }
}
