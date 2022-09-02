using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Data.Data;
using Lms.Core.Entities;
using Lms.Core.Repositories;
using AutoMapper;
using Lms.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace Lms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IUoW uow;
        private readonly IMapper mapper;

        public CoursesController(IUoW uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourse(bool includeModules = false)
        {
            var courses = await uow.CourseRepository.GetAllCourses(includeModules);
            var dto = mapper.Map<IEnumerable<CourseDto>>(courses);

            return Ok(dto);
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            if (!await uow.CourseRepository.AnyAsync(id))
                return NotFound();

            var course = await uow.CourseRepository.GetCourse(id);
            var dto = mapper.Map<CourseDto>(course);

            return Ok(dto);
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            try
            {
                uow.CourseRepository.Update(course);
                await uow.CompleteAsync();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            try
            {
                uow.CourseRepository.Add(course);
                await uow.CompleteAsync();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            if (!await uow.CourseRepository.AnyAsync(id))
                return NotFound();

            try
            {
                uow.CourseRepository.Remove(await uow.CourseRepository.FindAsync(id));
                await uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return NoContent();
        }

        [HttpPatch("{courseId}")]
        public async Task<ActionResult<CourseDto>> PatchCourse(int courseId, JsonPatchDocument<CourseDto> patchDocument)
        {
            var course = await uow.CourseRepository.GetCourse(courseId);
            var dto = mapper.Map<CourseDto>(course);
            patchDocument.ApplyTo(dto, ModelState);
            mapper.Map(dto, course);
            await uow.CompleteAsync();

            return Ok(dto);
        }
    }
}
