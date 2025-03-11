using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Installers/UARPG/Local Component/Direct Movement")]
public class DirectMovementLocalInstaller : CharacterControllerLocalInstaller
{
    public override void InstallBindings()
    {
        base.InstallBindings();
        Container.Bind<IMovement>().To<DirectMovement>().AsCached().NonLazy();
    }
}
