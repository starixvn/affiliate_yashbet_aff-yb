using Affiliate.YB.DTO;
using Affiliate.YB.Exceptions;
using Fanex.Data.Repository;
using System;
using System.Threading.Tasks;

namespace Affiliate.YB.Repository
{
    public class MemberImportRepository : IMemberImportRepository
    {
        private readonly IDynamicRepository _dynamicRepository;

        public MemberImportRepository(IDynamicRepository dynamicRepository)
        {
            _dynamicRepository = dynamicRepository;
        }

        public async Task<DateTime> GetLastedMemberImportedTimeAsync(MemberImportedTimeInfo settingInfo)
            => await _dynamicRepository.GetAsync<DateTime>(settingInfo);

        public async Task<string> GetSiteNameAsync(SiteNameInfo settingInfo)
            => await _dynamicRepository.GetAsync<string>(settingInfo);

        public async Task ImportMemberAsync(InsertMemberInfoCommand memberInfo)
        {
            await _dynamicRepository.ExecuteAsync(memberInfo);
            if (memberInfo.ErrCode != 0)
            {
                throw new ImporterException(memberInfo.ErrCode.ToString(), memberInfo.ErrMsg, memberInfo.XmlData);
            }
        }
    }
}