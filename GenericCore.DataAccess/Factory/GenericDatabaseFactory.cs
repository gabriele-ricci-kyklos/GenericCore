using GenericCore.DataAccess.DAOHelper;

namespace GenericCore.DataAccess.Factory
{
    public class GenericDatabaseFactory : BaseGenericDatabaseFactory
    {
        protected override IDAOHelper GetDAOHelperByProviderNameCore(string providerName)
        {
            return null;
        }
    }
}
