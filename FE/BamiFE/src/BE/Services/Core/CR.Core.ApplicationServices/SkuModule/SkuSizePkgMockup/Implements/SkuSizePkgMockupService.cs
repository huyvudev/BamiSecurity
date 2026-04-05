using CR.ApplicationBase.Common;
using CR.Constants.ErrorCodes;
using CR.Core.ApplicationServices.Common;
using CR.Core.ApplicationServices.SkuModule.SkuSizePkgMockup.Abstracts;
using CR.Core.Domain.Sku;
using CR.Core.Dtos.SkuModule.SkuSizePkgMockup;
using CR.DtoBase;
using CR.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class SkuSizePkgMockupService : CoreServiceBase, ISkuSizePkgMockupService
{
    public SkuSizePkgMockupService(
        ILogger<SkuSizePkgMockupService> logger,
        IHttpContextAccessor httpContext
    )
        : base(logger, httpContext) { }

    public async Task<Result<CreateSkuSizePkgMockupResultDto>> Create(
        CreateSkuSizePkgMockupDto input
    )
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(Create),
            nameof(input),
            input
        );

        var newMockup = new CoreSkuSizePkgMockup
        {
            MockupUrl = input.MockupUrl,
            SkuSizeId = input.SkuSizeId.HasValue ? input.SkuSizeId.Value : 0,
        };

        var createdMockup = _dbContext.CoreSkuSizePkgMockups.Add(newMockup).Entity;
        await _dbContext.SaveChangesAsync();
        return Result<CreateSkuSizePkgMockupResultDto>.Success(
            new CreateSkuSizePkgMockupResultDto { Id = createdMockup.Id }
        );
    }

    public async Task<Result> Delete(int id)
    {
        _logger.LogInformation(
            "{MethodName}: {MockupId} = {MockupIdValue}",
            nameof(Delete),
            nameof(id),
            id
        );

        var mockup = await _dbContext.CoreSkuSizePkgMockups.FirstOrDefaultAsync(e => e.Id == id);
        if (mockup == null)
        {
            return Result.Failure(
                CoreErrorCode.CoreSkuSizePkgMockupNotFound,
                this.GetCurrentMethodInfo()
            );
        }

        _dbContext.CoreSkuSizePkgMockups.Remove(mockup);
        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<SkuSizePkgMockupDto>> Find(int id)
    {
        _logger.LogInformation(
            "{MethodName}: {MockupId} = {MockupIdValue}",
            nameof(Find),
            nameof(id),
            id
        );

        var mockup = await _dbContext.CoreSkuSizePkgMockups.FirstOrDefaultAsync(e => e.Id == id);
        if (mockup == null)
        {
            return Result<SkuSizePkgMockupDto>.Failure(
                CoreErrorCode.CoreSkuSizePkgMockupNotFound,
                this.GetCurrentMethodInfo()
            );
        }

        return Result<SkuSizePkgMockupDto>.Success(
            new SkuSizePkgMockupDto
            {
                Id = mockup.Id,
                MockupUrl = mockup.MockupUrl,
                SkuSizeId = mockup.SkuSizeId,
            }
        );
    }

    public async Task<Result<PagingResult<SkuSizePkgMockupDto>>> FindAll(PagingRequestBaseDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(FindAll),
            nameof(input),
            input
        );

        var listMockups = _dbContext
            .CoreSkuSizePkgMockups.Where(e =>
                string.IsNullOrEmpty(input.Keyword) || e.MockupUrl.Contains(input.Keyword.ToLower())
            )
            .Select(e => new SkuSizePkgMockupDto
            {
                Id = e.Id,
                MockupUrl = e.MockupUrl,
                SkuSizeId = e.SkuSizeId,
            });

        int totalItems = await listMockups.CountAsync();
        listMockups = listMockups.PagingAndSorting(input);

        return Result<PagingResult<SkuSizePkgMockupDto>>.Success(
            new PagingResult<SkuSizePkgMockupDto>
            {
                TotalItems = totalItems,
                Items = await listMockups.ToListAsync(),
            }
        );
    }

    public async Task<Result<UpdateSkuSizePkgMockupResultDto>> Update(
        UpdateSkuSizePkgMockupDto input
    )
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(Update),
            nameof(input),
            input
        );

        var mockup = await _dbContext.CoreSkuSizePkgMockups.FirstOrDefaultAsync(e =>
            e.Id == input.Id
        );
        if (mockup == null)
        {
            return Result<UpdateSkuSizePkgMockupResultDto>.Failure(
                CoreErrorCode.CoreSkuSizePkgMockupNotFound,
                this.GetCurrentMethodInfo()
            );
        }

        mockup.MockupUrl = input.MockupUrl;

        await _dbContext.SaveChangesAsync();

        return Result<UpdateSkuSizePkgMockupResultDto>.Success(
            new UpdateSkuSizePkgMockupResultDto { Id = mockup.Id }
        );
    }
}
