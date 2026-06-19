namespace OMS.Presentation.Models.Pagination
{
    public abstract record PaginationBase
    {
        public uint PageNumber { get; init; }
        public uint PageSize { get; init; }
    }
}
