using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CR.DtoBase.Validations
{
    public class FileMaxLengthAttribute : ValidationAttribute
    {
        /// <summary>
        /// Kích thước tối đa của file đơn vị bytes
        /// </summary>
        public long MaxLength { get; set; }

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

                if (file.Length <= MaxLength)
                {
                    return ValidationResult.Success;
                }
            }
            else if (type.IsAssignableTo(typeof(IEnumerable<IFormFile>)))
            {
                var files = (IEnumerable<IFormFile>)value;
                if (files.Sum(f => f.Length) <= MaxLength)
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return ValidationResult.Success;
            }

            ErrorMessage = validationContext.Localize("error_validation_FileMaxLength");
            var msg =
                MaxLength <= 1024 * 1024
                    ? string.Format(ErrorMessage, MaxLength / 1024.0 + " KB")
                    : string.Format(ErrorMessage, MaxLength / 1024.0 / 1024.0 + " MB");
            return new ValidationResult(msg);
        }
    }
}
