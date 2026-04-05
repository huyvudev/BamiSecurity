using CR.Core.Dtos.SkuModule.Sku;
using CR.DtoBase;

namespace CR.Core.ApplicationServices.SkuModule.Sku.Abstracts;
public interface ISkuService
{
    Task<Result<CreateSkuResultDto>> Create(CreateSkuDto input);
    Task<Result<UpdateSkuResultDto>> Update(UpdateSkuDto input);
    Task<Result> Delete(int id);
    Task<Result<FindSkuDto>> Find(int id);
    Task<Result<PagingResult<SkuDto>>> FindAll(PagingRequestBaseDto input);
}
