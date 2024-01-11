using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Tracker;
using System;

namespace Fanex.Affiliate.Admin.Core.Repository.Tracker
{
    public class JobTrackerRepository : BaseRepository, IJobTrackerRepository
    {
        public (int Id, ResultInfo Result) InsertDataTracker(InsertDataTrackerDTO param)
        {
            const string SpKey = "Task_API_PushDataTracker_Import";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("PushDataType", param.PushDataType);
                objectDb.SetParameterValue("PushDataInfo", param.PushDataInfo);
                objectDb.SetParameterValue("PushTrackerId", DefaultValue.Int);
                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();
                var id = objectDb.GetParameterValue("PushTrackerId");
                return (id == DBNull.Value ? 0 : Convert.ToInt32(id), GetResultInfo(objectDb));
            }
        }

        public ResultInfo UpdateDataTracker(UpdateDataTrackerDTO param)
        {
            const string SpKey = "Task_API_PushDataTracker_Upd";

            using (var objectDb = GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("PushTrackerId", param.PushTrackerId);
                objectDb.SetParameterValue("StatusId", param.StatusId);
                AssignResultOutputParam(objectDb);

                objectDb.ExecuteNonQuery();
                return GetResultInfo(objectDb);
            }
        }
    }
}
