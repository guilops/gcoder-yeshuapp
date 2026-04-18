public static class ServiceCollectionExtensions
{
    public static void AddCustomHttpClients(this IServiceCollection services, string baseUrl)
    {
        var baseAddress = new Uri(baseUrl);

        services.AddHttpClient<AutenticacaoServices>(client =>
        {
            client.BaseAddress = baseAddress;
        });

        services.AddHttpClient<CaminhoesServices>(client =>
        {
            client.BaseAddress = baseAddress;
        });

        services.AddHttpClient<MontadorasServices>(client =>
        {
            client.BaseAddress = baseAddress;
        });
    }
}