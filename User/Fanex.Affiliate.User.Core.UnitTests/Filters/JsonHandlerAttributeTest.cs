using Fanex.Affiliate.User.Core.Controllers;
using Fanex.Affiliate.User.Core.Filters;
using Fanex.Affiliate.User.Core.Infrastructure;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using System.Collections.Generic;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Filters
{
    public class JsonHandlerAttributeTest
    {
        [Fact]
        public void IsValidForRequest_WhenCall_ReturnNull()
        {
            var actionContext = GetActionExecutedContext();

            var attr = new JsonHandlerAttribute();

            attr.OnActionExecuted(actionContext);

            Assert.Null(actionContext.Result as JsonResult);
        }

        [Fact]
        public void IsValidForRequest_WhenCall_ReturnCorrectJsonNetResult()
        {
            var actionContext = GetActionExecutedContext();
            actionContext.Result = new JsonResult();

            var attr = new JsonHandlerAttribute();

            attr.OnActionExecuted(actionContext);

            Assert.NotNull(actionContext.Result as JsonNetResult);
        }

        private static ActionExecutedContext GetActionExecutedContext()
        {
            var controller = NSubtituteMockingHelper.FakeController(new HomeController(null, null, null));
            var actionDes = new ActionDescriptorFake();
            return new ActionExecutedContext(controller.ControllerContext, actionDes, false, null);
        }
    }

    public class ActionDescriptorFake : ActionDescriptor
    {
        public override string ActionName => throw new System.NotImplementedException();

        public override ControllerDescriptor ControllerDescriptor => throw new System.NotImplementedException();

        public override object Execute(ControllerContext controllerContext, IDictionary<string, object> parameters)
        {
            throw new System.NotImplementedException();
        }

        public override ParameterDescriptor[] GetParameters()
        {
            throw new System.NotImplementedException();
        }
    }
}
