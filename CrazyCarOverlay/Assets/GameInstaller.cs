using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private InputHandler inputHandler;

    public override void InstallBindings()
    {
        Container
            .Bind<IInputHandler>()
            .FromInstance(inputHandler)
            .AsSingle();
    }
}