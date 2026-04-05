using System.Globalization;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CR.Utils.DataUtils
{
    public static class StringUtils
    {
        public const string SEPARATOR = ",";

        private static readonly string[] VietnameseSigns = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        public static string RemoveSign4VietnameseString(string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }

        /// <summary>
        /// Che số điện thoại bằng dấu sao (038****291)
        /// </summary>
        /// <param name="phone">Số điện thoại</param>
        /// <param name="startIndex">Vị trí bắt đầu che</param>
        /// <param name="endIndexFromRight">Vị trí che cuối cùng, tính từ phải sang</param>
        /// <returns></returns>
        public static string HidePhone(string? phone, int startIndexFromRight = 3, int hideLen = 4)
        {
            if (!string.IsNullOrEmpty(phone))
            {
                int startIndex = phone.Length - hideLen - startIndexFromRight;
                string hideText = new string('*', hideLen);

                string result = phone.Remove(startIndex, hideLen).Insert(startIndex, hideText);
                return result;
            }

            return "";
        }

        /// <summary>
        /// Che email bằng dấu sao (ha******@gmail.com)
        /// </summary>
        /// <param name="email"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static string HideEmail(string? email, int startIndex = 2)
        {
            if (!string.IsNullOrEmpty(email))
            {
                int splitIndex = email.LastIndexOf("@");

                int hideLen = splitIndex - startIndex;
                string hideText =
                    hideLen > 0 ? new string('*', hideLen) : new string('*', startIndex);

                string result = "";
                if (hideLen > 0)
                {
                    result = email.Remove(startIndex, hideLen).Insert(startIndex, hideText);
                }
                else
                {
                    result = email.Remove(0, startIndex).Insert(0, hideText);
                }
                return result;
            }

            return "";
        }

        /// <summary>
        /// Chuyển string sang dạng snake upper case
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ToSnakeUpperCase(this string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (text.Length < 2)
            {
                return text.ToUpper();
            }
            var sb = new StringBuilder();
            sb.Append(char.ToLowerInvariant(text[0]));
            for (int i = 1; i < text.Length; ++i)
            {
                char c = text[i];
                if (char.IsUpper(c))
                {
                    sb.Append('_');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// Chuyển sang chữ không dấu
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string? ToUnSign(this string? str)
        {
            if (str == null)
            {
                return null;
            }
            string stFormD = str.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc =
                    System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }

        public static string? ToCustomNormalize(this string? str)
        {
            if (str == null)
            {
                return null;
            }
            str = str.Trim().Replace(' ', '-');
            return ToUnSign(str);
        }

        /// <summary>
        /// Lấy chữ cái đầu từng từ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string? FirstLetterEachWord(this string str)
        {
            if (str == null)
            {
                return null;
            }
            string shortName = "";
            str.Split(' ').ToList().ForEach(i => shortName += i[0].ToString());
            return shortName;
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        /// <summary>
        /// Hàm chuyển sang dạng PascalCase (Ex: "examplePascal -> ExamplePascal")
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string? ToPascalCase(this string text)
        {
            if (text == null)
            {
                return null;
            }
            if (text.Length < 2)
            {
                return text.ToUpper();
            }
            string pascalCase = char.ToUpper(text[0]) + text.Substring(1);

            return pascalCase;
        }

        /// <returns></returns>
        /// <summary>
        /// Converts a string to camelCase.
        /// </summary>
        /// <param name="input">The input string to convert.</param>
        /// <returns>A camelCase version of the input string.</returns>
        public static string ToCamelCase(this string input)
        {
            if (string.IsNullOrEmpty(input) || input.Length < 2)
            {
                return input; // Nếu chuỗi rỗng hoặc chỉ có 1 ký tự, trả về chuỗi ban đầu
            }

            // Chuyển ký tự đầu tiên thành chữ thường và giữ nguyên các ký tự còn lại
            return char.ToLowerInvariant(input[0]) + input.Substring(1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <param name="replacePairValue"></param>
        /// <returns></returns>
        public static string? ReplaceMultipleText(
            this string? input,
            params (string replacements, string? values)[] replacePairValue
        )
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            StringBuilder resultBuilder = new(input);
            foreach (var (replacements, values) in replacePairValue)
            {
                if (!string.IsNullOrEmpty(values) && input.Contains(replacements!))
                {
                    resultBuilder.Replace(replacements, values);
                }
            }
            return resultBuilder.ToString();
        }

        /// <summary>
        /// Tên tổ chức trong email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string GetOrganizationFromEmail(string email)
        {
            // Tách phần sau dấu @
            string domain = email.Split('@')[1];

            // Tách phần trước dấu . đầu tiên
            string organization = domain.Split('.')[0];

            return organization;
        }

        /// <summary>
        /// Sinh chuỗi ngẫu nhiên từ độ dài cho trước
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomASCIIString(int length)
        {
            const string asciiCharacters =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=[]{}|;:,.<>?/";
            var random = new Random();
            var result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(0, asciiCharacters.Length);
                result.Append(asciiCharacters[randomIndex]);
            }

            return result.ToString();
        }

        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
