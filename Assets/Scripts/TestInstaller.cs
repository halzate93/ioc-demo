using Zenject;

public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Greeter>().AsSingle();
		// Container.Bind<IInitializable> ().To<GreeterUser> ().AsSingle ()
		//	.WithArguments ("Hello world IInitializable");
		// Container.Bind<string> ().FromInstance ("Hello world IInitializable")
		//	.WhenInjectedInto <GreeterUserOnce> ();
		Container.BindInterfacesTo<GreeterUser> ().AsSingle ().WithArguments ("Hello world Inject");
    }
}