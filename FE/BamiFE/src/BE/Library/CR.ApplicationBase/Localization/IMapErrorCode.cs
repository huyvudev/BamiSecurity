namespace CR.ApplicationBase.Localization
{
    /// <summary>
    /// Map error code sang message thông qua ILocalization
    /// </summary>
    public interface IMapErrorCode
    {
        /// <summary>
        /// Lấy error message
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        string GetErrorMessage(int errorCode);

        /// <summary>
        /// Lấy message key cho error code
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        string GetErrorMessageKey(int errorCode);

        /// <summary>
        /// Thử lấy error message nếu có
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        string? TryGetErrorMessage(int errorCode);
    }
}
