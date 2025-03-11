using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Installers/UARPG/Local Component/Character Controller")]
public class CharacterControllerLocalInstaller : ScriptableObjectInstaller
{
    public override void InstallBindings() => Container.Bind<CharacterController>().FromComponentInHierarchy().AsCached().NonLazy();
}
