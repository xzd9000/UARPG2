using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Installers/UARPG/Local Component/Character")]
public class CharacterLocalInstaller : ScriptableObjectInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ICharacter>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<Animator>().FromInstance(null);
        Container.Bind<AnimationStateControl>().FromInstance(null);
    }
}
