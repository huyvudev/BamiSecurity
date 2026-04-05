using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CR.DtoBase.Validations;

public class ListEnumDataTypeAttribute : ValidationAttribute
{
    private readonly Type EnumType;

    public ListEnumDataTypeAttribute(Type enumType)
    {
        if (!enumType.IsEnum)
        {
            throw new ArgumentException($"{enumType.Name} is not an enum type.");
        }

        EnumType = enumType;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || EnumType == null || !EnumType.IsEnum)
        {
            return ValidationResult.Success;
        }

        var enumValues = Enum.GetValues(EnumType).Cast<int>().ToList();
        if (enumValues.Count == 0)
        {
            return ValidationResult.Success;
        }

        var type = value.GetType();
        if (type.IsGenericType)
        {
            var genericType = type.GetGenericTypeDefinition();
            if (genericType.GetInterfaces().Any(i => i.Name == typeof(IEnumerable<>).Name))
            {
                var argumentType = type.GetGenericArguments()[0];
                if (!argumentType.IsEnum)
                {
                    return new ValidationResult($"'{argumentType}' is not Enum.");
                }

                var list =
                    JsonSerializer.Deserialize<IEnumerable<int>>(JsonSerializer.Serialize(value))
                    ?? [];

                foreach (var item in list)
                {
                    if (!enumValues.Contains(item))
                    {
                        return new ValidationResult(
                            $"The value '{item}' is not a valid {EnumType.Name}: {string.Join(", ", enumValues)}"
                        );
                    }
                }
            }
        }

        return ValidationResult.Success;
    }
}
