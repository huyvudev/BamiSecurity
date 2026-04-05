using CR.Core.Dtos.SkuModule.SkuSize;
using CR.DtoBase;

namespace CR.Core.ApplicationServices.SkuModule.SkuSize.Abstracts;
public interface ISkuSizeService
{
    Task<Result<CreateSkuSizeResultDto>> Create(CreateSkuSizeDto input);
    Task<Result<UpdateSkuSizeResultDto>> Update(UpdateSkuSizeDto input);
    Task<Result> Delete(int id);
    Task<Result<FindSkuSizeDto>> Find(int id);
    Task<Result<PagingResult<SkuSizeDto>>> FindAll(PagingRequestBaseDto input);
}
