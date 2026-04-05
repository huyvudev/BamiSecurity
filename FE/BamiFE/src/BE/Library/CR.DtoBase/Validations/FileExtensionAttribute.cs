using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CR.DtoBase.Validations
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        public string[] AllowableExtensions { get; set; } = [];

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext
        )
        {
            if (value == null)
                return ValidationResult.Success;

            var type = value.GetType();
            if (type.IsAssignableTo(typeof(IFormFile)))
            {
                var file = (IFormFile)value;
                if (AllowableExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                {
                    return ValidationResult.Success;
                }
            }
            else if (type.IsAssignableTo(typeof(IEnumerable<IFormFile>)))
            {
                var files = (IEnumerable<IFormFile>)value;
                if (
                    files
                        .Select(f => Path.GetExtension(f.FileName).ToLower())
                        .All(ext => AllowableExtensions.Contains(ext))
                )
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                throw new InvalidCastException("Field is not instance of IFormFile");
            }
            ErrorMessage = validationContext.Localize("error_validation_FileExtension");
            var msg = string.Format(ErrorMessage, string.Join(", ", AllowableExtensions ?? []));
            return new ValidationResult(msg);
        }
    }
}
