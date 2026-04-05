using CR.DtoBase;
using CR.Core.Dtos.SkuModule.ProductionMethod;

namespace CR.Core.ApplicationServices.SkuModule.ProductionMethod.Abstacts;
public interface IProductMethodService
{
    Task<Result<CreateProductionMethodResultDto>> Create(CreateProductionMethodDto input);
    Task<Result<UpdateProductionMethodResultDto>> Update(UpdateProductionMethodDto input);
    Task<Result> Delete(int id);
    Task<Result<ProductionMethodDto>> Find(int id);
    Task<Result<PagingResult<ProductionMethodDto>>> FindAll(PagingRequestBaseDto input);
}
