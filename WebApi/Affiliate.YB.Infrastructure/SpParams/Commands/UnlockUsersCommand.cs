using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Affiliate.YB.SpParams.Commands
{
    public class UnlockUsersCommand : NonQueryCommand
    {
        public string CustIdList { get; set; }

        public override string GetSettingKey() => "API_Customer_Unlock";

        public override bool IsValid() => true;
    }
}
