using CR.ApplicationBase.Common;
using CR.Constants.ErrorCodes;
using CR.Core.ApplicationServices.Common;
using CR.Core.ApplicationServices.SkuModule.Sku.Abstracts;
using CR.Core.ApplicationServices.SkuModule.SkuSize.Abstracts;
using CR.Core.Domain.Sku;
using CR.Core.Dtos.SkuModule.Sku;
using CR.Core.Dtos.SkuModule.SkuSize;
using CR.Core.Dtos.SkuModule.SkuSizePkgMockup;
using CR.DtoBase;
using CR.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CR.Core.ApplicationServices.SkuModule.Sku.Implements;

public class SkuService : CoreServiceBase, ISkuService
{
    private readonly ISkuSizeService _skuSizeService;

    public SkuService(
        ILogger<SkuService> logger,
        IHttpContextAccessor httpContext,
        ISkuSizeService skuSizeService
    )
        : base(logger, httpContext)
    {
        _skuSizeService = skuSizeService;
    }

    public async Task<Result<CreateSkuResultDto>> Create(CreateSkuDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(Create),
            nameof(input),
            input
        );
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        // nếu người dùng không nhập code, tự sinh code
        if (string.IsNullOrWhiteSpace(input.Code))
        {
            // lấy các thực thể liên quan để sinh code
            var skuBase = await _dbContext.CoreSkuBases.FirstOrDefaultAsync(sb =>
                sb.Id == input.SkuBaseId
            );
            if (skuBase == null)
            {
                return Result<CreateSkuResultDto>.Failure(
                    CoreErrorCode.CoreSkuBaseNotFound,
                    this.GetCurrentMethodInfo()
                );
            }
            var material = await _dbContext.CoreMaterials.FirstOrDefaultAsync(m =>
                m.Id == input.MaterialId
            );

            if (material == null)
            {
                return Result<CreateSkuResultDto>.Failure(
                    CoreErrorCode.CoreMaterialNotFound,
                    this.GetCurrentMethodInfo()
                );
            }
            var productionMethod = await _dbContext.CoreProductionMethods.FirstOrDefaultAsync(pm =>
                pm.Id == input.ProductMethodId
            );

            if (productionMethod == null)
            {
                return Result<CreateSkuResultDto>.Failure(
                    CoreErrorCode.CoreProductMethodNotFound,
                    this.GetCurrentMethodInfo()
                );
            }

            // Sinh code với thêm hậu tố để đảm bảo tính duy nhất
            int suffix = 1;
            string baseCode = $"{skuBase.Code}_{material.Code}_{productionMethod.Code}";
            input.Code = baseCode;

            // Kiểm tra và sinh code mới nếu trùng
            while (await _dbContext.CoreSkus.AnyAsync(e => e.Code == input.Code))
            {
                input.Code = $"{baseCode}_{suffix}";
                suffix++;
            }
        }

        // kiểm tra code đã tồn tại chưa
        if (_dbContext.CoreSkus.Any(e => e.Code == input.Code))
        {
            return Result<CreateSkuResultDto>.Failure(
                CoreErrorCode.CoreSkuCodeExisted,
                this.GetCurrentMethodInfo()
            );
        }

        var newSku = new CoreSku
        {
            Code = input.Code,
            IsActive = input.IsActive,
            AllowPartnerMarkQc = input.AllowPartnerMarkQc,
            AllowQcMultipleItems = input.AllowQcMultipleItems,
            Description = input.Description,
            IsBigSize = input.IsBigSize,
            MaterialId = input.MaterialId,
            NeedManageMaterials = input.NeedManageMaterials,
            NeedToReview = input.NeedToReview,
            ProductMethodId = input.ProductMethodId,
            SkuBaseId = input.SkuBaseId,
        };

