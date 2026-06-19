namespace OMS.Presentation.Controllers.OrderModule.Contracts
{
    public record CreateOrderApiRequest
    {
        public required string Name { get; init; }
    }
}
