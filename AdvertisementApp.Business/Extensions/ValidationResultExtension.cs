using AdvertisementApp.Common;
using FluentValidation.Results;
using System.Collections.Generic;

namespace AdvertisementApp.Business.Extensions
{
    //extension
    public static class ValidationResultExtension
    {
        public static List<CustomValidationError> ConvertToCustomValidationError(this ValidationResult validationResult)
        {
            List<CustomValidationError> customErrors = new List<CustomValidationError>();
            foreach(var item in validationResult.Errors)
            {
                customErrors.Add(new()
                {
                    ErrorMessage = item.ErrorMessage,
                    PropertyName = item.PropertyName,
                });
            }
            return customErrors;
        }
    }
}
