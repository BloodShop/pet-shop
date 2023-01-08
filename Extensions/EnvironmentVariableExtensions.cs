namespace PetShopProj.Extensions
{
    public static class EnvironmentVariableExtensions
    {
        public static string GetConnectionStringFromEnvironment(this IConfiguration configuration, string environmentName = "") =>
            string.IsNullOrEmpty(environmentName) ?
                configuration[$"ConnectionStrings:DefaultConnection"] :
                configuration[$"ConnectionStrings:{environmentName}"];
    }
}
