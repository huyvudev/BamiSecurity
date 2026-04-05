using CR.Core.Dtos.SkuModule.Material;
using CR.DtoBase;

namespace CR.Core.ApplicationServices.SkuModule.Material.Abstracts;
public interface IMaterialService
{
    Task<Result<CreateMaterialResultDto>> Create(CreateMaterialDto input);
    Task<Result<UpdateMaterialResultDto>> Update(UpdateMaterialDto input);
    Task<Result> Delete(int id);
    Task<Result<MaterialDto>> Find(int id);
    Task<Result<PagingResult<MaterialDto>>> FindAll(PagingRequestBaseDto input);
}
