using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Report;
using Fanex.Affiliate.Admin.Core.MetaModels.Report;
using Fanex.Affiliate.Admin.Core.Repository.Report;
using Fanex.Affiliate.Admin.Core.Utilities;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services.Report
{
    public class PlayerGgrReportService : IPlayerGgrReportService
    {
        private readonly IPlayerGgrReportRepository _playerGgrReportRepository;

        public PlayerGgrReportService(IPlayerGgrReportRepository playerGgrReportRepository)
        {
            _playerGgrReportRepository = playerGgrReportRepository;
        }

        public EntityResult<PlayerGgrReport> GetPlayerGgrReportData(PlayerGgrReportParam param)
        {
            if (DateTimeHelper.IsValidDateRange(param.FromDate, param.ToDate))
            {
                return _playerGgrReportRepository.GetPlayerGgrReportData(param);
            }

            return new EntityResult<PlayerGgrReport> { MainData = new List<PlayerGgrReport>() };
        }
    }
}
