using CR.ApplicationBase.Localization;
using CR.Core.Infrastructure.Persistence;

namespace CR.Core.Dtos.AuthenticationModule.UserDto.ExportExcel
{
    public class CoreUserColName : GenColName
    {

        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string? Password;

        /// <summary>
        /// Tên đầy đủ
        /// </summary>
        public string? FullName;

        /// <summary>
        /// Loại tài khoản
        /// </summary>
        public string? UserType;

        /// <summary>
        /// Giới tính
        /// </summary>
        public string? Gender;

        /// <summary>
        /// Mã số   (có thể là mã nhân viên hoặc MSSV)
        /// </summary>
        public string? UserCode;
        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string? PhoneNumber;

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public string? DateOfBirth;

        /// <summary>
        /// Quê quán
        /// </summary>
        public string? Hometown;

        /// <summary>
        /// Số ID (CCCD)
        /// </summary>
        public string? IdCode;
        /// <summary>
        /// Email
        /// </summary>
        public string? Email;

        public override string SourceDisplay(string? source)
        {
            return source switch
            {
                nameof(Password) => _localization.Localize("import_excel_password"),
                nameof(FullName) => _localization.Localize("import_excel_fullname"),
                nameof(UserType) => _localization.Localize("import_excel_usertype"),
                nameof(Email) => _localization.Localize("import_excel_email"),
                nameof(Gender) => _localization.Localize("import_excel_gender"),
                nameof(UserCode) => _localization.Localize("import_excel_usercode"),
                nameof(PhoneNumber) => _localization.Localize("import_excel_phonenumber"),
                nameof(DateOfBirth) => _localization.Localize("import_excel_dateofbirth"),
                nameof(Hometown) => _localization.Localize("import_excel_hometown"),
                nameof(IdCode) => _localization.Localize("import_excel_idcode"),
                _ => string.Empty,
            };
        }

        public CoreUserColName(ILocalization localization)
            : base(localization) { }
    }
}
