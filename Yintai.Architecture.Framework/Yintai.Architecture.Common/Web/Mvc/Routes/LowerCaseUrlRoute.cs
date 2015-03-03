using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Yintai.Architecture.Common.Web.Mvc.Routes
{
    /// <summary>
    /// CLR Version: 4.0.30319.269
    /// NameSpace: Yintai.Architecture.PMS.WebSiteCore.Mvc
    /// FileName: Class1
    ///
    /// Created at 10/18/2012 8:39:31 PM
    /// Description: 
    /// </summary>
    /// <summary>
    /// CLR Version: 4.0.30319.239
    /// NameSpace: Yintai.Preferential.Common.Web.Mvc
    /// FileName: LowerCaseUrlRoute
    ///
    /// Created at 1/18/2012 5:31:29 PM
    /// </summary>
    public class LowerCaseUrlRoute : Route
    {
        #region fields

        private static readonly string[] _requiredKeys = new[] { "area", "controller", "action" };

        #endregion

        #region .ctor

        public LowerCaseUrlRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler) { }

        public LowerCaseUrlRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler) { }

        public LowerCaseUrlRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler) { }

        public LowerCaseUrlRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler) { }

        #endregion

        #region properties

        #endregion

        #region methods

        private static void LowerRouteValues(RouteValueDictionary values)
        {
            foreach (var key in _requiredKeys)
            {
                if (values.ContainsKey(key) == false) continue;

                var value = values[key];
                if (value == null) continue;

                var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);
                if (valueString == null) continue;

                values[key] = valueString.ToLower();
            }

            var otherKyes = values.Keys
                .Except(_requiredKeys, StringComparer.InvariantCultureIgnoreCase)
                .ToArray();

            foreach (var key in otherKyes)
            {
                var value = values[key];
                values.Remove(key);
                values.Add(key.ToLower(), value);
            }
        }

        #endregion

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            LowerRouteValues(requestContext.RouteData.Values);
            LowerRouteValues(values);
            LowerRouteValues(Defaults);

            return base.GetVirtualPath(requestContext, values);
        }
    }

    public static class LowerCaseUrlRouteMapHelper
    {
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this RouteCollection routes, string name, string url)
        {
            return routes.MapLowerCaseUrlRoute(name, url, null, null);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this RouteCollection routes, string name, string url, object defaults)
        {
            return routes.MapLowerCaseUrlRoute(name, url, defaults, null);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this RouteCollection routes, string name, string url, string[] namespaces)
        {
            return routes.MapLowerCaseUrlRoute(name, url, null, null, namespaces);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this RouteCollection routes, string name, string url, object defaults, object constraints)
        {
            return routes.MapLowerCaseUrlRoute(name, url, defaults, constraints, null);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this RouteCollection routes, string name, string url, object defaults, string[] namespaces)
        {
            return routes.MapLowerCaseUrlRoute(name, url, defaults, null, namespaces);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this RouteCollection routes, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if (routes == null) throw new ArgumentNullException("routes");
            if (url == null) throw new ArgumentNullException("url");
            var route2 = new LowerCaseUrlRoute(url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary()
            };
            var item = route2;
            if ((namespaces != null) && (namespaces.Length > 0))
                item.DataTokens["Namespaces"] = namespaces;
            routes.Add(name, item);
            return item;
        }

        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this AreaRegistrationContext context, string name, string url)
        {
            return context.MapLowerCaseUrlRoute(name, url, null);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this AreaRegistrationContext context, string name, string url, object defaults)
        {
            return context.MapLowerCaseUrlRoute(name, url, defaults, null);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this AreaRegistrationContext context, string name, string url, string[] namespaces)
        {
            return context.MapLowerCaseUrlRoute(name, url, null, namespaces);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this AreaRegistrationContext context, string name, string url, object defaults, object constraints)
        {
            return context.MapLowerCaseUrlRoute(name, url, defaults, constraints, null);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this AreaRegistrationContext context, string name, string url, object defaults, string[] namespaces)
        {
            return context.MapLowerCaseUrlRoute(name, url, defaults, null, namespaces);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this AreaRegistrationContext context, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if ((namespaces == null) && (context.Namespaces != null))
                namespaces = context.Namespaces.ToArray<string>();
            LowerCaseUrlRoute route = context.Routes.MapLowerCaseUrlRoute(name, url, defaults, constraints, namespaces);
            route.DataTokens["area"] = context.AreaName;
            bool flag = (namespaces == null) || (namespaces.Length == 0);
            route.DataTokens["UseNamespaceFallback"] = flag;
            return route;
        }
    }
}
