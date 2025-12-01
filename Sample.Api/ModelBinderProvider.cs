using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Text.Json.Serialization;

namespace Sample.Api
{
    #region ModelBinderProvider

    internal sealed class ModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            if (context.BindingInfo.BindingSource == BindingSource.Query)
            {
                return new QueryBinder();
            }
            else
            {
                return null;
            }
        }
    }

    #endregion

    #region QueryBinder

    internal sealed class QueryBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ArgumentNullException.ThrowIfNull(bindingContext, nameof(bindingContext));

            var modelType = bindingContext.ModelType;
            var queryValues = bindingContext.HttpContext.Request.Query;
            var model = Activator.CreateInstance(modelType);

            foreach (var property in modelType.GetProperties())
            {
                var jsonPropertyName = property.GetCustomAttributes(typeof(JsonPropertyNameAttribute), false)
                    .Cast<JsonPropertyNameAttribute>()
                    .FirstOrDefault()?.Name;

                var propertyName = jsonPropertyName ?? property.Name;

                if (queryValues.TryGetValue(propertyName, out var value))
                {
                    if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                    {
                        if (DateTime.TryParse(value, null, System.Globalization.DateTimeStyles.RoundtripKind, out var date))
                        {
                            date = DateTime.SpecifyKind(date, DateTimeKind.Utc).ToLocalTime();
                            property.SetValue(model, date);
                        }
                        else
                        {
                            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid date format.");
                        }
                    }
                    else if (property.PropertyType == typeof(DateOnly) || property.PropertyType == typeof(DateOnly?))
                    {
                        if (DateTime.TryParse(value, null, System.Globalization.DateTimeStyles.RoundtripKind, out var date))
                        {
                            date = DateTime.SpecifyKind(date, DateTimeKind.Utc).ToLocalTime();
                            property.SetValue(model, DateOnly.FromDateTime(date));
                        }
                        else
                        {
                            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid date format.");
                        }
                    }
                    else if (property.PropertyType.IsEnum)
                    {
                        if (Enum.TryParse(property.PropertyType, value.ToString(), out object convertedValue))
                        {
                            property.SetValue(model, convertedValue);
                        }
                        else
                        {
                            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid enum format.");
                        }
                    }
                    else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) && property.PropertyType.GetGenericArguments()[0].IsEnum)
                    {
                        if (string.IsNullOrEmpty(value))
                        {
                            property.SetValue(model, null);
                        }
                        else if (Enum.TryParse(property.PropertyType.IsGenericType
                                && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                                ? Nullable.GetUnderlyingType(property.PropertyType)
                                : property.PropertyType,
                            value.ToString(),
                            out object convertedValue))
                        {
                            property.SetValue(model, convertedValue);
                        }
                        else
                        {
                            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid enum format.");
                        }
                    }
                    else
                    {
                        try
                        {
                            var convertedValue = Convert.ChangeType(value.ToString(), property.PropertyType);
                            property.SetValue(model, convertedValue);
                        }
                        catch (Exception ex)
                        {
                            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, ex.Message);
                        }
                    }
                }
            }

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }

    #endregion

    #region UtcToLocalDateTimeModelBinder

    internal sealed class UtcToLocalDateTimeModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            if (DateTime.TryParse(value, null, System.Globalization.DateTimeStyles.RoundtripKind, out var date))
            {
                date = DateTime.SpecifyKind(date, DateTimeKind.Utc).ToLocalTime();
                bindingContext.Result = ModelBindingResult.Success(date);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid date format.");
            }

            return Task.CompletedTask;
        }
    }

    #endregion
}
