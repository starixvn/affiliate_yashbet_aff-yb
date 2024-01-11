using Fanex.Affiliate.Admin.Core.DTO.Common;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.Services.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class UnauthorizedPropertyCensorAttribute : ActionFilterAttribute
    {
        public string PermissionName { get; set; }
        private IEnumerable<UserPermission> PermissionList { get; set; }

        public UnauthorizedPropertyCensorAttribute(string permissionName = null)
        {
            PermissionName = permissionName;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var sessionService = DependencyResolver.Current.GetService<ISessionService>();
            PermissionList = sessionService.UserPermissionList;

            var data = GetResultData(filterContext.Result);

            if (data != null && PermissionList.Any())
            {
                CensorData(data);
            }
            base.OnResultExecuting(filterContext);
        }

        private void CensorData(object data)
        {
            var type = data?.GetType();
            if (data == null || IsPrimitiveType(type))
            {
                return;
            }

            if (data is IEnumerable)
            {
                foreach (var item in data as IEnumerable)
                {
                    CensorData(item);
                }
            }
            else
            {
                var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (var pro in properties)
                {
                    if (IsNeedResetPro(pro))
                    {
                        pro.SetValue(data, null, null);
                    }

                    var value = pro.GetValue(data, null);
                    CensorData(value);
                }
            }
        }

        private bool IsNeedResetPro(PropertyInfo pro)
        {
            var attribute = pro.GetCustomAttribute<PermissionCheckAttribute>();
            return attribute != null && !HasPermission(attribute.PermissionName);
        }

        private bool HasPermission(string permissionName)
        {
            return (string.IsNullOrWhiteSpace(PermissionName) && permissionName == PermissionName)
                || PermissionList.Any(p => p.FunctionName == permissionName);
        }

        private static bool IsPrimitiveType(Type type)
        {
            return type.IsPrimitive || type.IsValueType || type == typeof(string);
        }

        private static object GetResultData(ActionResult result)
        {
            switch (result)
            {
                case JsonResult jsonResult:
                    return jsonResult.Data;
                case JsonNetResult jsonNetResult:
                    return jsonNetResult.Data;
                case ViewResultBase viewResult:
                    return viewResult.Model;
                default:
                    throw new InvalidOperationException(result.GetType().Name);
            }
        }
    }
}
