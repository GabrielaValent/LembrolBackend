public class TagRepositoryFactory
{
    private readonly IServiceProvider _serviceProvider;

    public TagRepositoryFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IServiceScope CreateScope()
    {
        return _serviceProvider.CreateScope();
    }
}
