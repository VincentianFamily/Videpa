namespace Videpa.Identity.Logic.Ports
{
    public interface IConfigurationManager
    {
        bool FakeIdentity { get; }
        bool ShowStackTraceInErrorResponse { get; }
        string JwtHeaderKey { get; }
    }
}