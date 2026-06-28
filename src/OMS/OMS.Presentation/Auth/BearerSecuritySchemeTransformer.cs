using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace OMS.Presentation.Auth
{
    public sealed class BearerSecuritySchemeTransformer
        : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(
            OpenApiDocument document,
            OpenApiDocumentTransformerContext context,
            CancellationToken cancellationToken)
        {
            if (document.Components == null)
            {
                document.Components = new OpenApiComponents();
            }

            document.Components.SecuritySchemes ??=
                new Dictionary<string, IOpenApiSecurityScheme>();

            document.Components.SecuritySchemes["Bearer"] =
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                };

            return Task.CompletedTask;

        }

    }
}
