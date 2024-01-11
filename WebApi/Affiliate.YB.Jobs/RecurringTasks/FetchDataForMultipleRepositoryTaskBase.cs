using Affiliate.YB.Core;
using Affiliate.YB.Exceptions;
using Affiliate.YB.Jobs.Configurations;
using Affiliate.YB.Jobs.RecurringTasks.Interfaces;
using Affiliate.YB.Repository;
using Affiliate.YB.SpParams.Commands;
using Affiliate.YB.Utitlities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.YB.Jobs.RecurringTasks
{
    public abstract class FetchDataForMultipleRepositoryTaskBase<TData> : IFetchDataTask
        where TData : class
    {
        protected readonly IThirdPartyImportRepository _thirdPartyImportRepository;
        protected readonly FetchServiceSetting Settings;

        protected string TableName => Settings.DataSource;
        protected virtual int PagingSize => 2000;

        protected FetchDataForMultipleRepositoryTaskBase(
            IThirdPartyImportRepository thirdPartyImportRepository,
            FetchServiceSetting setting)
        {
            _thirdPartyImportRepository = thirdPartyImportRepository ?? throw new ArgumentNullException(nameof(thirdPartyImportRepository));
            Settings = setting ?? throw new ArgumentNullException(nameof(setting));
        }

        public abstract Task FetchDataAsync();

        protected async Task ImportDataToMultipleRepoAsync(IEnumerable<TData> data)
        {
            if (!data.Any())
            {
                return;
            }

            var supportedPrefix = GetSupportedRepoPrefix();
            var groupRecords = GetInsertedRecordBySupportPrefix(supportedPrefix, data);
            var importTasks = groupRecords.Select(g => ImportDataAsync(g.Value));
            await Task.WhenAll(importTasks);
        }

        protected abstract IEnumerable<string> GetSupportedRepoPrefix();

        protected virtual IDictionary<string, IEnumerable<InsertThirdPartyRecodInfoCommand>> GetInsertedRecordBySupportPrefix(
            IEnumerable<string> supportedPrefix,
            IEnumerable<TData> datas)
        {
            var dic = new Dictionary<string, IEnumerable<InsertThirdPartyRecodInfoCommand>>();
            foreach (var prefix in supportedPrefix)
            {
                var pagingData = PagingHelper.PagingData(datas, PagingSize);
                var records = pagingData.Select(data => CreateInsertRecord(prefix, data));
                dic.Add(prefix, records);
            }

            return dic;
        }

        protected InsertThirdPartyRecodInfoCommand CreateInsertRecord(
            string prefix,
            IEnumerable<TData> data)
        {
            var xml = BuildXml(data);
            return new InsertThirdPartyRecodInfoCommand(new PrefixProvider(prefix))
            {
                DataSource = Settings.DataSource,
                XmlData = xml,
                ErrCode = 0,
                ErrMsg = ""
            };
        }

        protected abstract string BuildXml(IEnumerable<TData> data);

        private async Task ImportDataAsync(IEnumerable<InsertThirdPartyRecodInfoCommand> records)
        {
            foreach (var record in records)
            {
                await _thirdPartyImportRepository.ImportRecordAsync(record);
                if (record.ErrCode != 0)
                    throw new ImporterException(record.ErrMsg);
            }
        }
    }
}
