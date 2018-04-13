namespace DataLibrary.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataLibrary.DbContext;

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

        public IEnumerable<User> Users()
        {
            return dbContext.Users
                .AsEnumerable()
                .Select(x => new User()
                {
                    BirthDate = x.BirthDate,
                    ContactNo = x.ContactNo,
                    ContactPreference = x.ContactPreference,
                    Email = x.Email,
                    Fistname = x.Fistname,
                    Gender = x.Gender,
                    Id = x.Id,
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted,
                    Lastname = x.Lastname,
                    Photo = x.Photo,
                    UserDepartment = new Department()
                    {
                        Id = x.UserDepartment.Id,
                        Name = x.UserDepartment.Name,
                        IsDeleted = x.UserDepartment.IsDeleted
                    },
                });
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
            dbContext.Entry(user).State = EntityState.Modified;
            dbContext.SaveChangesAsync();
            return Task.FromResult(user);
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
