using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CR.Core.ApplicationServices.OrderModule.Abstracts;

public interface IHandleOrderService
{
    /// <summary>
    /// Xử lý order khi sale upload lên
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task HandleOrder(int orderId);
}
