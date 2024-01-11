using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.MetalModels.Attributes;
using NSubstitute;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.MetaModels.Attributes
{
    public class RequiredByCheckedAttributeTest
    {
        [Fact]
        public void IsValid_WhenCall_ReturnCorrect()
        {
            var attr = new RequiredByCheckedFakeAttribute("HasWebSite", true);

            var objectValue = new UserSignUpParam { HasWebSite = true };
            var validationContext = new ValidationContext(objectValue);

            var result = attr.IsValid(objectValue, validationContext);

            Assert.Null(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("The field HasWebSite is invalid.")]
        public void IsValid_WhenCall_ReturnError(string errMessage)
        {
            var validationAttribute = Substitute.For<ValidationAttribute>();
            validationAttribute.FormatErrorMessage(Arg.Any<string>()).ReturnsForAnyArgs(errMessage);

            var attr = new RequiredByCheckedFakeAttribute("HasWebSite", true);

            var objectValue = new UserSignUpParam { HasWebSite = true };
            var validationContext = new ValidationContext(objectValue)
            {
                DisplayName = "HasWebSite"
            };

            var result = attr.IsValid(null, validationContext);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetClientValidationRules_WhenCall_ReturnCorrect()
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(Language));

            var attr = new RequiredByCheckedFakeAttribute();

            var result = attr.GetClientValidationRules(metadata, null);

            Assert.NotNull(result);
        }
    }

    public class RequiredByCheckedFakeAttribute : RequiredByCheckedAttribute
    {
        public RequiredByCheckedFakeAttribute() { }

        public RequiredByCheckedFakeAttribute(string dependentPropertyName, bool isChecked)
        {
            base.DependentPropertyName = dependentPropertyName;
            base.IsChecked = isChecked;
        }

        public new ValidationResult IsValid(object value, ValidationContext validationContext) => base.IsValid(value, validationContext);

        public new IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return base.GetClientValidationRules(metadata, context);
        }
    }
}
