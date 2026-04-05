using CR.ApplicationBase.Common;
using CR.Constants.ErrorCodes;
using CR.Core.ApplicationServices.Common;
using CR.Core.ApplicationServices.PartnerModule.Abstracts;
using CR.Core.Domain.Partner;
using CR.Core.Dtos.BrandModule.Store;
using CR.Core.Dtos.PartnerModule.PartnerType;
using CR.DtoBase;
using CR.Utils.DataUtils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CR.Core.ApplicationServices.PartnerModule.Implements
{
    public class PartnerTypeService : CoreServiceBase, IPartnerTypeService
    {
        public PartnerTypeService(
            ILogger<PartnerTypeService> logger,
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContext
        )
            : base(logger, httpContext) { }

        public async Task<Result<CreatePartnerTypeResultDto>> CreatePartnerType(
            CreatePartnerTypeDto input
        )
        {
            _logger.LogInformation(
                "{MethodName}: {InputName} = {@InputValue}",
                nameof(CreatePartnerType),
                nameof(input),
                input
            );
            if (_dbContext.CorePartnerTypes.Any(e => e.Name == input.Name))
            {
                return Result<CreatePartnerTypeResultDto>.Failure(
                    CoreErrorCode.CorePartnerTypeNameHasBeenUsed,
                    this.GetCurrentMethodInfo()
                );
            }
            var type = new CorePartnerType { Name = input.Name };
            var createPartnerType = _dbContext.CorePartnerTypes.Add(type).Entity;
            await _dbContext.SaveChangesAsync();
            return Result<CreatePartnerTypeResultDto>.Success(
                new CreatePartnerTypeResultDto { Id = createPartnerType.Id }
            );
        }

        public async Task<Result> Delete(int id)
        {
            _logger.LogInformation("{MethodName}: id = {Id}", nameof(Delete), id);
            var type = await _dbContext.CorePartnerTypes.FirstOrDefaultAsync(b => b.Id == id);
            if (type == null)
            {
                return Result.Failure(
                    CoreErrorCode.CorePartnerTypeNotFound,
                    this.GetCurrentMethodInfo()
                );
            }
            _dbContext.CorePartnerTypes.Remove(type);
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<PagingResult<PartnerTypeDto>>> FindAll(PagingRequestBaseDto input)
        {
            _logger.LogInformation(
                "{MethodName}: {InputName} = {@InputValue}",
                nameof(FindAll),
                nameof(input),
                input
            );

            var listPartnerTypes = _dbContext
                .CorePartnerTypes.Where(e =>
                    string.IsNullOrEmpty(input.Keyword) || e.Name.Contains(input.Keyword.ToLower())
                )
                .Select(e => new PartnerTypeDto { Id = e.Id, Name = e.Name });

            int totalItems = await listPartnerTypes.CountAsync();
            listPartnerTypes = listPartnerTypes.PagingAndSorting(input);

            return Result<PagingResult<PartnerTypeDto>>.Success(
                new PagingResult<PartnerTypeDto>
                {
                    TotalItems = totalItems,
                    Items = await listPartnerTypes.ToListAsync(),
                }
            );
        }

        public async Task<Result<PartnerTypeDto>> GetById(int id)
        {
            _logger.LogInformation("{MethodName}: id = {Id}", nameof(GetById), id);
            if (!await _dbContext.CorePartnerTypes.AnyAsync(u => u.Id == id))
            {
                return Result<PartnerTypeDto>.Failure(
                    CoreErrorCode.CorePartnerTypeNotFound,
                    this.GetCurrentMethodInfo()
                );
            }
            var type = await _dbContext
                .CorePartnerTypes.Where(u => u.Id == id)
                .Select(u => new PartnerTypeDto { Id = u.Id, Name = u.Name })
                .FirstOrDefaultAsync();
            return Result<PartnerTypeDto>.Success(type);
        }

        public async Task<Result<UpdatePartnerTypeResultDto>> Update(UpdatePartnerTypeDto input)
        {
            _logger.LogInformation("{MethodName}: input = {@Input}", nameof(Update), input);
            var type = await _dbContext.CorePartnerTypes.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (type == null)
            {
                return Result<UpdatePartnerTypeResultDto>.Failure(
                    CoreErrorCode.CorePartnerTypeNotFound,
                    this.GetCurrentMethodInfo()
                );
            }
            type.Name = input.Name;
            await _dbContext.SaveChangesAsync();
            return Result<UpdatePartnerTypeResultDto>.Success(
                new UpdatePartnerTypeResultDto { Id = type.Id }
            );
        }
    }
}
