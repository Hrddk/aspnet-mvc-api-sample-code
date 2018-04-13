using System;
using DataLibrary.DbContext;

namespace DataLibrary.Repository
{
    public sealed class DepartmentRepository
    {
        private static readonly Lazy<DatabaseContext> instance = new Lazy<DatabaseContext>(() => new DatabaseContext());

        private DepartmentRepository() { }

        public static DatabaseContext Instance
        {
            get { return instance.Value; }
        }
    }
}
