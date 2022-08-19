using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTR.Infrastructure.Services.CommaSeparated
{
    [ModelBinder(BinderType = typeof(CommaSeperatedArrayEntryBinder))]
    public class CommaSeparatedIntListModelBinder : List<int>
    {
        public CommaSeparatedIntListModelBinder()
        { 
        }

        public CommaSeparatedIntListModelBinder(IEnumerable<int> args)
        {
            this.AddRange(args);
        }

        public static bool Convert(string source, out int result)
        {
            return int.TryParse(source, out result);
        }
    }


    public class CommaSeperatedArrayEntryBinder : IModelBinder
    {


        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelName = bindingContext.ModelName;

            // Try to fetch the value of the argument by name
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;

            // Check if the argument value is null or empty
            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }
            
            var complete = default(bool);
            var result = value
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x =>
                {
                    if (complete |= !CommaSeparatedIntListModelBinder.Convert(x.Trim(), out int r))
                        bindingContext.ModelState.TryAddModelError(
                            modelName, "Author Id must be an integer.");
                    return r;
                });

            if(!complete)
                bindingContext.Result = ModelBindingResult.Success(new CommaSeparatedIntListModelBinder(result));

            return Task.CompletedTask;
        }
    }
}
