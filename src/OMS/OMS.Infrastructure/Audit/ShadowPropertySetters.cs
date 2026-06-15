namespace OMS.Infrastructure.Audit
{
    internal static class ShadowPropertySetters
    {
        internal static CreationShadowPropertySetter Creation => new();
        internal static CreationShadowPropertySetter Modification => new();
        internal static CreationShadowPropertySetter Deletion => new();
    }
}
