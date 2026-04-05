using CR.ApplicationBase.Common;
using CR.Constants.ErrorCodes;
using CR.Core.ApplicationServices.BrandModule.Abstracts;
using CR.Core.ApplicationServices.Common;
using CR.Core.Domain.Brand;
using CR.Core.Dtos.BrandModule.Brand;
using CR.Core.Dtos.BrandModule.Store;
using CR.DtoBase;
using CR.Utils.DataUtils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CR.Core.ApplicationServices.BrandModule.Implements
{
    public class StoreService : CoreServiceBase, IStoreService
    {
        public StoreService(
            ILogger<StoreService> logger,
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContext
        )
            : base(logger, httpContext) { }

        public async Task<Result<CreateStoreResultDto>> CreateStore(CreateStoreDto input)
        {
            _logger.LogInformation("{MethodName}: input = {@Input}", nameof(CreateStore), input);
            //Kiểm tra Store đã tồn tại chưa
            if (await _dbContext.CoreStores.AnyAsync(u => u.Name == input.Name))
            {
                return Result<CreateStoreResultDto>.Failure(
                    CoreErrorCode.CoreStoreNameHasBeenUsed,
                    this.GetCurrentMethodInfo()
                );
            }
            if (!await _dbContext.CoreBrands.AnyAsync(u => u.Id == input.BrandId))
            {
                return Result<CreateStoreResultDto>.Failure(
                    CoreErrorCode.CoreBrandDoesNotExist,
                    this.GetCurrentMethodInfo()
                );
            }
            var store = _dbContext
                .CoreStores.Add(new CoreStore { Name = input.Name, BrandId = input.BrandId })
                .Entity;
            await _dbContext.SaveChangesAsync();
            return Result<CreateStoreResultDto>.Success(new CreateStoreResultDto { Id = store.Id });
        }

        public async Task<Result> Delete(int id)
        {
            _logger.LogInformation("{MethodName}: id = {Id}", nameof(Delete), id);
            var store = await _dbContext.CoreStores.FirstOrDefaultAsync(b => b.Id == id);
            if (store == null)
            {
                return Result.Failure(CoreErrorCode.CoreStoreNotFound, this.GetCurrentMethodInfo());
            }

            _dbContext.CoreStores.Remove(store);
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<PagingResult<StoreDto>>> FindAll(PagingRequestBaseDto input)
        {
            _logger.LogInformation(
                "{MethodName}: {InputName} = {@InputValue}",
                nameof(FindAll),
                nameof(input),
                input
            );

            var listStores = _dbContext
                .CoreStores.Include(a => a.Brand)
                .Where(e =>
                    string.IsNullOrEmpty(input.Keyword) || e.Name.Contains(input.Keyword.ToLower())
                )
                .Select(e => new StoreDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    BrandId = e.BrandId,
                });

            int totalItems = await listStores.CountAsync();
            listStores = listStores.PagingAndSorting(input);

            return Result<PagingResult<StoreDto>>.Success(
                new PagingResult<StoreDto>
                {
                    TotalItems = totalItems,
                    Items = await listStores.ToListAsync(),
                }
            );
        }

        public async Task<Result<StoreDto>> GetById(int id)
        {
            _logger.LogInformation("{MethodName}: id = {Id}", nameof(GetById), id);
            if (!await _dbContext.CoreStores.AnyAsync(u => u.Id == id))
            {
                return Result<StoreDto>.Failure(
                    CoreErrorCode.CoreStoreNotFound,
                    this.GetCurrentMethodInfo()
                );
            }
            var store = await _dbContext
                .CoreStores.Include(a => a.Brand)
                .Where(u => u.Id == id)
                .Select(u => new StoreDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    BrandId = u.BrandId,
                })
                .FirstOrDefaultAsync();
            return Result<StoreDto>.Success(store);
        }

        public async Task<Result<UpdateStoreResultDto>> Update(UpdateStoreDto input)
        {
            _logger.LogInformation("{MethodName}: input = {@Input}", nameof(Update), input);
            var store = await _dbContext.CoreStores.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (store == null)
            {
                return Result<UpdateStoreResultDto>.Failure(
                    CoreErrorCode.CoreStoreNotFound,
                    this.GetCurrentMethodInfo()
                );
            }
            store.Name = input.Name;
            await _dbContext.SaveChangesAsync();
            return Result<UpdateStoreResultDto>.Success(new UpdateStoreResultDto { Id = store.Id });
        }
    }
}
