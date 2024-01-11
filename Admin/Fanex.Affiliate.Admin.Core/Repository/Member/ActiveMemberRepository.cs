using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO.Member;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data;

namespace Fanex.Affiliate.Admin.Core.Repository.Member
{
    public class ActiveMemberRepository : BaseRepository, IActiveMemberRepository
    {
        public AllMemberEntities GetAllMembers(string userNames)
        {
            const string SpKey = "Aff_Admin_ActiveMember_CheckStatus";

            var result = new AllMemberEntities();

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue(Field.UserName, userNames);
                AssignResultOutputParam(objectDb);

                using (var reader = objectDb.ExecuteReader())
                {
                    result.ActivedMemeberList = GetActivatedMemberList(reader);
                    result.InactiveMemberList = GetInactivatedMemberList(reader);
                }

                result.MemberNotExist = Convert.ToString(objectDb.GetParameterValue("ErrMsg"));
            }

            return result;
        }

        public IEnumerable<ActiveMemberInfo> GetActivatedMemberList(IDataReader reader)
        {
            var result = new List<ActiveMemberInfo>();

            while (reader.Read())
            {
                result.Add(new ActiveMemberInfo
                {
                    IbcCustId = reader.GetVal<int>("IBCCustID"),
                    IbcUserName = reader.ToString("IBCUserName"),
                    UserName = reader.ToString(Field.UserName),
                    FirstName = reader.ToString(Field.FirstName),
                    LastName = reader.ToString(Field.LastName),
                    CreatedDate = reader.GetVal<DateTime?>("CreatedDate")
                });
            }

            return result;
        }

        public IEnumerable<InactiveMemberInfo> GetInactivatedMemberList(IDataReader reader)
        {
            var result = new List<InactiveMemberInfo>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    result.Add(new InactiveMemberInfo
                    {
                        UserName = reader.ToString(Field.UserName),
                        FirstName = reader.ToString(Field.FirstName),
                        LastName = reader.ToString(Field.LastName)
                    });
                }
            }

            return result;
        }
    }
}
