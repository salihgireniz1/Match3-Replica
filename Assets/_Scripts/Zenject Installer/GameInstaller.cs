using UnityEngine;
using Zenject;

namespace Match3
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        Match3Data gridData;

        [SerializeField]
        GemData gemData;

        [SerializeField]
        SwitchAnimationData switchAnimationData;
        public override void InstallBindings()
        {
            Container.Bind<GridGenerator>().AsSingle().NonLazy();
            Container.Bind<InputReader>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<GameStateMachine>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<GemSelector>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GemProvider<Gem>>().To<InstantiateGem>().AsSingle().NonLazy();
            Container.BindInstance(gridData);
            Container.BindInstance(gemData);
            Container.BindInstance(switchAnimationData);
            Container.Bind<GridSystem<GridObject<Gem>>>().FromMethod(x => GridSystem<GridObject<Gem>>.CreateVerticalInstance(gridData)).AsSingle().NonLazy();
        }
    }
}
