using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Member;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels.Member;
using Fanex.Common.Extensions;
using System;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Repository.Member
{
    public class MemberRepository : BaseRepository, IMemberRepository
    {
        public EntityResult<MemberInfo> GetMemberList(MemberParam param)
        {
            const string Spkey = "Aff_Admin_MemberList_Sel";
            var result = new EntityResult<MemberInfo>();
            var dataList = new List<MemberInfo>();

            using (var objectDb = GetObjectDb(Spkey))
            {
                objectDb.SetParameterValue("AffUsername", param.AffName);
                objectDb.SetParameterValue("UserName", param.UserName);
                objectDb.SetParameterValue("IBCUserName", param.IbcUserName);
                objectDb.SetParameterValue("PageIndex", param.PageIndex);
                objectDb.SetParameterValue("PageSize", param.PageSize);

                objectDb.SetParameterValue("NRows", DefaultValue.Int);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(new MemberInfo
                        {
                            MemberId = reader.ToInt("CustID"),
                            UserName = reader.ToString("UserName"),
                            IbcUserName = reader.ToString("IBCUserName"),
                            FirstName = reader.ToString("FirstName"),
                            LastName = reader.ToString("LastName"),
                            CreatedDate = reader.GetVal<DateTime?>("RegisteredDate"),
                            LastActionDate = reader.GetVal<DateTime?>("LastAction"),
                            IsInclude = reader.GetVal<bool>("IsInclude"),
                            IsRemoved = reader.GetVal<bool>("IsRemoved"),
                            AffName = reader.ToString("AffUserName")
                        });
                    }
                }

                result.MainData = dataList;
                result.TotalItem = GetTotalRows(objectDb);
            }

            return result;
        }

        public PlayerActionInfo GetMemberDetail(int memberId)
        {
            const string Spkey = "Aff_Admin_Player_Sel";

            using (var objectDb = GetObjectDb(Spkey))
            {
                objectDb.SetParameterValue("CustId", memberId);

                using (var reader = objectDb.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new PlayerActionInfo
                        {
                            UserName = reader.ToString("UserName"),
                            IsInclude = reader.GetVal<bool>("IsInclude"),
                            IsRemoved = reader.GetVal<bool>("IsRemoved"),
                            LastActionDate = reader.GetVal<DateTime?>("LastAction"),
                        };
                    }
                }
            }

            return null;
        }

        public ResultInfo UpdateMember(UpdateMemberParam param)
        {
            const string Spkey = "Aff_Admin_Member_RemoveStatus_Upd";

            using (var objectDb = GetObjectDb(Spkey))
            {
                objectDb.SetParameterValue("CustId", param.MemberId);
                objectDb.SetParameterValue("IsRemoved", param.IsRemoved);
                objectDb.SetParameterValue("IsInclude", param.IsInclude);
                objectDb.SetParameterValue("UserId", param.UserId);
                objectDb.SetParameterValue("Ip", param.Ip);
                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();

                return GetResultInfo(objectDb);
            }
        }
    }
}
