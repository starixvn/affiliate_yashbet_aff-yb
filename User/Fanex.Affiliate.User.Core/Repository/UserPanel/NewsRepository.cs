using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Common.Extensions;
using System.Collections.Generic;
using System.Data;

namespace Fanex.Affiliate.User.Core.Repository.UserPanel
{
    public class NewsRepository : INewsRepository
    {
        private const string MessagePrefix = "message_";

        private readonly IObjectDBBuilder _builder;

        public NewsRepository(IObjectDBBuilder builder)
        {
            _builder = builder;
        }
        public LatestHotNews GetLastestHotNews(bool isInternal)
        {
            const string uSpKey = "Aff_HotNewsDetail";
            var dbMode = isInternal ? DBMode.View : DBMode.Main;

            var result = new LatestHotNews();

            using (var objectDb = _builder.Build(uSpKey, dbMode))
            {
                using (var reader = objectDb.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result.HotNewsList = GetMessageData(reader);

                        result.IsImportant = reader.ToBool("Important");
                    }
                }
            }

            return result;
        }

        public IEnumerable<MessageHistory> GetMessageList(MessageHistoryParam param)
        {
            const string uSpKey = "Aff_HotNewsSel";
            var mode = param.IsInternal ? DBMode.View : DBMode.Main;

            using (var objectDb = _builder.Build(uSpKey, mode))
            {
                return objectDb.Query<MessageHistory>(param);
            }
        }

        private static IDictionary<string, string> GetMessageData(IDataReader reader)
        {
            return new Dictionary<string, string>
            {
                ["en-US"] = reader[MessagePrefix + "en"]?.ToString(),
                ["zh-CN"] = reader[MessagePrefix + "cn"]?.ToString(),
                ["th-TH"] = reader[MessagePrefix + "th"]?.ToString(),
                ["vi-VN"] = reader[MessagePrefix + "vi"]?.ToString(),
                ["id-ID"] = reader[MessagePrefix + "id"]?.ToString(),
                ["hi-IN"] = reader[MessagePrefix + "hi"]?.ToString(),
                ["ms-MY"] = reader[MessagePrefix + "ms"]?.ToString(),
                ["bn-BD"] = reader[MessagePrefix + "bn"]?.ToString(),
                ["km-KH"] = reader[MessagePrefix + "kh"]?.ToString(),
            };
        }
    }
}