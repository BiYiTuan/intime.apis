using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Common.Web.Mvc.Binders;
using Yintai.Architecture.Framework.ServiceLocation;

namespace Yintai.Hangzhou.WebSupport.Binder
{
    public class ModelWithInternalJsonArrayBinder:DefaultModelBinder
    {
        private string[] _inspectedParams;
        private bool _inspecting;
        
        public ModelWithInternalJsonArrayBinder(string inspectorParas):base()
        { 
            if (!string.IsNullOrEmpty(inspectorParas))
            {
                _inspectedParams = inspectorParas.Split(',');
                _inspecting = true;
            }
        }
        /*
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var response = base.CreateModel(controllerContext, bindingContext,modelType);
            
            if (_inspecting)
            {
                var log = ServiceLocator.Current.Resolve<ILog>();
                foreach (var prop in _inspectedParams)
                {
                    log.Debug(prop);
                    var property = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(p => p.Name.ToLower() == prop.ToLower()).First();
                    log.Debug(property);
                    if (property == null || property.GetValue(response) != null)
                        continue;
                   
                    string[] propValues = controllerContext.HttpContext.Request.Params.GetValues(string.Concat(prop, "[]"));
                    log.Debug(propValues);
                    if (propValues != null)
                    {
                        var targetValues = Array.ConvertAll<string, int>(propValues, s => int.Parse(s));
                        
                        property.SetValue(response, targetValues);
                        log.Debug(property.GetValue(response));
                    }
                }
            }
            return response;
        }
   */
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var response =  base.BindModel(controllerContext, bindingContext);
            if (_inspecting)
            {
                var log = ServiceLocator.Current.Resolve<ILog>();

                var modelType = response.GetType();
                foreach (var prop in _inspectedParams)
                {
                    
                    var property = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(p => p.Name.ToLower() == prop.ToLower()).First();
                    log.Debug(property);
                    if (property == null || property.GetValue(response)!=null)
                        continue;
                    string[] propValues = controllerContext.HttpContext.Request.Params.GetValues(string.Concat(prop,"[]"));

                    if (propValues != null)
                    {
                       var targetValues = Array.ConvertAll<string, int>(propValues,s => int.Parse(s));
                       property.SetValue(response, targetValues);
                       log.Debug(property.GetValue(response));
                    }
                }
            }
            return response;
        }
         
    }
    public class InternalJsonArrayAttribute : CustomModelBinderAttribute
    {
        private string _inspects;
        
        public InternalJsonArrayAttribute(string inspects) :base(){
            _inspects = inspects;    
        }

        public override IModelBinder GetBinder()
        {
            return new ModelWithInternalJsonArrayBinder(_inspects);
        }
    }
}
