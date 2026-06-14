using OMS.Application.Modules.OrderModule.Models;

namespace OMS.Presentation.Controllers.OrderModule.Contracts
{
    public class CreateOrderRequest
    {
        public required CreateOrderRequestDto Order { get; init; }
    }
}
