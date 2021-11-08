using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceApi;
using System;
using System.Linq;
using System.Net.Http;

namespace ConsoleAppCliente
{
    class Program
    {
        static void Main(string[] args)
        {
            //var channel = GrpcChannel.ForAddress("http://localhost:5000");
            //var client = new PeopleProtoService.PeopleProtoServiceClient(channel);
            //PeopleModel reply = client.PostPeople(new PeopleModel() { Birthday = DateTime.UtcNow.ToTimestamp(), Name = "Gabriel", LastName = "Silva" });
            //channel.ShutdownAsync().Wait();
            //Console.WriteLine("Fim post Grpc...");

            var config = new ManualConfig()
                    .WithOptions(ConfigOptions.DisableOptimizationsValidator)
                    .AddValidator(JitOptimizationsValidator.DontFailOnError)
                    .AddLogger(ConsoleLogger.Default)
                    .AddColumnProvider(DefaultColumnProviders.Instance);

            BenchmarkRunner.Run<LinqBenchmark>(config);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
