using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Affiliate.YB.RepullToolWinform
{
    public partial class UpdateTrackerForm : Form
    {
        public UpdateTrackerForm()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            var dataSource = new Dictionary<string, string>
            {
                { "Bonus Information", "BonusInfo" },
                { "Customer", "Customer" },
                { "Customer Reg Time", "CustRegTime" },
                { "Deposit Information", "DepositInfo" },
                { "Server Information", "ServerInfo" },
                { "Tp Bet Record", "TpBetRecord" },
                { "Tp Game Type", "TpGameType" },
                { "Tp Transfer Information", "TpTransferInfo" },
                { "Withdrawal Information", "WithdrawalInfo" }
            };

            comboBoxDataSource.DataSource = dataSource;
            comboBoxDataSource.DisplayMember = "Key";
            comboBoxDataSource.ValueMember = "Value";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            
        }
    }
}
