namespace MsSqlBenchmark
{
	using System;
	using System.Data.SqlClient;
	using System.Diagnostics;
	using System.Linq;

	class Program
	{
		const int NumberOfThreads = 16;
		const int NumberOfIterations = 10000 * NumberOfThreads;

		static void Main(string[] args)
		{
			const string connectionString = "Data Source=localhost;Initial Catalog=InsertBenchmark;Integrated Security=True";

			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();
				new SqlCommand("DELETE From FooBar", connection).ExecuteNonQuery();
			}

			var stopwatch = Stopwatch.StartNew();

			Enumerable.Range(1, NumberOfIterations).AsParallel().WithDegreeOfParallelism(NumberOfThreads).Select(n =>
			{
				using (var connection = new SqlConnection(connectionString))
				{
					connection.Open();

					var command = new SqlCommand("INSERT INTO FooBar(Foo, Bar) VALUES('foo', 'bar')", connection);
					command.ExecuteNonQuery();
				}

				return n;
			}).ToArray();

			Console.WriteLine(stopwatch.Elapsed);
			Console.WriteLine(NumberOfIterations * 1000 / stopwatch.ElapsedMilliseconds);
		}
	}
}
