namespace DatabaseLibrary.Repository
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DatabaseLibrary.DbContext;

    public sealed class UserRepository
    {
        private static readonly Lazy<UserRepository> instance = new Lazy<UserRepository>(() => new UserRepository());
        private static DatabaseContext dbContext;

        private UserRepository()
        {
            dbContext = DatabaseRepository.Instance;
        }

        public static UserRepository Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public IQueryable<User> Users()
        {
            return dbContext.Users.Include("Department");
        }

        public Task<User> FindAsync(int id)
        {
            return dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<User> Add(User user)
        {
            dbContext.Users.Add(user);
            dbContext.SaveChangesAsync();
            return Task.FromResult(user);
        }

        public Task<User> Update(User user)
        {
            var oldUser = this.FindAsync(user.Id).Result;

            oldUser.Fistname = user.Fistname;
            oldUser.Lastname = user.Lastname;
            oldUser.Photo = user.Photo;
            oldUser.BirthDate = user.BirthDate;
            oldUser.ContactNo = user.ContactNo;
            oldUser.ContactPreference = user.ContactPreference;
            oldUser.DepartmentId = user.DepartmentId;
            oldUser.Email = user.Email;
            oldUser.Gender = user.Gender;
            oldUser.IsActive = user.IsActive;
            oldUser.IsDeleted = user.IsDeleted;
            dbContext.SaveChangesAsync();
            return Task.FromResult(oldUser);
        }

        public Task<User> Delete(int id)
        {

            User user = this.FindAsync(id).Result;
            if (user == null)
            {
                return null;
            }
            dbContext.Users.Remove(user);
            dbContext.SaveChangesAsync();
            return Task.FromResult(user);
        }

        public Task<int> SaveChangesAsync()
        {
            return dbContext.SaveChangesAsync();
        }
    }
}
