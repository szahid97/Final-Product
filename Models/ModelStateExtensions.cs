using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

public static class ModelStateExtensions
{
    public static Dictionary<string, List<string>> ToErrorDictionary(this ModelStateDictionary modelState)
    {
        return modelState
            .Where(x => x.Value?.Errors?.Count > 0) // Null-conditional checks
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors?
                    .Select(e => e.ErrorMessage ?? "Unknown error")
                    .ToList() ?? new List<string>()
            );
    }
}