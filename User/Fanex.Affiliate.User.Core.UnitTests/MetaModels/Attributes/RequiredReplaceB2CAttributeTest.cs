using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.MetalModels.Attributes;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.MetaModels.Attributes
{
    public class RequiredReplaceB2CAttributeTest
    {
        [Fact]
        public void GetClientValidationRules_WhenCall_ReturnCorrect()
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(Language));

            var attr = new RequiredReplaceB2CAttribute();

            var result = attr.GetClientValidationRules(metadata, null);

            Assert.NotNull(result);
        }
    }
}
