using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Installers/UARPG/Local Component/No Movement")]
public class NoMovementInstaller : ScriptableObjectInstaller
{
    public override void InstallBindings()
    {
        base.InstallBindings();
        Container.Bind<IMovement>().To<NoMovement>().AsCached().NonLazy();
    }
}
