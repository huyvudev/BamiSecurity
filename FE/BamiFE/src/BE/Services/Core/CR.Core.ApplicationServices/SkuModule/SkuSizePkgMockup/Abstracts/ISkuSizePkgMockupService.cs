using CR.Core.Dtos.SkuModule.SkuSizePkgMockup;
using CR.DtoBase;

namespace CR.Core.ApplicationServices.SkuModule.SkuSizePkgMockup.Abstracts;
public interface ISkuSizePkgMockupService
{
    Task<Result<CreateSkuSizePkgMockupResultDto>> Create(CreateSkuSizePkgMockupDto input);
    Task<Result<UpdateSkuSizePkgMockupResultDto>> Update(UpdateSkuSizePkgMockupDto input);
    Task<Result> Delete(int id);
    Task<Result<SkuSizePkgMockupDto>> Find(int id);
    Task<Result<PagingResult<SkuSizePkgMockupDto>>> FindAll(PagingRequestBaseDto input);
}
