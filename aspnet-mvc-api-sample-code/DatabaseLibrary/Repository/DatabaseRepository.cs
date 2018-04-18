using System;
using DatabaseLibrary.DbContext;

namespace DatabaseLibrary.Repository
{
    public sealed class DatabaseRepository
    {
        private static readonly Lazy<DatabaseContext> instance = new Lazy<DatabaseContext>(() => new DatabaseContext());

        private DatabaseRepository() { }

        public static DatabaseContext Instance
        {
            get { return instance.Value; }
        }
    }
}
