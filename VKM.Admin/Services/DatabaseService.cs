using VKM.Admin.Providers;

namespace VKM.Admin.Services
{
    public abstract class DatabaseService
    {
        protected readonly IDatabaseProvider DatabaseProvider;
        
        protected DatabaseService(IDatabaseProvider databaseProvider)
        {
            DatabaseProvider = databaseProvider;
        }
    }
}