namespace MongoDbBenchmark
{
	using System.Diagnostics;
	using System.Linq;
	using System.Threading;
	using MongoDB.Driver;

	class Benchmark
	{
		private readonly MongoServer _server;

		public Benchmark(MongoServer server)
		{
			_server = server;
		}

		public long Run(FooBar document, int numberOfThreads)
		{
			Prepare();

			var database = _server.GetDatabase("InsertPerformance");
			var collection = database.GetCollection("FooBar");

			var numberOfIterations = 10000 * numberOfThreads;

			var stopwatch = Stopwatch.StartNew();

			Enumerable.Range(1, numberOfIterations).AsParallel().WithDegreeOfParallelism(numberOfThreads).Select(n =>
			{
				collection.Insert(document);

				return n;
			}).ToArray();

			return stopwatch.ElapsedMilliseconds;
		}

		private void Prepare()
		{
			var database = _server.GetDatabase("InsertPerformance");
			database.DropCollection("FooBar");

			var adminDatabase = _server.GetDatabase("admin");
			var fsync = new CommandDocument { { "fsync", 1 } };
			adminDatabase.RunCommand(fsync);
			
			Thread.Sleep(5000);
		}
	}
}