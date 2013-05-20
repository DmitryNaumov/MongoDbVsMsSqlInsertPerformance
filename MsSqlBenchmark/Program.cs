namespace MsSqlBenchmark
{
	using System;
	using System.Data.SqlClient;
	using System.Diagnostics;

	class Program
	{
		static void Main(string[] args)
		{
			const int numberOfIterations = 10000;

			const string connectionString = "Data Source=localhost;Initial Catalog=InsertBenchmark;Integrated Security=True";

			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();
				new SqlCommand("DELETE From FooBar", connection).ExecuteNonQuery();
			}

			var stopwatch = Stopwatch.StartNew();

			int n = numberOfIterations;
			while (n-- > 0)
			{
				using (var connection = new SqlConnection(connectionString))
				{
					connection.Open();

					var command = new SqlCommand("INSERT INTO FooBar(Foo, Bar) VALUES('foo', 'bar')", connection);
					command.ExecuteNonQuery();
				}
			}

			Console.WriteLine(stopwatch.Elapsed);
			Console.WriteLine(numberOfIterations * 1000 / stopwatch.ElapsedMilliseconds);
		}
	}
}
