using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Constants.Core.Order;
using CR.DtoBase;

namespace CR.Core.Dtos.OrderModule.OrderDto;
public class FilterOrderDto : PagingRequestBaseDto
{

    public string? Address {  get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? OrderNumber {  get; set; }
    public OrderStatus? Status { get; set; } 
    public string? Tax { get; set; }
    public string? Phone {  get; set; }
    public string? Email { get; set; }
    public string? Address2 { get; set; }
    public string? Namespace { get; set; }
}
