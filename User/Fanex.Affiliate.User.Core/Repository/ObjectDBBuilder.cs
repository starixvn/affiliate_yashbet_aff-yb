using Fanex.Affiliate.User.Core.DTO;
using Fanex.Data;
using Fanex.Data.SqlServer;

namespace Fanex.Affiliate.User.Core.Repository
{
    public class ObjectDBBuilder : IObjectDBBuilder
    {
        public IObjectDb Build(string spKey, string dbMode = DBMode.Main)
            => new SqlObjectDb(spKey + dbMode);
    }
}