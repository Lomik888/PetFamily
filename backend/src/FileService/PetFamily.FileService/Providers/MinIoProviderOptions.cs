namespace PetFamily.FileService.Providers;

public sealed class MinIoProviderOptions
{
    public const string SECTION_NAME = "MinIO";
    public const string ACCESSKEY_NAME = "AccessKey";
    public const string SECRETKEY_NAME = "SecretKey";
    public const string ENDPOINT_NAME = "Endpoint";
    public const string WITH_SSL = "SSL";
    public const string REQUEST_LIMIT = "REQUEST_LIMIT";

    public string MinIO { get; init; }
    public string AccessKey { get; init; }
    public string SecretKey { get; init; }
    public string Endpoint { get; init; }
    public bool SSL { get; init; }
    public int RequestLimit { get; init; }
}