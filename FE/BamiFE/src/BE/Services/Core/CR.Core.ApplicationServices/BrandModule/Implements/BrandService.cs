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
    public class BrandService : CoreServiceBase, IBrandService
    {
        public BrandService(
            ILogger<BrandService> logger,
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContext
        )
            : base(logger, httpContext) { }

        public async Task<Result<CreateBrandResultDto>> CreateBrand(CreateBrandDto input)
        {
            _logger.LogInformation("{MethodName}: input = {@Input}", nameof(CreateBrand), input);
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            //Kiểm tra Brand đã tồn tại chưa
            if (await _dbContext.CoreBrands.AnyAsync(u => u.Name == input.Name))
            {
                return Result<CreateBrandResultDto>.Failure(
                    CoreErrorCode.CoreBrandNameHasBeenUsed,
                    this.GetCurrentMethodInfo()
                );
            }
            var brand = _dbContext.CoreBrands.Add(new CoreBrand { Name = input.Name }).Entity;
            await _dbContext.SaveChangesAsync();
            foreach (var store in input.Stores)
            {
                _dbContext.CoreStores.Add(new CoreStore { Name = store.Name, BrandId = brand.Id });
            }
            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
            return Result<CreateBrandResultDto>.Success(new CreateBrandResultDto { Id = brand.Id });
        }

        public async Task<Result> Delete(int id)
        {
            _logger.LogInformation("{MethodName}: id = {Id}", nameof(Delete), id);
            var brand = await _dbContext.CoreBrands.Include(x=>x.Stores).FirstOrDefaultAsync(b => b.Id == id);
            if (brand == null)
            {
                return Result.Failure(CoreErrorCode.CoreBrandNotFound, this.GetCurrentMethodInfo());
            }
            _dbContext.CoreStores.RemoveRange(brand.Stores);
            _dbContext.CoreBrands.Remove(brand);
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<PagingResult<BrandDto>>> FindAll(PagingRequestBaseDto input)
        {
            _logger.LogInformation(
                "{MethodName}: {InputName} = {@InputValue}",
                nameof(FindAll),
                nameof(input),
                input
            );

            var listBrands = _dbContext
                .CoreBrands.Include(e => e.Stores)
                .Where(e =>
                    string.IsNullOrEmpty(input.Keyword) || e.Name.Contains(input.Keyword.ToLower())
                )
                .Select(e => new BrandDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Stores = e.Stores.Select(d => new StoreDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        BrandId = d.BrandId,
                    }),
                });

            int totalItems = await listBrands.CountAsync();
            listBrands = listBrands.PagingAndSorting(input);

            return Result<PagingResult<BrandDto>>.Success(
                new PagingResult<BrandDto>
                {
                    TotalItems = totalItems,
                    Items = await listBrands.ToListAsync(),
                }
            );
        }

        public async Task<Result<BrandDto>> GetById(int id)
        {
            _logger.LogInformation("{MethodName}: id = {Id}", nameof(GetById), id);
            if (!await _dbContext.CoreBrands.AnyAsync(u => u.Id == id))
            {
                return Result<BrandDto>.Failure(
                    CoreErrorCode.CoreBrandNotFound,
                    this.GetCurrentMethodInfo()
                );
            }
            var brand = await _dbContext
                .CoreBrands.Include(o=>o.Stores).Where(u => u.Id == id)
                .Select(u => new BrandDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Stores = u.Stores.Select(d => new StoreDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        BrandId = d.BrandId,
                    }),
                })
                .FirstOrDefaultAsync();
            return Result<BrandDto>.Success(brand);
        }

        public async Task<Result<UpdateBrandResultDto>> Update(UpdateBrandDto input)
        {
            _logger.LogInformation("{MethodName}: input = {@Input}", nameof(Update), input);
            var brand = await _dbContext.CoreBrands.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (brand == null)
            {
                return Result<UpdateBrandResultDto>.Failure(
                    CoreErrorCode.CoreBrandNotFound,
                    this.GetCurrentMethodInfo()
                );
            }
            brand.Name = input.Name;
            await _dbContext.SaveChangesAsync();
            return Result<UpdateBrandResultDto>.Success(new UpdateBrandResultDto { Id = brand.Id });
        }
    }
}
