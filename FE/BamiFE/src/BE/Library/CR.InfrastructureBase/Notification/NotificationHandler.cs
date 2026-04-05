using CR.Constants.Common;
using CR.DtoBase.Notification;
using CR.Utils.DataUtils;
using System.Text.Json;

namespace CR.InfrastructureBase.Notification
{
    /// <summary>
    /// Strategy xử lý consume message
    /// </summary>
    public abstract class NotificationHandler
    {
        /// <summary>
        /// Tiêu thụ message
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public abstract Task Consume<TData>(NotificationMessage<string, TData> message)
            where TData : class;

        public virtual Task<string> FormatDateTime(int? tenantId, DateTime? dateTime)
        {
            return Task.FromResult(dateTime?.ToString("yyyy/MM/dd HH:mm") ?? string.Empty);
        }

        /// <summary>
        /// Lấy cặp key value từ object data truyền vào
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="tenantId">Id tenant để lấy các config format</param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected async Task<Dictionary<string, string>> GetDataKeyValuePairs<TData>(int? tenantId, TData? data)
            where TData : class
        {
            var result = new Dictionary<string, string>();
            var jsonElement = data as JsonElement?;
            if (jsonElement == null)
            {
                return result;
            }

            var jsonDoc = JsonDocument.Parse(jsonElement.Value.ToString());
            foreach (var property in jsonDoc.RootElement.EnumerateObject())
            {
                var name = property.Name;
                var valueKind = property.Value.ValueKind;
                if (
                    valueKind == JsonValueKind.String
                    && property.Value.TryGetDateTime(out DateTime dateTime)
                )
                {
                    result.Add(name, await FormatDateTime(tenantId, dateTime));
                }
                else if (
                    valueKind == JsonValueKind.String
                    || valueKind == JsonValueKind.Number
                    || valueKind == JsonValueKind.Null
                )
                {
                    var value = property.Value.ToString();
                    result.Add(name, value);
                }
            }
            return result;
        }

        private static string ReplaceTemplateSpecialVariables(
            Dictionary<string, string> keyValuePairs,
            string template,
            ContentTypes contentType = ContentTypes.Html
        )
        {
            if (template is null)
            {
                return string.Empty;
            }

            string result = template;

            return result;
        }

        /// <summary>
        /// Thay thế các biến mẫu trong chuỗi template bằng các giá trị tương ứng từ từ điển key-value.
        /// </summary>
        /// <param name="keyValuePairs">Từ điển chứa các cặp key-value.</param>
        /// <param name="template">Chuỗi template chứa các biến mẫu.</param>
        /// <param name="contentType">Loại nội dung</param>
        /// <returns>Chuỗi đã được thay thế các biến mẫu.</returns>
        protected static string ReplaceTemplateVariables(
            Dictionary<string, string> keyValuePairs,
            string template,
            ContentTypes contentType = ContentTypes.Html
        )
        {
            if (template is null)
            {
                return string.Empty;
            }

            string result = ReplaceTemplateSpecialVariables(keyValuePairs, template, contentType);

            foreach (var kvp in keyValuePairs ?? [])
            {
                result = result.Replace(
                    "{" + kvp.Key + "}",
                    kvp.Value,
                    StringComparison.OrdinalIgnoreCase
                );
            }

            return result;
        }

        /// <summary>
        /// Thay thế các biến mẫu trong chuỗi template bằng các giá trị tương ứng từ từ điển key-value.
        /// </summary>
        /// <param name="tenantId">Id tenant để lấy các config format</param>
        /// <param name="data">Chuyển object sang dạng từ điển chứa các cặp key-value.</param>
        /// <param name="template">Chuỗi template chứa các biến mẫu.</param>
        /// <param name="contentType">Loại nội dung</param>
        /// <returns>Chuỗi đã được thay thế các biến mẫu.</returns>
        protected async Task<string> ReplaceTemplateVariables<TData>(
            int? tenantId,
            TData? data,
            string template,
            ContentTypes contentType = ContentTypes.Html
        )
            where TData : class
        {
            return ReplaceTemplateVariables(
                await GetDataKeyValuePairs(tenantId, data),
                template,
                contentType
            );
        }
    }
}
