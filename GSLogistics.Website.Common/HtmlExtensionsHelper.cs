using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GSLogistics.Website.Common
{
    public static class HtmlExtensionsHelper
    {
        public static ModelMetadata GetModelMetadata<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            return metadata;
        }

        /// <summary>
        /// Gets the attribute based on the specified attribute type and the given <c>ModelMetadata</c> object.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute type you need.</typeparam>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public static TAttribute GetAttributeFromMetadata<TAttribute>(ModelMetadata metadata)
            where TAttribute : class
        {
            var property = metadata.ContainerType.GetProperties().Where(p => p.Name == metadata.PropertyName).FirstOrDefault();

            if (property != null)
            {
                return property.GetCustomAttributes(typeof(TAttribute), true) as TAttribute;
            }

            return null;
        }

    }
}
