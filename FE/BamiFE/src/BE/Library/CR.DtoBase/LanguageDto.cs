using CR.DtoBase.Validations;

namespace CR.DtoBase;

public class LanguageDto
{
    private string? _language = null!;

    [CustomMaxLength(100)]
    public string? Language
    {
        get => _language;
        set => _language = value?.Trim();
    }
}
