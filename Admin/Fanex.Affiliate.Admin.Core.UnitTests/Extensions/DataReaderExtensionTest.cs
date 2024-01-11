using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.UnitTests.TestHelper;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Extensions
{
    public class DataReaderExtensionTest
    {
        [Fact]
        public void GetVal_WhenCall_ReturnDefaultValue()
        {
            // Arrange
            var dataReader = new Mock<IDataReader>();
            ReaderHelper.MapReaderData(dataReader, new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["Abc"] = DBNull.Value
            });

            // Action
            var value = DataReaderExtension.GetVal<int>(dataReader.Object, "Abc");

            // Assert
            Assert.Equal(0, value);
        }
    }
}
