using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    internal class CourseRepository : ICourseRepository
    {
        private readonly LmsApiContext db;

        public CourseRepository(LmsApiContext db)
        {
            this.db = db;
        }

        public void Add(Course course)
        {
            db.Add(course);
        }

        public async Task<bool> AnyAsync(int? id)
        {
            ArgumentNullException.ThrowIfNull(id);
            return await db.Course.AnyAsync(c => c.Id == id);
        }

        public async Task<Course> FindAsync(int? id)
        {
            ArgumentNullException.ThrowIfNull(id);
            var course = await db.Course.Include(c => c.Modules).FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                throw new DirectoryNotFoundException();

            return course;
        }

        public async Task<IEnumerable<Course>> GetAllCourses(bool includeModules = false)
        {
            IQueryable<Course> query = db.Course;

            if (includeModules)
                query = query.Include(c => c.Modules);

            var result = await query.ToListAsync();
            return result;
        }

        public async Task<Course> GetCourse(int? id)
        {
            ArgumentNullException.ThrowIfNull(id);
            var course = await db.Course.Include(c => c.Modules).FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                throw new DirectoryNotFoundException();

            return course;
        }

        public void Remove(Course course)
        {
            db.Course.Remove(course);
        }

        public void Update(Course course)
        {
            db.Course.Update(course);
        }
    }
}
