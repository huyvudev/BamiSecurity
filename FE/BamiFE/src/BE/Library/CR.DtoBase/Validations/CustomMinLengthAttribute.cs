using System.ComponentModel.DataAnnotations;

namespace CR.DtoBase.Validations
{
    public class CustomMinLengthAttribute : MinLengthAttribute, IValidationAttribute
    {
        public CustomMinLengthAttribute(int length)
            : base(length) { }

        public string? ErrorMessageLocalization { get; set; }

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext
        )
        {
            ErrorMessage = string.Format(
                validationContext.Localize(
                    ErrorMessageLocalization ?? "error_validation_field_CustomMinLength"
                ),
                Length
            );
            return base.IsValid(value, validationContext);
        }
    }
}
