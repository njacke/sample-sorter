using Dapper;
using SampleSorter.Models;
using SampleSorter.Workers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SampleSorter.Repository
{
    class SampleDataCommand
    {
        private string _connectionString;
        private static readonly SampleDataStringGenerator _sampleDataStringGenerator = new SampleDataStringGenerator();
        private static readonly SampleDataBubbleSort _sampleDataBubbleSort = new SampleDataBubbleSort();


        public SampleDataCommand(string connectionString)
        {
            _connectionString = connectionString;
        }


        public void RecreateTable() // drops current table and creates new table
        {
            var sql = "SampleDatabase_RecreateTable";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
            connection.Execute(sql);
            }
        }

        public void BulkInsert(int tableSize) // transaction solution, dapper doesn't support bulk insert, read from csv to save time for larger DB?
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand sqlInsertList = new SqlCommand("INSERT INTO [SampleData] (SampleText) VALUES(@SampleText)", connection);
            IList<string> strings = _sampleDataStringGenerator.StringsBatch(tableSize);
            connection.Open();
            var transaction = connection.BeginTransaction();

            for (int i = 0; i < strings.Count; i++)
            {
                sqlInsertList.Transaction = transaction;
                sqlInsertList.Parameters.Clear();
                sqlInsertList.Parameters.AddWithValue("@SampleText", strings[i]);
                sqlInsertList.ExecuteNonQuery();
            }
            transaction.Commit();
        }

        public IList<SampleDataModel> GetList()
        {
            List<SampleDataModel> samples = new List<SampleDataModel>();

            var sql = "SampleData_GetList";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                samples = connection.Query<SampleDataModel>(sql).ToList();
            }

            return samples;
        }

        public IList<SampleDataModel> GetListSorted() 
        {
            List<SampleDataModel> samples = new List<SampleDataModel>();

            var sql = "SampleData_GetListSorted"; // ASC sorted list procedure

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                samples = connection.Query<SampleDataModel>(sql).ToList();
            }

            return samples;
        }

        public IList<SampleDataModel> GetListBubbleSorted()
        {
            List<SampleDataModel> samples = new List<SampleDataModel>();

            var sql = "SampleData_GetList";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                samples = connection.Query<SampleDataModel>(sql).ToList();
            }

            samples = _sampleDataBubbleSort.BubbleSort(samples); //bubble sort here while still in List<T>
            return samples;
        }
    }
}
