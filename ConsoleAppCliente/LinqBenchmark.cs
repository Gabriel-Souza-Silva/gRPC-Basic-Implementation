using BenchmarkDotNet.Attributes;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcServiceApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCliente
{
    public class LinqBenchmark
    {
        private GrpcChannel channel;

        [GlobalSetup]
        public void Setup()
        {
            this.channel = GrpcChannel.ForAddress("http://localhost:5000");
        }

        [Benchmark(Baseline = true)]
        public void WithGrpc()
        {
            var client = new PeopleProtoService.PeopleProtoServiceClient(channel);
            PeopleResponse reply = client.GetPeople(new GetPeopleRequest() { });
            channel.ShutdownAsync().Wait();
            Console.WriteLine("Fim modo Grpc...");
        }

        [Benchmark]
        public void WithApi()
        {
            HttpClient req = new HttpClient();
            var content = req.GetAsync("http://localhost:5003/people").Result;
            List<People> responsePeoples = new List<People>(JsonConvert.DeserializeObject<List<People>>(content.Content.ReadAsStringAsync().Result));
            Console.WriteLine("Fim modo Api...");
        }

        [Benchmark]
        public void PostWithGrpc()
        {
            var client = new PeopleProtoService.PeopleProtoServiceClient(channel);
            PeopleModel reply = client.PostPeople(new PeopleModel() { Birthday = Timestamp.FromDateTime(DateTime.UtcNow), Name = "Gabriel", LastName = "Silva" });
            channel.ShutdownAsync().Wait();
            Console.WriteLine("Fim post Grpc...");
        }

        [Benchmark]
        public void PostWithApi()
        {
            People model = new People()
            {
                Birthday = DateTime.UtcNow,
                Name = "Gabriel",
                LastName = "Silva"
            };
            var myContent = JsonConvert.SerializeObject(model);

            HttpClient req = new HttpClient();

            var content = req.PostAsync("http://localhost:5003/people", new StringContent(myContent, UnicodeEncoding.UTF8, "application/json")).Result;
            People responsePeoples = JsonConvert.DeserializeObject<People>(content.Content.ReadAsStringAsync().Result);
            Console.WriteLine("Fim post Api...");
        }
    }
}
