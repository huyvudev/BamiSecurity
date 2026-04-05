using CR.DtoBase;
using CR.LLM.Dtos.Generate;

namespace CR.LLM.Abstracts
{
    public interface ILlmService
    {
        /// <summary>
        /// Sinh nội dung parse sang Dto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prompt"></param>
        /// <returns></returns>
        Task<Result<T>> GenerateContentAsync<T>(string prompt) where T : class;

        /// <summary>
        /// Tạo nội dung văn bản dựa trên đầu vào từ người dùng
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<GenerateMessageDto>> GenerateTextAsync(
            string prompt,
            CancellationToken cancellationToken = default
        );
    }
}
