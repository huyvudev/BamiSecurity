using CR.ApplicationBase.Localization;

namespace CR.Core.Dtos.AuthenticationModule.UserDto.ExportExcel
{
    /// <summary>
    /// Class abstract khi gen excel
    /// </summary>
    public abstract class GenColName
    {
        protected readonly ILocalization _localization;
        public GenColName(ILocalization localization)
        {
            _localization = localization;
        }

        public abstract string SourceDisplay(string? source);
    }
}
