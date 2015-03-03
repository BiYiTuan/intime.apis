using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Yintai.Architecture.Framework.Mapping;

namespace Yintai.Architecture.Framework.Mapping
{
    public class PropertyMappingResolver : IMappingResolver
    {
        #region IMappingResolver Members

        public ResolutionResult Resolve(ResolutionContext context)
        {
            var propertyResult = new ResolutionResult(context);

            var bindings = new List<MemberBinding>();

            var targetProperties = from property in context.TargetType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                   where property.CanWrite
                                   select property;

            foreach (PropertyInfo targetProperty in targetProperties)
            {
                MappingAttribute attribute = targetProperty.GetAttribute();
                if (attribute != null && attribute.Ignored)
                    continue;

                PropertyInfo sourceProperty = context.SourceType.GetProperty(targetProperty.Name);

                if (sourceProperty == null)
                {
                    if (attribute != null)
                        sourceProperty = context.SourceType.GetProperty(attribute.Name);

                    if (sourceProperty == null)
                        continue;
                }

                if (!sourceProperty.CanRead)
                    continue;

                MemberBinding memberBinding;

                if (!targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                {
                    memberBinding = Expression.Bind(
                        targetProperty,
                        Expression.Convert(
                            Expression.Property(context.Parameter, sourceProperty),
                            targetProperty.PropertyType)
                        );
                }
                else
                {
                    memberBinding = Expression.Bind(targetProperty, Expression.Property(context.Parameter, sourceProperty));
                }

                bindings.Add(memberBinding);
            }

            propertyResult.MemberBindings = bindings;
            return propertyResult;
        }

        #endregion
    }
}
