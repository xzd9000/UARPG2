using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Installers/UARPG/Local Component/Manual Walk Move Control")]
public class ManualWalkMovementControlLocalInstaller : ScriptableObjectInstaller
{
    public override void InstallBindings() => Container.Bind<ManualMovementControl>().FromComponentInHierarchy().AsCached().NonLazy();
}