        var createSku = _dbContext.CoreSkus.Add(newSku).Entity;
        await _dbContext.SaveChangesAsync();
        foreach (var size in input.SkuSizeList)
        {
            size.SkuId = createSku.Id;
            var sizeResult = await _skuSizeService.Create(size);
            if (!sizeResult.IsSuccess)
            {
                return Result<CreateSkuResultDto>.Failure(
                    CoreErrorCode.CoreSkuSizeCreateFailed,
                    this.GetCurrentMethodInfo()
                );
            }
        }
        transaction.Commit();
        return Result<CreateSkuResultDto>.Success(new CreateSkuResultDto { Id = createSku.Id });
    }

    public async Task<Result> Delete(int id)
    {
        _logger.LogInformation(
            "{MethodName}: {MethodId} = {MethodIdValue}",
            nameof(Delete),
            nameof(id),
            id
        );

        var sku = await _dbContext.CoreSkus.FirstOrDefaultAsync(e => e.Id == id);
        if (sku == null)
        {
            return Result.Failure(CoreErrorCode.CoreSkuNotFound, this.GetCurrentMethodInfo());
        }

        _dbContext.CoreSkus.Remove(sku);
        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<FindSkuDto>> Find(int id)
    {
        _logger.LogInformation(
            "{MethodName}: {MethodId} = {MethodIdValue}",
            nameof(Find),
            nameof(id),
            id
        );

        var sku = await _dbContext.CoreSkus.Include(e => e.SkuSizes).ThenInclude(e => e.PkgMockups).FirstOrDefaultAsync(e => e.Id == id);
        if (sku == null)
        {
            return Result<FindSkuDto>.Failure(
                CoreErrorCode.CoreSkuNotFound,
                this.GetCurrentMethodInfo()
            );
        }

        return Result<FindSkuDto>.Success(
            new FindSkuDto
            {
                Id = id,
                Code = sku.Code,
                ProductMethodId = sku.ProductMethodId,
                NeedToReview = sku.NeedToReview,
                NeedManageMaterials = sku.NeedManageMaterials,
                MaterialId = sku.MaterialId,
                AllowPartnerMarkQc = sku.AllowPartnerMarkQc,
                AllowQcMultipleItems = sku.AllowQcMultipleItems,
                Description = sku.Description,
                IsActive = sku.IsActive,
                IsBigSize = sku.IsBigSize,
                SkuBaseId = sku.SkuBaseId,
                Sizes = sku
                    .SkuSizes.Select(e => new FindSkuSizeDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        AdditionalWeight = e.AdditionalWeight,
                        BaseCost = e.BaseCost,
                        CostInMeters = e.CostInMeters,
                        Height = e.Height,
                        IsDefault = e.IsDefault,
                        Length = e.Length,
                        PackageDescription = e.PackageDescription,
                        SkuId = e.SkuId,
                        Weight = e.Weight,
                        WeightByVolume = e.WeightByVolume,
                        Width = e.Width,
                        mockUpsList = e
                            .PkgMockups.Select(p => new SkuSizePkgMockupDto
                            {
                                MockupUrl = p.MockupUrl,
                                Id = p.Id,
                                SkuSizeId = e.Id,
                            })
                            .ToList(),
                    })
                    .ToList(),
            }
        );
    }

    public async Task<Result<PagingResult<SkuDto>>> FindAll(PagingRequestBaseDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(FindAll),
            nameof(input),
            input
        );

        var listSkus = _dbContext
            .CoreSkus.Where(e =>
                string.IsNullOrEmpty(input.Keyword) || e.Code.Contains(input.Keyword.ToLower())
            )
            .Select(e => new SkuDto
            {
                Id = e.Id,
                Code = e.Code,
                SkuBaseId = e.SkuBaseId,
                IsBigSize = e.IsBigSize,
                IsActive = e.IsActive,
                Description = e.Description,
                AllowQcMultipleItems = e.AllowQcMultipleItems,
                AllowPartnerMarkQc = e.AllowPartnerMarkQc,
                MaterialId = e.MaterialId,
                NeedManageMaterials = e.NeedManageMaterials,
                NeedToReview = e.NeedToReview,
                ProductMethodId = e.ProductMethodId,
            });

        int totalItems = await listSkus.CountAsync();
        listSkus = listSkus.PagingAndSorting(input);

        return Result<PagingResult<SkuDto>>.Success(
            new PagingResult<SkuDto>
            {
                TotalItems = totalItems,
                Items = await listSkus.ToListAsync(),
            }
        );
    }

    public async Task<Result<UpdateSkuResultDto>> Update(UpdateSkuDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(Update),
            nameof(input),
            input
        );

        var sku = await _dbContext.CoreSkus.FirstOrDefaultAsync(e => e.Id == input.Id);
        if (sku == null)
        {
            return Result<UpdateSkuResultDto>.Failure(
                CoreErrorCode.CoreSkuNotFound,
                this.GetCurrentMethodInfo()
            );
        }
        // nếu người dùng không nhập code, tự sinh code
        if (string.IsNullOrWhiteSpace(input.Code))
        {
            // lấy các thực thể liên quan để sinh code
            var skuBase = await _dbContext.CoreSkuBases.FirstOrDefaultAsync(sb =>
                sb.Id == input.SkuBaseId
            );
            if (skuBase == null)
            {
                return Result<UpdateSkuResultDto>.Failure(
                    CoreErrorCode.CoreSkuBaseNotFound,
                    this.GetCurrentMethodInfo()
                );
            }
            var material = await _dbContext.CoreMaterials.FirstOrDefaultAsync(m =>
                m.Id == input.MaterialId
            );

            if (material == null)
            {
                return Result<UpdateSkuResultDto>.Failure(
                    CoreErrorCode.CoreMaterialNotFound,
                    this.GetCurrentMethodInfo()
                );
            }
            var productionMethod = await _dbContext.CoreProductionMethods.FirstOrDefaultAsync(pm =>
                pm.Id == input.ProductMethodId
            );

            if (productionMethod == null)
            {
                return Result<UpdateSkuResultDto>.Failure(
                    CoreErrorCode.CoreProductMethodNotFound,
                    this.GetCurrentMethodInfo()
                );
            }

            // Sinh code với thêm hậu tố để đảm bảo tính duy nhất
            int suffix = 1;
            string baseCode = $"{skuBase.Code}_{material.Code}_{productionMethod.Code}";
            input.Code = baseCode;

            // Kiểm tra và sinh code mới nếu trùng
            while (await _dbContext.CoreSkus.AnyAsync(e => e.Code == input.Code))
            {
                input.Code = $"{baseCode}_{suffix}";
                suffix++;
            }
        }
        // Kiểm tra code có trùng với sku khác không
        var isCodeDuplicate = await _dbContext.CoreSkus.AnyAsync(e =>
            e.Code == input.Code && e.Id != input.Id // Trùng code nhưng khác Id
        );

        if (isCodeDuplicate)
        {
            return Result<UpdateSkuResultDto>.Failure(
                CoreErrorCode.CoreSkuCodeExisted,
                this.GetCurrentMethodInfo()
            );
        }

        sku.Code = input.Code;
        sku.Description = input.Description;
        sku.AllowPartnerMarkQc = input.AllowPartnerMarkQc;
        sku.ProductMethodId = input.ProductMethodId;
        sku.AllowQcMultipleItems = input.AllowQcMultipleItems;
        sku.IsActive = input.IsActive;
        sku.IsBigSize = input.IsBigSize;
        sku.SkuBaseId = input.SkuBaseId;
        sku.MaterialId = input.MaterialId;
        sku.NeedToReview = input.NeedToReview;
        sku.NeedManageMaterials = input.NeedManageMaterials;

        await _dbContext.SaveChangesAsync();

        return Result<UpdateSkuResultDto>.Success(new UpdateSkuResultDto { Id = sku.Id });
    }
}
