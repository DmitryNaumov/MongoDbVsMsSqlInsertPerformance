namespace MongoDbBenchmark
{
	using System;
	using System.Linq;
	using MongoDB.Bson;
	using MongoDB.Driver;

	class Program
	{
		static void Main(string[] args)
		{
			var builder = new MongoConnectionStringBuilder("Server=localhost");
			builder.Journal = true;

			var client = new MongoClient(builder.ToString());

			var server = client.GetServer();
			var benchmark = new Benchmark(server);

			var threadCount = new[] { 1, 2, 4, 8, 16 };
			var arraySizes = new[] { 10, 100, 200, 500, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500, 1600, 1700, 1800, 1900, 2000, 5000, 10000, 20000, 50000, 100000 };

			const int attempts = 5;
			const string format = "|{0, 7}|{1, 13}|{2, 7}|";
			Console.WriteLine(format, "Threads", "Document Size", "Elapsed");

			foreach (var numberOfThreads in threadCount)
			{
				foreach (var sizeOfArray in arraySizes)
				{
					var document = new FooBar("foo", sizeOfArray);
					var documentSize = document.ToBson().Length;

					var elapsed = Enumerable.Repeat(1, attempts).Select(x =>
						{
							var time = benchmark.Run(document, numberOfThreads);
							Console.Write(".");

							return time;
						}).Min();

					Console.CursorLeft = 0;
					Console.WriteLine(format, numberOfThreads, documentSize, elapsed);
				}
			}
		}
	}

	class FooBar
	{
		public FooBar(string foo, int n)
		{
			Foo = foo;
			Bar = Enumerable.Range(1, n).ToArray();
		}

		public string Foo { get; private set; }

		public int[] Bar { get; private set; }
	}
}
