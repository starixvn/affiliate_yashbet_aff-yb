using Affiliate.YB.DTO;
using Affiliate.YB.SpParams.Criteria;
using Affiliate.YB.WebApi.Extensions;
using Affiliate.YB.WebApi.Infastructure.Enums;
using Affiliate.YB.WebApi.Models.HelthCheck;
using Fanex.Data.Repository;
using System;
using System.Threading.Tasks;

namespace Affiliate.YB.WebApi.Health
{
    public class DatabaseHealthCheckProvider : IHealthCheckProvider
    {
        private DatabaseServerType DatabaseServerType { get; }
        private IDynamicRepository Repo { get; }

        public int SortOrder { get; }


        public DatabaseHealthCheckProvider(IDynamicRepository repo, DatabaseServerType databaseServerType, int sortOrder = 20)
        {
            Repo = repo;
            DatabaseServerType = databaseServerType;
            SortOrder = sortOrder;
        }

        public async Task<HealthCheckItemResult> GetHealthCheckAsync()
        {
            var dataBaseServerName = DatabaseServerType.ToString();
            var criteria = new GetAffCustomersCriteria(DateTime.Now, dataBaseServerName);
            var result = new HealthCheckItemResult($"{nameof(DatabaseHealthCheckProvider)}"
                , SortOrder
                , "Checks the database"
                , $"Checks whether the {dataBaseServerName} database can be accessed.");
            try
            {
                await Repo.FetchAsync<AffCustomerInfo>(criteria);
                result.HealthState = HealthState.Healthy;
                result.Messages.Add($"Connect database {dataBaseServerName} success");
            }
            catch 
            {
                result.HealthState = HealthState.Unhealthy;
                result.Messages.Add($"Connect database {dataBaseServerName} fail");
            }
            return result;
        }
    }
}