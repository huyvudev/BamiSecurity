using CR.Core.Dtos.OrderModule.OrderDetail;
using CR.DtoBase;

namespace CR.Core.ApplicationServices.OrderModule.Abstracts;

public interface IOrderDetailService
{
    Task<Result<PagingResult<OrderDetailDoneDto>>> FindAllOrderDetailDone(
        PagingRequestBaseDto input
    );
    Task<Result<PagingResult<OrderDetailDoneDto>>> FindAllOrderDetail(
        FilterOrderDetailDto input
    );
    Task<Result<OrderDetailDoneDto>> FindByIdDetailDone(int id);
    Task<Result<UpdateOrderDetailResultDto>> UpdateOrderDetail(UpdateOrderDetailBaseDto input);
    Task<Result<UpdateOrderDetailResultDto>> UpdateDetailDone(
        UpdateBasicInformationForOrderDetailDto input
    );
    Task<Result<ApproveOrderDetailResultDto>> ApproveOrderDetail(int id);
    Task<Result> Delete(int id);
    Task<Result<CreateOrderDetailResultDto>> Create(AddOrderDetailDto input);
}
