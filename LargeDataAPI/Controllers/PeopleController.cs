using Bogus;
using LargeDataAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LargeDataAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PeopleController : ControllerBase
    {

        private readonly ILogger<PeopleController> _logger;
        private readonly PersonContext _context;

        public PeopleController(PersonContext context,ILogger<PeopleController> logger)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<People> Get()
        {
            List<People> peoples = new List<People>();

            Faker fake = new Faker();

            for (int i = 0; i < 7000; i++)
            {
                People people = new People()
                {
                    Name = fake.Name.FirstName(),
                    LastName = fake.Name.LastName(),
                    Birthday = fake.Date.Between(new DateTime(1990, 1, 1), DateTime.Now)
                };

                peoples.Add(people);
            }

            return peoples;
        }


        [HttpPost]
        public IActionResult Post(People people)
        {
            _context.Add(people);
            _context.SaveChanges();
            return Ok(people);
        }
    }
}
