using CR.DtoBase;

namespace CR.InfrastructureBase.LoadFile;

public interface IImageLoader
{
    Task<Result<Stream>> LoadImageFromUrlAsync(string url);
}
