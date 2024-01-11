using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Report;
using Fanex.Affiliate.Admin.Core.MetaModels.Report;
using Fanex.Affiliate.Admin.Core.Repository.Report;
using Fanex.Affiliate.Admin.Core.Utilities;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Services.Report
{
    public class TransferReportService : ITransferReportService
    {
        private readonly ITransferReportRepository _transferReportRepository;

        public TransferReportService(ITransferReportRepository transferReportRepository)
        {
            _transferReportRepository = transferReportRepository;
        }

        public EntityResult<TransferReport> GetTransferReportData(TransferReportParam param)
        {
            if (DateTimeHelper.IsValidDateTime(param.FromDate, param.ToDate))
            {
                return _transferReportRepository.GetTransferReportData(param);
            }

            return new EntityResult<TransferReport> { MainData = new List<TransferReport>() };
        }
    }
}
