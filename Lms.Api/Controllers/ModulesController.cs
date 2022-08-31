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

namespace Lms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly IUoW uow;
        private readonly IMapper mapper;

        public ModulesController(IUoW uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        // GET: api/Modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Module>>> GetModule()
        {
            var modules = await uow.ModuleRepository.GetAllModules();
            var dto = mapper.Map<IEnumerable<ModuleDto>>(modules);

            return Ok(dto);
        }

        // GET: api/Modules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Module>> GetModule(int id)
        {
            if (!await uow.ModuleRepository.AnyAsync(id))
                return NotFound();

            var module = await uow.ModuleRepository.GetModule(id);
            var dto = mapper.Map<ModuleDto>(module);

            return Ok(dto);
        }

        // PUT: api/Modules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModule(int id, Module @module)
        {
            try
            {
                uow.ModuleRepository.Update(@module);
                await uow.CompleteAsync();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return NoContent();
        }

        // POST: api/Modules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Module>> PostModule(Module @module)
        {
            try
            {
                uow.ModuleRepository.Add(@module);
                await uow.CompleteAsync();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return CreatedAtAction("GetModule", new { id = @module.Id }, @module);
        }

        // DELETE: api/Modules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            if (!await uow.ModuleRepository.AnyAsync(id))
                return NotFound();

            try
            {
                uow.ModuleRepository.Remove(await uow.ModuleRepository.FindAsync(id));
                await uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return NoContent();
        }
    }
}
