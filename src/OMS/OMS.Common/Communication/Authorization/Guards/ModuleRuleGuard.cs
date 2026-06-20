using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication.Authorization.Guards;

namespace OMS.Common.Communication.Authorization.Guards
{
	/// <summary>
	/// Per-handler role guard. A handler constructs this guard with the set of roles
	/// the current user must hold for the handler to run. If the principal does not
	/// carry any of the required roles, <see cref="Authorize"/> returns
	/// <see cref="Result.Failure(string)"/> and <see cref="AuthorizingMediator"/>
	/// silently skips the handler (today).
	/// <para>
	/// Behaviour matrix on the mediator side:
	/// <list type="bullet">
	///   <item><term>FanOutAsync / FanOutSequentialAsync</term><description>handler is filtered out (soft skip).</description></item>
	///   <item><term>EmitAsync</term><description>completes without invoking the handler.</description></item>
	///   <item><term>RequestAsync</term><description>returns a failed <see cref="IResult{TResponse}"/>; the caller decides how to surface it.</description></item>
	/// </list>
	/// </para>
	/// <para>
	/// Long-term: a "mandatory" mode is planned so handlers that must run can surface
	/// a hard authorization error instead of being skipped. Until that lands, callers
	/// must treat skipped handlers as a deliberate no-op.
	/// </para>
	/// <example>
	/// <code>
	/// internal sealed class FooHandler(IUserContext userContext, IMediator mediator)
	///     : AuthorizedEventHandler&lt;FooEvent, ModuleRuleGuard&gt;(
	///         new ModuleRuleGuard(userContext, "Admin", "Clinic Admin"))
	/// {
	///     public override Task HandleAsync(FooEvent @event, CancellationToken ct = default)
	///         =&gt; ...;
	/// }
	/// </code>
	/// </example>
	/// </summary>
	public class ModuleRuleGuard : IAuthorizationGuard
	{
		private readonly IUserContext _userContext;
		private readonly HashSet<string> _requiredRoles;

		public ModuleRuleGuard(IUserContext userContext, params string[] requiredRoles)
			: this(userContext, (IEnumerable<string>)(requiredRoles ?? Array.Empty<string>()))
		{
		}

		public ModuleRuleGuard(IUserContext userContext, IEnumerable<string> requiredRoles)
		{
			ArgumentNullException.ThrowIfNull(userContext);
			ArgumentNullException.ThrowIfNull(requiredRoles);

			_userContext = userContext;
			_requiredRoles = new HashSet<string>(requiredRoles, StringComparer.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Roles that satisfy this guard. Exposed for diagnostics / tests; the set
		/// is intentionally read-only.
		/// </summary>
		public IReadOnlyCollection<string> RequiredRoles => _requiredRoles;

		public IResult Authorize()
		{
			// No rule attached -> guard is a no-op (handler always runs).
			if (_requiredRoles.Count == 0)
			{
				return Result.Success();
			}

			var principalRoles = _userContext.Claims.FindAll(Constants.Auth.ROLE_CLAIM_TYPE);

			foreach (var claim in principalRoles)
			{
				if (_requiredRoles.Contains(claim.Value))
				{
					return Result.Success();
				}
			}

			return Result.Failure($"Required role(s) missing: {string.Join(", ", _requiredRoles)}.");
		}
	}
}
