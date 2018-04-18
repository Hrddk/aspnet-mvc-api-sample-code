using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DatabaseLibrary.DbContext;

namespace DatabaseLibrary.Repository
{
    public sealed class DepartmentRepository
    {
        private static readonly Lazy<DepartmentRepository> instance = new Lazy<DepartmentRepository>(() => new DepartmentRepository());
        private static DatabaseContext dbContext;

        private DepartmentRepository()
        {
            dbContext = DatabaseRepository.Instance;
        }

        public static DepartmentRepository Instance
        {
            get { return instance.Value; }
        }

        public IQueryable<Department> Departments()
        {
            return dbContext.Departments.Include(x => x.Users);
        }

        public Task<Department> FindAsync(int id)
        {
            return dbContext.Departments.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Department> Add(Department department)
        {
            var newDepartment = dbContext.Departments.Add(department);
            dbContext.SaveChangesAsync();
            return Task.FromResult(newDepartment);
        }

        public Task<Department> Update(Department department)
        {
            var oldUser = this.FindAsync(department.Id).Result;

            oldUser.Name = department.Name;
            oldUser.IsDeleted = department.IsDeleted;

            dbContext.SaveChangesAsync();
            return Task.FromResult(oldUser);
        }

        public Task<Department> Delete(int id)
        {

            Department department = this.FindAsync(id).Result;
            if (department == null)
            {
                return null;
            }
            var removedDepartment = dbContext.Departments.Remove(department);
            dbContext.SaveChangesAsync();
            return Task.FromResult(removedDepartment);
        }

        public Task<int> SaveChangesAsync()
        {
            return dbContext.SaveChangesAsync();
        }
    }
}
