using CR.ApplicationBase.Common;
using CR.ApplicationBase.Localization;
using CR.Constants.ErrorCodes;
using CR.Core.ApplicationServices.Common;
using CR.Core.ApplicationServices.SkuModule.ProductionMethod.Abstacts;
using CR.Core.Domain.Sku;
using CR.Core.Dtos.SkuModule.ProductionMethod;
using CR.DtoBase;
using CR.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CR.Core.ApplicationServices.SkuModule.ProductionMethod.Implements;

public class ProductMethodService : CoreServiceBase, IProductMethodService
{
    public ProductMethodService(
        ILogger<ProductMethodService> logger,
        IHttpContextAccessor httpContext
    )
        : base(logger, httpContext) { }

    public async Task<Result<CreateProductionMethodResultDto>> Create(
        CreateProductionMethodDto input
    )
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(Create),
            nameof(input),
            input
        );
        if (_dbContext.CoreProductionMethods.Any(e => e.Code == input.Code))
        {
            return Result<CreateProductionMethodResultDto>.Failure(
                CoreErrorCode.CoreProductMethodCodeExisted,
                this.GetCurrentMethodInfo()
            );
        }
        var newMethod = new CoreProductionMethod { Name = input.Name, Code = input.Code };
        var createMethod = _dbContext.CoreProductionMethods.Add(newMethod).Entity;
        await _dbContext.SaveChangesAsync();
        return Result<CreateProductionMethodResultDto>.Success(
            new CreateProductionMethodResultDto { Id = createMethod.Id }
        );
    }

    public async Task<Result> Delete(int id)
    {
        _logger.LogInformation(
            "{MethodName}: {MethodId} = {MethodIdValue}",
            nameof(Delete),
            nameof(id),
            id
        );
        var method = await _dbContext.CoreProductionMethods.FirstOrDefaultAsync(e => e.Id == id);
        if (method == null)
        {
            return Result.Failure(
                CoreErrorCode.CoreProductMethodNotFound,
                this.GetCurrentMethodInfo()
            );
        }
        _dbContext.CoreProductionMethods.Remove(method);
        await _dbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<ProductionMethodDto>> Find(int id)
    {
        _logger.LogInformation(
            "{MethodName}: {MethodId} = {MethodIdValue}",
            nameof(Find),
            nameof(id),
            id
        );
        var method = await _dbContext.CoreProductionMethods.FirstOrDefaultAsync(e => e.Id == id);
        if (method == null)
        {
            return Result<ProductionMethodDto>.Failure(
                CoreErrorCode.CoreProductMethodNotFound,
                this.GetCurrentMethodInfo()
            );
        }
        return Result<ProductionMethodDto>.Success(
            new ProductionMethodDto { Name = method.Name, Code = method.Code }
        );
    }

    public async Task<Result<PagingResult<ProductionMethodDto>>> FindAll(PagingRequestBaseDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(FindAll),
            nameof(input),
            input
        );
        var listMethods = _dbContext
            .CoreProductionMethods.Where(e =>
                string.IsNullOrEmpty(input.Keyword) || e.Name.Contains(input.Keyword.ToLower())
            )
            .Select(e => new ProductionMethodDto
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
            });
        int totalItems = await listMethods.CountAsync();
        listMethods = listMethods.PagingAndSorting(input);
        return Result<PagingResult<ProductionMethodDto>>.Success(
            new PagingResult<ProductionMethodDto>
            {
                TotalItems = totalItems,
                Items = await listMethods.ToListAsync(),
            }
        );
    }

    public async Task<Result<UpdateProductionMethodResultDto>> Update(
        UpdateProductionMethodDto input
    )
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(Update),
            nameof(input),
            input
        );
        var method = await _dbContext.CoreProductionMethods.FirstOrDefaultAsync(e =>
            e.Id == input.Id
        );
        if (method == null)
        {
            return Result<UpdateProductionMethodResultDto>.Failure(
                CoreErrorCode.CoreProductMethodNotFound,
                this.GetCurrentMethodInfo()
            );
        }
        // Kiểm tra code có trùng với method khác không
        var isCodeDuplicate = await _dbContext.CoreProductionMethods.AnyAsync(e =>
            e.Code == input.Code && e.Id != input.Id // Trùng code nhưng khác Id
        );

        if (isCodeDuplicate)
        {
            return Result<UpdateProductionMethodResultDto>.Failure(
                CoreErrorCode.CoreProductMethodCodeExisted,
                this.GetCurrentMethodInfo()
            );
        }
        method.Name = input.Name;
        method.Code = input.Code;
        await _dbContext.SaveChangesAsync();
        return Result<UpdateProductionMethodResultDto>.Success(
            new UpdateProductionMethodResultDto { Id = method.Id }
        );
    }
}
