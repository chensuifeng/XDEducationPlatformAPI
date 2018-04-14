using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using System.Configuration;
using System.Web.Http.Cors;
using XDEducationPlatformAPI.UrlDecrypt;
using Microsoft.Owin.Security.OAuth;
namespace XDEducationPlatformAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // Web API 配置和服务
            #region 将返回数据格式变为json；键值同一为小写(原格式是xml)
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            var jsonFormatter = config.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            jsonFormatter.SerializerSettings.ContractResolver = new UnderlineSplitContractResolver();
            #endregion

            #region 跨域
            var allowedOrigin = ConfigurationManager.AppSettings["cros:allowedOrigin"];
            var allowedHeaders = ConfigurationManager.AppSettings["cros:allowedHeaders"];
            var allowedMethods = ConfigurationManager.AppSettings["cros:allowedMethods"];
            var configCros = new EnableCorsAttribute(allowedOrigin, allowedHeaders, allowedMethods) { SupportsCredentials = true };
            config.EnableCors(configCros);

            //不做任何限制
            //config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            #endregion

            // Web API 路由
            config.MapHttpAttributeRoutes();

            #region 原始路由
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            #endregion

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = RouteParameter.Optional }
            );



            // Web API 配置和服务
            // 将 Web API 配置为仅使用不记名令牌身份验证。            调用此方法后 500Internal Server Error 原因未知
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            // 添加自定义消息处理
            config.MessageHandlers.Add(new JoyMessageHandler());

        }

        public class UnderlineSplitContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                //return CamelCaseToUnderlineSplit(propertyName);
                return propertyName.ToLower();
            }
        }

    }
}
