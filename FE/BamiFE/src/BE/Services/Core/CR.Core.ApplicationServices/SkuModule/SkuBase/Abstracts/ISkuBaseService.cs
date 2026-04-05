using CR.Core.Dtos.SkuModule.SkuBase;
using CR.DtoBase;

namespace CR.Core.ApplicationServices.SkuModule.SkuBase.Abstracts;
public interface ISkuBaseService
{
    Task<Result<CreateSkuBaseResultDto>> Create(CreateSkuBaseDto input);
    Task<Result<UpdateSkuBaseResultDto>> Update(UpdateSkuBaseDto input);
    Task<Result> Delete(int id);
    Task<Result<SkuBaseDto>> Find(int id);
    Task<Result<PagingResult<SkuBaseDto>>> FindAll(PagingRequestBaseDto input);
}
