using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace OMS.Infrastructure.Options
{
    internal abstract class EntityStateActionOptions<TAction> where TAction : Delegate
    {
        private readonly Dictionary<EntityState, TAction> _actionsByState = new(Enum.GetValues<EntityState>().Length);
        internal bool IsConfigurationCompleted { get; private set; }
        internal IReadOnlyCollection<EntityState> RegisteredStates => _actionsByState.Keys;
        internal TAction GetAction(EntityState entityState) => _actionsByState[entityState];

        internal EntityStateActionOptions<TAction> RegisterAction(EntityState state, TAction action)
        {
            ArgumentNullException.ThrowIfNull(action);

            if (IsConfigurationCompleted)
            {
                throw new InvalidOperationException("Can not register new actions after configuration is completed.");
            }

            _actionsByState.Add(state, action);

            return this;
        }

        internal void Complete()
        {
            IsConfigurationCompleted = true;
        }
    }

    internal sealed class EntityStateActionOptions : EntityStateActionOptions<Action<EntityEntry, Guid, DateTime>>;
}
