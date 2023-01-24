namespace PetShopProj.Configuration
{
    public interface IServiceInstaller
    {
        void Install(IServiceCollection services, IConfiguration configuration, string environmentName = "");
    }
}