using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Fanex.Affiliate.Admin.Core.UnitTests.TestHelper
{
    public static class ReaderHelper
    {
        public static void MapReaderData(Mock<IDataReader> mockReader, object readerDataMapping)
        {
            var func = CreateMappingFunc(readerDataMapping);
            mockReader.Setup(r => r[It.IsAny<string>()]).Returns(func);
        }

        public static Func<string, object> CreateMappingFunc(object mappingData)
        {
            var dictionary = ToDictionary(mappingData);
            return x =>
            {
                if (dictionary.ContainsKey(x))
                {
                    return dictionary[x];
                }

                return string.Empty;
            };
        }

        private static IDictionary<string, object> ToDictionary(object data)
        {
            if (data is IDictionary)
            {
                return data as IDictionary<string, object>;
            }

            var type = data.GetType();
            var properties = type.GetProperties();
            return properties.ToDictionary(x => x.Name, x => x.GetValue(data, null), StringComparer.OrdinalIgnoreCase);
        }
    }
}
