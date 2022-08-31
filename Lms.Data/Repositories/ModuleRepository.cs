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
    internal class ModuleRepository : IModuleRepository
    {
        private readonly LmsApiContext db;

        public ModuleRepository(LmsApiContext db)
        {
            this.db = db;
        }

        public void Add(Module module)
        {
            db.Add(module);
        }

        public async Task<bool> AnyAsync(int? id)
        {
            ArgumentNullException.ThrowIfNull(id);
            return await db.Module.AnyAsync(c => c.Id == id);
        }

        public async Task<Module> FindAsync(int? id)
        {
            ArgumentNullException.ThrowIfNull(id);
            var module = await db.Module.FindAsync(id);

            if (module == null)
                throw new DirectoryNotFoundException();

            return module;
        }

        public async Task<IEnumerable<Module>> GetAllModules()
        {
            return await db.Module.ToListAsync();
        }

        public async Task<Module> GetModule(int? id)
        {
            ArgumentNullException.ThrowIfNull(id);
            var module = await db.Module.FindAsync(id);

            if (module == null)
                throw new DirectoryNotFoundException();

            return module;
        }

        public void Remove(Module module)
        {
            db.Module.Remove(module);
        }

        public void Update(Module module)
        {
            db.Module.Update(module);
        }
    }
}
