namespace PetFamily.Infrastructure.Providers.MinIo;

public sealed record MinIoProviderOptions
{
    public const string SECTION_NAME = "MinIO";
    public const string ACCESSKEY_NAME = "AccessKey";
    public const string SECRETKEY_NAME = "SecretKey";
    public const string ENDPOINT_NAME = "Endpoint";
}