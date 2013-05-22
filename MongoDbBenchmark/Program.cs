namespace MongoDbBenchmark
{
	using System;
	using System.Diagnostics;
	using System.Linq;
	using MongoDB.Driver;

	class Program
	{
		const int NumberOfIterations = 100000;

		static void Main(string[] args)
		{
			var builder = new MongoConnectionStringBuilder("Server=localhost");
			builder.Journal = true;

			var client = new MongoClient(builder.ToString());

			var server = client.GetServer();
			var database = server.GetDatabase("InsertPerformance");

			database.DropCollection("FooBar");

			var collection = database.GetCollection("FooBar");

			var stopwatch = Stopwatch.StartNew();

			Enumerable.Range(1, NumberOfIterations).AsParallel().WithDegreeOfParallelism(8).Select(n =>
			{
				collection.Insert(new FooBar("foo", "bar"));

				return n;
			}).ToArray();

			Console.WriteLine(stopwatch.Elapsed);
			Console.WriteLine(NumberOfIterations * 1000 / stopwatch.ElapsedMilliseconds);
		}
	}

	class FooBar
	{
		public FooBar(string foo, string bar)
		{
			Foo = foo;
			Bar = bar;
		}

		public string Foo { get; private set; }

		public string Bar { get; private set; }
	}
}
