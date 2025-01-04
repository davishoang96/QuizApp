using Autofac;
using QuizApp.Database.Repositories;

namespace QuizApp.Database;

public class RepositoryModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.RegisterType<QuizRepository>().AsImplementedInterfaces().InstancePerDependency();
        builder.RegisterType<UserRepository>().AsImplementedInterfaces().InstancePerDependency();
    }
}
