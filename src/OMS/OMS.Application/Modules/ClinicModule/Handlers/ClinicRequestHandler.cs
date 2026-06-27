using OMS.Application.Connectors;
using OMS.Application.Handlers;
using OMS.Application.Interfaces.Persistation;
using OMS.Common.Interfaces.Communication;
using OMS.Domain.Modules.ClinicModule;

namespace OMS.Application.Modules.ClinicModule.Handlers
{
    internal sealed class ClinicRequestHandler(
        IMediator mediator,
        IDbContext context)
        : WriteHandler<Clinic>(mediator, context);
}
