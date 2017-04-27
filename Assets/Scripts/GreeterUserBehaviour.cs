using UnityEngine;
using Zenject;

public class GreeterUserBehaviour: MonoBehaviour
{
    [SerializeField]
    private string greeting = "Hello world!";
    [Inject]
    private Greeter greeter;

    private void Start ()
    {
        greeter.Greet (greeting);
    }
}