using Bogus;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcServiceApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceApi.Services
{
    public class PeopleService : PeopleProtoService.PeopleProtoServiceBase
    {
        private readonly PersonContext _context;
        public PeopleService(PersonContext context)
        {
            _context = context;
        }
        public override async Task<PeopleResponse> GetPeople(GetPeopleRequest request, ServerCallContext context)
        {
            PeopleResponse response = new PeopleResponse();

            Faker fake = new Faker();

            for (int i = 0; i < 7000; i++)
            {
                PeopleModel people = new PeopleModel()
                {
                    Name = fake.Name.FirstName(),
                    LastName = fake.Name.LastName(),
                    Birthday = Timestamp.FromDateTime(TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(fake.Date.Between(new DateTime(1990, 1, 1), DateTime.UtcNow), DateTimeKind.Utc)))
                };

                response.Peoples.Add(people);
            }

            return response;
        }

        public override async Task<PeopleModel> PostPeople(PeopleModel people, ServerCallContext context)
        {
            _context.Add(new People() { Birthday = people.Birthday.ToDateTime(), Name = people.Name, LastName = people.LastName });
            _context.SaveChanges();
            return people;
        }
    }
}
