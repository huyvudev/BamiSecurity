using CR.ApplicationBase.Common;
using CR.ApplicationBase.Localization;
using CR.Constants.ErrorCodes;
using CR.Core.ApplicationServices.Common;
using CR.Core.ApplicationServices.SkuModule.SkuBase.Abstracts;
using CR.Core.Domain.Sku;
using CR.Core.Dtos.SkuModule.SkuBase;
using CR.DtoBase;
using CR.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class SkuBaseService : CoreServiceBase, ISkuBaseService
{
    public SkuBaseService(ILogger<SkuBaseService> logger, IHttpContextAccessor httpContext)
        : base(logger, httpContext) { }

    public async Task<Result<CreateSkuBaseResultDto>> Create(CreateSkuBaseDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(Create),
            nameof(input),
            input
        );
        if (_dbContext.CoreSkuBases.Any(e => e.Code == input.Code))
        {
            return Result<CreateSkuBaseResultDto>.Failure(
                CoreErrorCode.CoreSkuBaseCodeExisted,
                this.GetCurrentMethodInfo()
            );
        }
        var newSkuBase = new CoreSkuBase { Code = input.Code, Description = input.Description };
        var createdSkuBase = _dbContext.CoreSkuBases.Add(newSkuBase).Entity;
        await _dbContext.SaveChangesAsync();
        return Result<CreateSkuBaseResultDto>.Success(
            new CreateSkuBaseResultDto { Id = createdSkuBase.Id }
        );
    }

    public async Task<Result> Delete(int id)
    {
        _logger.LogInformation(
            "{MethodName}: {SkuBaseId} = {SkuBaseIdValue}",
            nameof(Delete),
            nameof(id),
            id
        );
        var skuBase = await _dbContext.CoreSkuBases.FirstOrDefaultAsync(e => e.Id == id);
        if (skuBase == null)
        {
            return Result.Failure(CoreErrorCode.CoreSkuBaseNotFound, this.GetCurrentMethodInfo());
        }
        _dbContext.CoreSkuBases.Remove(skuBase);
        await _dbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<SkuBaseDto>> Find(int id)
    {
        _logger.LogInformation(
            "{MethodName}: {SkuBaseId} = {SkuBaseIdValue}",
            nameof(Find),
            nameof(id),
            id
        );
        var skuBase = await _dbContext.CoreSkuBases.FirstOrDefaultAsync(e => e.Id == id);
        if (skuBase == null)
        {
            return Result<SkuBaseDto>.Failure(
                CoreErrorCode.CoreSkuBaseNotFound,
                this.GetCurrentMethodInfo()
            );
        }
        return Result<SkuBaseDto>.Success(
            new SkuBaseDto
            {
                Id = skuBase.Id,
                Code = skuBase.Code,
                Description = skuBase.Description,
            }
        );
    }

    public async Task<Result<PagingResult<SkuBaseDto>>> FindAll(PagingRequestBaseDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(FindAll),
            nameof(input),
            input
        );
        var listSkuBases = _dbContext
            .CoreSkuBases.Where(e =>
                string.IsNullOrEmpty(input.Keyword) || e.Code.Contains(input.Keyword.ToLower())
            )
            .Select(e => new SkuBaseDto
            {
                Id = e.Id,
                Code = e.Code,
                Description = e.Description,
            });
        int totalItems = await listSkuBases.CountAsync();
        listSkuBases = listSkuBases.PagingAndSorting(input);
        return Result<PagingResult<SkuBaseDto>>.Success(
            new PagingResult<SkuBaseDto>
            {
                TotalItems = totalItems,
                Items = await listSkuBases.ToListAsync(),
            }
        );
    }

    public async Task<Result<UpdateSkuBaseResultDto>> Update(UpdateSkuBaseDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(Update),
            nameof(input),
            input
        );
        var skuBase = await _dbContext.CoreSkuBases.FirstOrDefaultAsync(e => e.Id == input.Id);
        if (skuBase == null)
        {
            return Result<UpdateSkuBaseResultDto>.Failure(
                CoreErrorCode.CoreSkuBaseNotFound,
                this.GetCurrentMethodInfo()
            );
        }
        var isCodeDuplicate = await _dbContext.CoreSkuBases.AnyAsync(e =>
            e.Code == input.Code && e.Id != input.Id
        );

        if (isCodeDuplicate)
        {
            return Result<UpdateSkuBaseResultDto>.Failure(
                CoreErrorCode.CoreSkuBaseCodeExisted,
                this.GetCurrentMethodInfo()
            );
        }
        skuBase.Code = input.Code;
        skuBase.Description = input.Description;
        await _dbContext.SaveChangesAsync();
        return Result<UpdateSkuBaseResultDto>.Success(
            new UpdateSkuBaseResultDto { Id = skuBase.Id }
        );
    }
}
