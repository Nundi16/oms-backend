using OMS.Common.Interfaces.Communication;
using OMS.Common.Interfaces.Entity;

namespace OMS.Common.Interfaces.Extensions
{
    public interface IExtension : IEntity<Guid>
    {
        string TypeDescriptor { get; }
        Guid? SourceId { get; }
        IExtension[]? ChildExtensions { get; set; }

        void AssignSourceId(Guid sourceId);
        Task<IResult> DispatchCreationAsync(IMediator m, CancellationToken ct = default);
    }
}
