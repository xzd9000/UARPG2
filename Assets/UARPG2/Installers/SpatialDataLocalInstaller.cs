using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Installers/UARPG/Local Component/Spatial Data")]
public class SpatialDataLocalInstaller : ScriptableObjectInstaller
{
    public override void InstallBindings() => Container.Bind<ISpatialData>().FromComponentInHierarchy().AsCached().NonLazy();
}
