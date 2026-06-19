namespace OMS.Presentation.Models.Pagination
{
    public sealed record PaginationResponse : PaginationBase
    {
        public uint ItemCount { get; init; }
    }
}
