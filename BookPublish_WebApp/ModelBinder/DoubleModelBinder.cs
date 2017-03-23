using System;
using System.Globalization;
using System.Web.Mvc;

namespace BookPublish_WebApp.ModelBinder
{
    public class DoubleModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            ModelState modelState = new ModelState { Value = valueProviderResult };
            object actualValue = null;

            try
            {
                actualValue = Convert.ToDouble(valueProviderResult.AttemptedValue,
                    CultureInfo.InvariantCulture);
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);

            return actualValue;

        }
    }
}