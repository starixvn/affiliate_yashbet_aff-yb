using Affiliate.YB.SpParams.Commands;
using System.Threading.Tasks;

namespace Affiliate.YB.Repository
{
    public interface IThirdPartyImportRepository
    {
        Task ImportRecordAsync(InsertThirdPartyRecodInfoCommand betRecord);
    }
}
 