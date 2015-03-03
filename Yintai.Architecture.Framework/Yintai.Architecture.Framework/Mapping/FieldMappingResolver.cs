using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Yintai.Architecture.Framework.Mapping;

namespace Yintai.Architecture.Framework.Mapping
{
    public class FieldMappingResolver : IMappingResolver
    {
        #region IMappingResolver Members

        public ResolutionResult Resolve(ResolutionContext context)
        {
            ResolutionResult fieldResult = new ResolutionResult(context);

            List<MemberBinding> bindings = new List<MemberBinding>();

            var targetFields = from field in context.TargetType.GetFields(BindingFlags.Public | BindingFlags.Instance)
                               select field;

            foreach (FieldInfo targetField in targetFields)
            {
                MappingAttribute attribute = targetField.GetAttribute();
                if (attribute != null && attribute.Ignored)
                    continue;

                FieldInfo sourceField = context.SourceType.GetField(targetField.Name);

                if (sourceField == null)
                {
                    if (attribute != null)
                        sourceField = context.SourceType.GetField(attribute.Name);

                    if (sourceField == null)
                        continue;
                }

                MemberBinding memberBinding;

                if (!targetField.FieldType.IsAssignableFrom(sourceField.FieldType))
                {
                    memberBinding = Expression.Bind(
                        targetField,
                        Expression.Convert(
                            Expression.Field(context.Parameter, sourceField),
                            targetField.FieldType)
                        );
                }
                else
                {
                    memberBinding = Expression.Bind(targetField, Expression.Field(context.Parameter, sourceField));
                }

                bindings.Add(memberBinding);
            }
            fieldResult.MemberBindings = bindings;
            return fieldResult;
        }

        #endregion
    }
}
