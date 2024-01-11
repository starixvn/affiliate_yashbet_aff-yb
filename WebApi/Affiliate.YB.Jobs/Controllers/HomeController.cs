using Affiliate.YB.Jobs.Apis;
using Affiliate.YB.Jobs.RecurringTasks;
using Affiliate.YB.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Affiliate.YB.Jobs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILicenseeApi _licenseeApi;
        private readonly IFetchBetRecordDataTask _fetchBetRecordService;
        private readonly IFetchCustomerDataTask _fetchCustomerService;

        public HomeController(ILicenseeApi lincenseeApi, IFetchBetRecordDataTask fetchBetRecordService, IFetchCustomerDataTask fetchCustomerDataTask)
        {
            _licenseeApi = lincenseeApi;
            _fetchBetRecordService = fetchBetRecordService;
            _fetchCustomerService = fetchCustomerDataTask;
        }

        [HttpGet]
        [HttpGet("Index")]
        public Task<string> Index()
        {
            return Task.FromResult("Site is running in api");
        }

        [HttpGet("CheckConnection")]
        public async Task<string> CheckConnection()
        {
            return await  _licenseeApi.CheckConnection();
        }

        [HttpGet("GetCustomers")]
        public async Task<IEnumerable<AffCustomerModel>> GetAffCustomerAsync(DateTime date)
        {
            return await _licenseeApi.GetAffCustomerAsync(date);
        }

        [HttpGet("GetBetRecord")]
        public async Task<IEnumerable<TransBetRecordInfo>> GetBetRecord(DateTime fromDate, DateTime toDate, int pageIndex, int pageSize)
        {
            return await _licenseeApi.GetBetRecordAsync(fromDate, toDate, pageIndex, pageSize);
        }

        [HttpGet("PullBetRecord")]
        public async Task<string> PullBetRecord(DateTime fromDate, DateTime toDate)
        {
            await _fetchBetRecordService.FetchDataAsync(fromDate, toDate);
            return "Finished Pull";
        }

        [HttpGet("PullFixedCustomer")]
        public async Task<string> PullFixedCustomer()
        {
            await _fetchCustomerService.FetchFixedDataTask();
            return "Finished Pull";
        }
    }
}