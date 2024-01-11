using Fanex.Affiliate.User.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Fanex.Affiliate.User.Core.MetalModels.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class RequiredReplaceB2CAttribute : RequiredAttribute, IClientValidatable
    {
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "required",
                ErrorMessage = SiteFriendlyNameHelper.ReplaceB2C(FormatErrorMessage(metadata.GetDisplayName()))
            };
            yield return rule;
        }
    }
}
