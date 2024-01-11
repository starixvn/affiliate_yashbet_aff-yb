using Fanex.Affiliate.User.Core.DTO;
using Fanex.Data;

namespace Fanex.Affiliate.User.Core.Repository
{
    public interface IObjectDBBuilder
    {
        IObjectDb Build(string spKey, string dbMode = DBMode.Main);
    }
}