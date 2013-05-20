namespace MsSqlBenchmark
{
	using System;
	using System.Data.SqlClient;
	using System.Diagnostics;

	class Program
	{
		const int NumberOfIterations = 10000;

		static void Main(string[] args)
		{
			const string connectionString = "Data Source=localhost;Initial Catalog=InsertBenchmark;Integrated Security=True";

			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();
				new SqlCommand("DELETE From FooBar", connection).ExecuteNonQuery();
			}

			var stopwatch = Stopwatch.StartNew();

			int n = NumberOfIterations;
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
			Console.WriteLine(NumberOfIterations * 1000 / stopwatch.ElapsedMilliseconds);
		}
	}
}
