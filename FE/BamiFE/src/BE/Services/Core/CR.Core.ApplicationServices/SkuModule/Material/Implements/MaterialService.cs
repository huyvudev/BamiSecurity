using CR.ApplicationBase.Common;
using CR.ApplicationBase.Localization;
using CR.Constants.ErrorCodes;
using CR.Core.ApplicationServices.Common;
using CR.Core.ApplicationServices.SkuModule.Material.Abstracts;
using CR.Core.Domain.Sku;
using CR.Core.Dtos.SkuModule.Material;
using CR.DtoBase;
using CR.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CR.Core.ApplicationServices.SkuModule.Material.Implements;

public class MaterialService : CoreServiceBase, IMaterialService
{
    public MaterialService(ILogger<MaterialService> logger, IHttpContextAccessor httpContext)
        : base(logger, httpContext) { }

    public async Task<Result<CreateMaterialResultDto>> Create(CreateMaterialDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(Create),
            nameof(input),
            input
        );
        if (_dbContext.CoreMaterials.Any(e => e.Code == input.Code))
        {
            return Result<CreateMaterialResultDto>.Failure(
                CoreErrorCode.CoreMaterialCodeExisted,
                this.GetCurrentMethodInfo()
            );
        }
        var newMaterial = new CoreMaterial
        {
            Name = input.Name,
            Code = input.Code,
            Description = input.Description,
        };
        var createMaterial = _dbContext.CoreMaterials.Add(newMaterial).Entity;
        await _dbContext.SaveChangesAsync();
        return Result<CreateMaterialResultDto>.Success(
            new CreateMaterialResultDto { Id = createMaterial.Id }
        );
    }

    public async Task<Result> Delete(int id)
    {
        _logger.LogInformation(
            "{MethodName}: {MaterialId} = {MaterialIdValue}",
            nameof(Delete),
            nameof(id),
            id
        );
        var material = await _dbContext.CoreMaterials.FirstOrDefaultAsync(e => e.Id == id);
        if (material == null)
        {
            return Result.Failure(CoreErrorCode.CoreMaterialNotFound, this.GetCurrentMethodInfo());
        }
        _dbContext.CoreMaterials.Remove(material);
        await _dbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<MaterialDto>> Find(int id)
    {
        _logger.LogInformation(
            "{MethodName}: {MaterialId} = {MaterialIdValue}",
            nameof(Find),
            nameof(id),
            id
        );
        var material = await _dbContext.CoreMaterials.FirstOrDefaultAsync(e => e.Id == id);
        if (material == null)
        {
            return Result<MaterialDto>.Failure(
                CoreErrorCode.CoreMaterialNotFound,
                this.GetCurrentMethodInfo()
            );
        }
        return Result<MaterialDto>.Success(
            new MaterialDto
            {
                Id = material.Id,
                Name = material.Name,
                Code = material.Code,
                Description = material.Description,
            }
        );
    }

    public async Task<Result<PagingResult<MaterialDto>>> FindAll(PagingRequestBaseDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(FindAll),
            nameof(input),
            input
        );
        var listMaterials = _dbContext
            .CoreMaterials.Where(e =>
                string.IsNullOrEmpty(input.Keyword) || e.Name.Contains(input.Keyword.ToLower())
            )
            .Select(e => new MaterialDto
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                Description = e.Description,
            });
        int totalItems = await listMaterials.CountAsync();
        listMaterials = listMaterials.PagingAndSorting(input);
        return Result<PagingResult<MaterialDto>>.Success(
            new PagingResult<MaterialDto>
            {
                TotalItems = totalItems,
                Items = await listMaterials.ToListAsync(),
            }
        );
    }

    public async Task<Result<UpdateMaterialResultDto>> Update(UpdateMaterialDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(Update),
            nameof(input),
            input
        );
        var material = await _dbContext.CoreMaterials.FirstOrDefaultAsync(e => e.Id == input.Id);
        if (material == null)
        {
            return Result<UpdateMaterialResultDto>.Failure(
                CoreErrorCode.CoreMaterialNotFound,
                this.GetCurrentMethodInfo()
            );
        }
        var isCodeDuplicate = await _dbContext.CoreMaterials.AnyAsync(e =>
            e.Code == input.Code && e.Id != input.Id
        );

        if (isCodeDuplicate)
        {
            return Result<UpdateMaterialResultDto>.Failure(
                CoreErrorCode.CoreMaterialCodeExisted,
                this.GetCurrentMethodInfo()
            );
        }
        material.Name = input.Name;
        material.Code = input.Code;
        material.Description = input.Description;
        await _dbContext.SaveChangesAsync();
        return Result<UpdateMaterialResultDto>.Success(
            new UpdateMaterialResultDto { Id = material.Id }
        );
    }
}
