using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Installers/UARPG/Local Component/Character Animation")]
public class CharacterAnimationLocalInstaller : ScriptableObjectInstaller
{
    public override void InstallBindings()
    {
        Container.Rebind<Animator>().FromComponentInHierarchy().AsCached().NonLazy();
        Container.Rebind<AnimationStateControl>().AsCached().NonLazy();
    }
}
