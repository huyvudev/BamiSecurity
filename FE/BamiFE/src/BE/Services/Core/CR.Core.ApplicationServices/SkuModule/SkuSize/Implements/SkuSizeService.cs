using CR.ApplicationBase.Common;
using CR.Constants.ErrorCodes;
using CR.Core.ApplicationServices.Common;
using CR.Core.ApplicationServices.SkuModule.SkuSize.Abstracts;
using CR.Core.ApplicationServices.SkuModule.SkuSizePkgMockup.Abstracts;
using CR.Core.Domain.Sku;
using CR.Core.Dtos.SkuModule.SkuSize;
using CR.Core.Dtos.SkuModule.SkuSizePkgMockup;
using CR.DtoBase;
using CR.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class SkuSizeService : CoreServiceBase, ISkuSizeService
{
    private readonly ISkuSizePkgMockupService _skuSizePkgMockupService;

    public SkuSizeService(
        ILogger<SkuSizeService> logger,
        IHttpContextAccessor httpContext,
        ISkuSizePkgMockupService skuSizePkgMockupService
    )
        : base(logger, httpContext)
    {
        _skuSizePkgMockupService = skuSizePkgMockupService;
    }

    public async Task<Result<CreateSkuSizeResultDto>> Create(CreateSkuSizeDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(Create),
            nameof(input),
            input
        );
        //bỏ mặc định cũ
        if (input.IsDefault)
        {
            var oldDefault = _dbContext.CoreSkuSizes.FirstOrDefault(e => e.IsDefault);
            if (oldDefault != null)
            {
                oldDefault.IsDefault = false;
            }
        }
        var newSkuSize = new CoreSkuSize
        {
            Name = input.Name,
            Width = input.Width,
            Height = input.Height,
            Length = input.Length,
            Weight = input.Weight,
            AdditionalWeight = input.AdditionalWeight,
            IsDefault = input.IsDefault,
            BaseCost = input.BaseCost,
            CostInMeters = input.CostInMeters,
            WeightByVolume = input.WeightByVolume,
            PackageDescription = input.PackageDescription,
            SkuId = input.SkuId.HasValue ? input.SkuId.Value : 0,
        };

        var createdSkuSize = _dbContext.CoreSkuSizes.Add(newSkuSize).Entity;
        await _dbContext.SaveChangesAsync();

        foreach (var mock in input.MockupsList)
        {
            mock.SkuSizeId = createdSkuSize.Id;
            var mockupResult = await _skuSizePkgMockupService.Create(mock);
            if (!mockupResult.IsSuccess)
            {
                return Result<CreateSkuSizeResultDto>.Failure(
                    CoreErrorCode.CoreSkuMockUpCreateFailed,
                    this.GetCurrentMethodInfo()
                );
            }
        }

        return Result<CreateSkuSizeResultDto>.Success(
            new CreateSkuSizeResultDto { Id = createdSkuSize.Id }
        );
    }

    public async Task<Result> Delete(int id)
    {
        _logger.LogInformation(
            "{MethodName}: {SkuSizeId} = {SkuSizeIdValue}",
            nameof(Delete),
            nameof(id),
            id
        );

        var skuSize = await _dbContext.CoreSkuSizes.FirstOrDefaultAsync(e => e.Id == id);
        if (skuSize == null)
        {
            return Result.Failure(CoreErrorCode.CoreSkuSizeNotFound, this.GetCurrentMethodInfo());
        }

        _dbContext.CoreSkuSizes.Remove(skuSize);
        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<FindSkuSizeDto>> Find(int id)
    {
        _logger.LogInformation(
            "{MethodName}: {SkuSizeId} = {SkuSizeIdValue}",
            nameof(Find),
            nameof(id),
            id
        );

        var skuSize = await _dbContext.CoreSkuSizes.Include(e => e.PkgMockups).FirstOrDefaultAsync(e => e.Id == id);
        if (skuSize == null)
        {
            return Result<FindSkuSizeDto>.Failure(
                CoreErrorCode.CoreSkuSizeNotFound,
                this.GetCurrentMethodInfo()
            );
        }

        return Result<FindSkuSizeDto>.Success(
            new FindSkuSizeDto
            {
                Id = skuSize.Id,
                Name = skuSize.Name,
                Width = skuSize.Width,
                Height = skuSize.Height,
                Length = skuSize.Length,
                Weight = skuSize.Weight,
                AdditionalWeight = skuSize.AdditionalWeight,
                IsDefault = skuSize.IsDefault,
                BaseCost = skuSize.BaseCost,
                CostInMeters = skuSize.CostInMeters,
                WeightByVolume = skuSize.WeightByVolume,
                PackageDescription = skuSize.PackageDescription,
                SkuId = skuSize.SkuId,
                mockUpsList = skuSize
                    .PkgMockups.Select(e => new SkuSizePkgMockupDto
                    {
                        MockupUrl = e.MockupUrl,
                        SkuSizeId = skuSize.Id,
                        Id = e.Id,
                    })
                    .ToList(),
            }
        );
    }

    public async Task<Result<PagingResult<SkuSizeDto>>> FindAll(PagingRequestBaseDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(FindAll),
            nameof(input),
            input
        );

        var listSkuSizes = _dbContext
            .CoreSkuSizes.Where(e =>
                string.IsNullOrEmpty(input.Keyword) || e.Name.Contains(input.Keyword.ToLower())
            )
            .Select(e => new SkuSizeDto
            {
                Id = e.Id,
                Name = e.Name,
                Width = e.Width,
                Height = e.Height,
                Length = e.Length,
                Weight = e.Weight,
                AdditionalWeight = e.AdditionalWeight,
                IsDefault = e.IsDefault,
                BaseCost = e.BaseCost,
                CostInMeters = e.CostInMeters,
                WeightByVolume = e.WeightByVolume,
                PackageDescription = e.PackageDescription,
                SkuId = e.SkuId,
            });

        int totalItems = await listSkuSizes.CountAsync();
        listSkuSizes = listSkuSizes.PagingAndSorting(input);

        return Result<PagingResult<SkuSizeDto>>.Success(
            new PagingResult<SkuSizeDto>
            {
                TotalItems = totalItems,
                Items = await listSkuSizes.ToListAsync(),
            }
        );
    }

    public async Task<Result<UpdateSkuSizeResultDto>> Update(UpdateSkuSizeDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(Update),
            nameof(input),
            input
        );
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        var skuSize = await _dbContext
            .CoreSkuSizes.Include(e => e.PkgMockups)
            .FirstOrDefaultAsync(e => e.Id == input.Id);
        if (skuSize == null)
        {
            return Result<UpdateSkuSizeResultDto>.Failure(
                CoreErrorCode.CoreSkuSizeNotFound,
                this.GetCurrentMethodInfo()
            );
        }

        if (await _dbContext.CoreSkuSizes.AnyAsync(e => e.Name == input.Name && e.Id != input.Id))
        {
            return Result<UpdateSkuSizeResultDto>.Failure(
                CoreErrorCode.CoreSkuSizeNameExisted,
                this.GetCurrentMethodInfo()
            );
        }
        //bỏ mặc định cũ
        if (input.IsDefault)
        {
            var oldDefault = _dbContext.CoreSkuSizes.FirstOrDefault(e => e.IsDefault);
            if (oldDefault != null)
            {
                oldDefault.IsDefault = false;
            }
        }

        skuSize.Name = input.Name;
        skuSize.Width = input.Width;
        skuSize.Height = input.Height;
        skuSize.Length = input.Length;
        skuSize.Weight = input.Weight;
        skuSize.AdditionalWeight = input.AdditionalWeight;
        skuSize.IsDefault = input.IsDefault;
        skuSize.BaseCost = input.BaseCost;
        skuSize.CostInMeters = input.CostInMeters;
        skuSize.WeightByVolume = input.WeightByVolume;
        skuSize.PackageDescription = input.PackageDescription;

        // Sửa hoặc thêm
        var existingMockupIds = skuSize.PkgMockups.Select(m => m.Id).ToList();
        var inputMockupIds = input.MockupsList.Select(m => m.Id).ToList();

        // Xóa mockup không còn trong list
        var mockupsToDelete = skuSize
            .PkgMockups.Where(m => !inputMockupIds.Contains(m.Id))
            .ToList();
        _dbContext.CoreSkuSizePkgMockups.RemoveRange(mockupsToDelete);

        foreach (var mockup in input.MockupsList)
        {
            if (existingMockupIds.Contains(mockup.Id))
            {
                var existingMockup = skuSize.PkgMockups.First(m => m.Id == mockup.Id);
                existingMockup.MockupUrl = mockup.MockupUrl;
            }
            else
            {
                var newMockup = new CoreSkuSizePkgMockup
                {
                    MockupUrl = mockup.MockupUrl,
                    SkuSizeId = skuSize.Id,
                };
                _dbContext.CoreSkuSizePkgMockups.Add(newMockup);
            }
        }

        await _dbContext.SaveChangesAsync();
        transaction.Commit();
        return Result<UpdateSkuSizeResultDto>.Success(
            new UpdateSkuSizeResultDto { Id = skuSize.Id }
        );
    }
}
