using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TravelTracker.Views
{
    public static class ModelStateExtension
    {
        public static bool IsFieldInvalid(this ModelStateDictionary modelStateDictionary, string key)
        {
            return modelStateDictionary.GetFieldValidationState(key).Equals(ModelValidationState.Invalid);
        }

        public static bool IsAFieldInvalid(this ModelStateDictionary modelStateDictionary, params string[] keys)
        {
            return keys.Any(modelStateDictionary.IsFieldInvalid);
        }
    }
}
