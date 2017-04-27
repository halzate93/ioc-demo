using System;
using Zenject;

public class GreeterUser: IInitializable, ITickable
{
    private string greeting;
    private Greeter greeter;

    public GreeterUser (string greeting, Greeter greeter)
    {
        this.greeting = greeting;
        this.greeter = greeter;
    }

    [Inject]
    private void OnInject ()
    {
        greeter.Greet ("Awake " + greeting);
    }

    public void Initialize()
    {
        greeter.Greet ("Start " + greeting);
    }

    public void Tick()
    {
        greeter.Greet ("Update " + greeting);
    }
}
