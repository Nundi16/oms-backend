using OMS.Application.Modules.OrderModule.Models;

namespace OMS.Presentation.Controllers.OrderModule.Contracts
{
    public record CreateOrderApiRequest
    {
        public required CreateOrderRequest Order { get; init; }
    }
}
