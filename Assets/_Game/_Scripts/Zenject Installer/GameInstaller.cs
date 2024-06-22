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
            Container.Bind<GridSystem<GridObject<BaseGem>>>()
                .FromMethod(x => GridSystem<GridObject<BaseGem>>.CreateVerticalInstance(gridData))
                .AsSingle().NonLazy();
            Container.Bind<GameStateMachine>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<GemProvider<BaseGem>>().To<InstantiateGem>().AsSingle().NonLazy();
            Container.Bind<InputReader>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<GemSelector>().FromComponentInHierarchy().AsSingle();
            Container.Bind<SelectionProvider>().AsSingle().NonLazy();
            Container.Bind<InGridMovement>().AsSingle().NonLazy();
            Container.Bind<GridGenerator>().AsSingle().NonLazy();
            Container.BindInstance(switchAnimationData);
            Container.BindInstance(gridData);
            Container.BindInstance(gemData);

            // Selection Processes
            Container.Bind<SimpleToSimpleSelection>().AsSingle().NonLazy();
        }
    }
}
