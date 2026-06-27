using System.ComponentModel.DataAnnotations.Schema;
using OMS.Common;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;
using OMS.Common.Interfaces.Extensions;
using OMS.Common.Models;
using OMS.Domain.Abstractions.Events;
using OMS.Domain.Interfaces.Events;

namespace OMS.Domain.Extensions
{

    public abstract class Extension<TSelf, TSource> : Entity, IExtension
        where TSelf : Extension<TSelf, TSource> // 4Robi: https://en.wikipedia.org/wiki/Curiously_recurring_template_pattern
        where TSource : Entity
    {
        public abstract string TypeDescriptor { get; }
        public Guid? SourceId { get; set; }
        public TSource? Source { get; set; }
        [NotMapped]
        public IExtension[]? ChildExtensions { get; set; }

        public void AssignSourceId(Guid sourceId) => SourceId = sourceId;
        public async Task<IResult> DispatchCreationAsync(IMediator m, CancellationToken ct = default)
        {
            var response = await m.RequestAsync<ICreationDomainEvent<TSelf>, ServiceResponse<TSelf>>(
                new CreationDomainEvent<TSelf>((TSelf)(object)this, Connectors: null, PersistChanges: false) { Extensions = ChildExtensions }, ct);

            return response.Succeeded
                ? Result.Success()
                : Result.Failure(response.ErrorMessage ?? "Extension creation failed.");
        }

        public async Task<IResult> DispatchModificationAsync(IMediator m, CancellationToken ct = default)
        {
            var response = await m.RequestAsync<IModificationDomainEvent<TSelf>, ServiceResponse<TSelf>>(
                new ModificationDomainEvent<TSelf>((TSelf)(object)this, Connectors: null, PersistChanges: false) { Extensions = ChildExtensions }, ct);

            return response.Succeeded
                ? Result.Success()
                : Result.Failure(response.ErrorMessage ?? "Extension modification failed.");
        }

        public Task<IResult> DispatchDeletionAsync(IMediator m, CancellationToken ct = default) =>
            m.EmitAsync<IDeletionDomainEvent<TSelf>>(new DeletionDomainEvent<TSelf>((TSelf)(object)this, Connectors: null, PersistChanges: false) { Extensions = ChildExtensions }, ct);
    }
}
