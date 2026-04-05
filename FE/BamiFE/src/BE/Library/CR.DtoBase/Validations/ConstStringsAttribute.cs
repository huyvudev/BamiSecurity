using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CR.DtoBase.Validations;

public class ConstStringsAttribute : ValidationAttribute
{
    private readonly Type StaticClassType;

    public ConstStringsAttribute(Type staticClassType)
    {
        StaticClassType = staticClassType;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || StaticClassType == null)
        {
            return ValidationResult.Success;
        }

        var allowableValues = GetConstStringValues(StaticClassType);
        string currValue = string.Empty;
        var type = value.GetType();
        var interfaces = type.GetInterfaces();
        if (type == typeof(string))
        {
            currValue = value.ToString() ?? string.Empty;
            if (allowableValues.Contains(currValue))
            {
                return ValidationResult.Success;
            }
        }
        else if (type.IsArray)
        {
            var array = value as IEnumerable<string>;
            var listErr = new List<string>();
            foreach (var item in array ?? [])
            {
                if (!allowableValues.Contains(item))
                {
                    listErr.Add(item);
                }
            }
            if (listErr.Count == 0)
            {
                return ValidationResult.Success;
            }
            currValue = string.Join(", ", listErr);
        }
        else if (interfaces.Any(i => i.Name == typeof(IEnumerable<>).Name))
        {
            var genericArguments = type.GetGenericArguments();
            if (genericArguments.Length == 0)
            {
                return ValidationResult.Success;
            }

            var argumentType = type.GetGenericArguments()[0];
            if (argumentType != typeof(string))
            {
                return new ValidationResult($"'{argumentType}' is not string.");
            }

            var array = value as IEnumerable<string>;
            var listErr = new List<string>();
            foreach (var item in array ?? [])
            {
                if (!allowableValues.Contains(item))
                {
                    listErr.Add(item);
                }
            }
            if (listErr.Count == 0)
            {
                return ValidationResult.Success;
            }
            currValue = string.Join(", ", listErr);
        }
        else
        {
            return new ValidationResult($"'{type.Name}' is not support attribute 'ConstStrings'.");
        }

        ErrorMessage ??= validationContext.Localize("error_validation_ConstStrings");
        var msg = string.Format(ErrorMessage, $"\"{currValue}\"", $"[{string.Join(", ", allowableValues)}]");
        return new ValidationResult(msg);
    }

    private string[] GetConstStringValues(Type staticClassType)
    {
        var constFields = staticClassType
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(f => f.FieldType == typeof(string) && f.IsLiteral && !f.IsInitOnly)
            .Select(f => f.GetValue(null)?.ToString())
            .Where(v => v != null)
            .Select(v => v!)
            .OrderBy(v => v)
            .ToArray();

        return constFields;
    }
}
