using CR.ApplicationBase.Common;
using CR.Constants.ErrorCodes;
using CR.Core.ApplicationServices.Common;
using CR.Core.ApplicationServices.PartnerModule.Abstracts;
using CR.Core.Domain.Partner;
using CR.Core.Dtos.PartnerModule.Partner;
using CR.Core.Dtos.PartnerModule.PartnerType;
using CR.DtoBase;
using CR.Utils.DataUtils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CR.Core.ApplicationServices.PartnerModule.Implements
{
    public class PartnerService : CoreServiceBase, IPartnerService
    {
        public PartnerService(
            ILogger<PartnerService> logger,
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContext
        )
            : base(logger, httpContext) { }

        public async Task<Result<CreatePartnerResultDto>> CreatePartner(CreatePartnerDto input)
        {
            _logger.LogInformation(
                "{MethodName}: {InputName} = {@InputValue}",
                nameof(CreatePartner),
                nameof(input),
                input
            );
            if (_dbContext.CorePartners.Any(e => e.Name == input.Name))
            {
                return Result<CreatePartnerResultDto>.Failure(
                    CoreErrorCode.CorePartnerNameHasBeenUsed,
                    this.GetCurrentMethodInfo()
                );
            }
            if (!_dbContext.CorePartnerTypes.Any(e => e.Id == input.PartnerTypeId))
            {
                return Result<CreatePartnerResultDto>.Failure(
                    CoreErrorCode.CorePartnerTypeDoesNotExist,
                    this.GetCurrentMethodInfo()
                );
            }
            var partner = new CorePartner
            {
                Name = input.Name,
                PartnerTypeId = input.PartnerTypeId,
            };
            var createPartner = _dbContext.CorePartners.Add(partner).Entity;
            await _dbContext.SaveChangesAsync();
            return Result<CreatePartnerResultDto>.Success(
                new CreatePartnerResultDto { Id = createPartner.Id }
            );
        }

        public async Task<Result> Delete(int id)
        {
            _logger.LogInformation("{MethodName}: id = {Id}", nameof(Delete), id);
            var partner = await _dbContext.CorePartners.FirstOrDefaultAsync(b => b.Id == id);
            if (partner == null)
            {
                return Result.Failure(
                    CoreErrorCode.CorePartnerNotFound,
                    this.GetCurrentMethodInfo()
                );
            }
            _dbContext.CorePartners.Remove(partner);
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<PagingResult<PartnerDto>>> FindAll(FilterPartnerDto input)
        {
            _logger.LogInformation(
                "{MethodName}: {InputName} = {@InputValue}",
                nameof(FindAll),
                nameof(input),
                input
            );

            var listParner = _dbContext
                .CorePartners.Include(o => o.PartnerType)
                .Where(e =>
                    (
                        string.IsNullOrEmpty(input.Keyword)
                        || e.Name.ToLower().Contains(input.Keyword.ToLower())
                    )
                    && (
                        !input.PartnerTypeId.HasValue
                        || e.PartnerTypeId == input.PartnerTypeId.Value
                    )
                )
                .Select(e => new PartnerDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    PartnerTypeId = e.PartnerTypeId,
                    NamePartnerType = e.PartnerType.Name,
                    CreatedDate = e.CreatedDate,
                });

            int totalItems = await listParner.CountAsync();

            var items = await listParner.PagingAndSorting(input).ToListAsync();

            return Result<PagingResult<PartnerDto>>.Success(
                new PagingResult<PartnerDto> { TotalItems = totalItems, Items = items }
            );
        }

        public async Task<Result<PartnerDto>> GetById(int id)
        {
            _logger.LogInformation("{MethodName}: id = {Id}", nameof(GetById), id);
            if (!await _dbContext.CorePartners.AnyAsync(u => u.Id == id))
            {
                return Result<PartnerDto>.Failure(
                    CoreErrorCode.CorePartnerNotFound,
                    this.GetCurrentMethodInfo()
                );
            }
            var partner = await _dbContext
                .CorePartners.Include(o => o.PartnerType)
                .Where(u => u.Id == id)
                .Select(u => new PartnerDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    PartnerTypeId = u.PartnerTypeId,
                    NamePartnerType = u.PartnerType.Name,
                    CreatedDate = u.CreatedDate,
                })
                .FirstOrDefaultAsync();
            return Result<PartnerDto>.Success(partner);
        }

        public async Task<Result<UpdatePartnerResultDto>> Update(UpdatePartnerDto input)
        {
            _logger.LogInformation("{MethodName}: input = {@Input}", nameof(Update), input);
            var partner = await _dbContext.CorePartners.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (partner == null)
            {
                return Result<UpdatePartnerResultDto>.Failure(
                    CoreErrorCode.CorePartnerNotFound,
                    this.GetCurrentMethodInfo()
                );
            }
            partner.Name = input.Name;
            partner.PartnerTypeId = input.PartnerTypeId;
            await _dbContext.SaveChangesAsync();
            return Result<UpdatePartnerResultDto>.Success(
                new UpdatePartnerResultDto { Id = partner.Id }
            );
        }
    }
}
