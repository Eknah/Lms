using Bogus;
using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data
{
    public class SeedData
    {
        public static async Task InitAsync(LmsApiContext db)
        {
            var faker = new Faker("sv");
            var courses = new List<Course>();

            for (var i = 0; i < 20; i++)
            {
                var course = new Course()
                {
                    Title = faker.Random.Word(),
                    StartDate = faker.Date.Soon()
                };

                var numModules = 1 + Random.Shared.Next(5);

                for (var j = 0; j < numModules; j++)
                {
                    var module = new Module()
                    {
                        Title = faker.Random.Word(),
                        StartDate = faker.Date.Soon()
                    };

                    course.Modules.Add(module);
                }

                courses.Add(course);
            }

            await db.AddRangeAsync(courses);
            await db.SaveChangesAsync();
        }
    }
}
