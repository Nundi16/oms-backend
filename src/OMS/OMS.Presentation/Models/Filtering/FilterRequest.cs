using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OMS.Domain;

namespace OMS.Presentation.Models.Filtering
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class FromQueryFilterAttribute : ModelBinderAttribute<FilterExpressionBinder>
    {
        public override BindingSource? BindingSource => BindingSource.Query;
    }

    // To return available filter for the user based on his roles/claims
    // Entities must have an attribute if it can be filtered with the given role
    // For returning these, it must have an API
    // These filter rules are collected on startup, then the endpoint returns the relevant filters based on the user's roles
    public class FilterExpressionBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Get jwt from auth header
            var token = bindingContext.HttpContext.Request.Headers.Authorization;
            // Get roles from jwt claims

            // Get the filter query string

            // Iterate roles, check if role is equal to entity name in filter, if not then decline request or thow exception
            throw new NotImplementedException();
        }
    }

    public record FilterRule
    {
        public required string Entity { get; init; }
        public required string Field { get; init; }
        public required string Operator { get; init; }
        public required object Value { get; init; }
    }
}
