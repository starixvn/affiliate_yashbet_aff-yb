using Fanex.Affiliate.User.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Fanex.Affiliate.User.Core.MetalModels.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class RequiredByCheckedAttribute : ValidationAttribute, IClientValidatable
    {
        private const string DefaultErrorMessage = "{0} field is required";

        public string DependentPropertyName { get; set; }

        public bool IsChecked { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //Get details about the dependent property
            var dependentProperty = validationContext.ObjectInstance.GetType().GetProperty(DependentPropertyName);
            var dependentPropertyValue = Convert.ToString(dependentProperty.GetValue(validationContext.ObjectInstance, null));

            if (bool.TryParse(dependentPropertyValue, out bool isChecked) &&
                isChecked == IsChecked && string.IsNullOrEmpty(Convert.ToString(value)))
            {
                //Validation should fail if the checkbox is selected but the main field is empty
                var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(string.IsNullOrEmpty(errorMessage) ?
                        string.Format(DefaultErrorMessage, validationContext.DisplayName) :
                        errorMessage);
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "requiredbychecked",
                ErrorMessage = SiteFriendlyNameHelper.ReplaceB2C(FormatErrorMessage(metadata.GetDisplayName()))
            };
            rule.ValidationParameters.Add("dependentprop", DependentPropertyName);
            rule.ValidationParameters.Add("checked", IsChecked);
            yield return rule;
        }
    }
}