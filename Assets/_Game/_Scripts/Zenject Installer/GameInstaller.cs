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

        [SerializeField]
        FallAnimationData fallAnimationData;
        public override void InstallBindings()
        {
            // Selection Handlers...
            BindSelectionProcessResponsibles();
            // MonoBehaviours...
            BindMonoBehaviourClasses();
            // Chain Seeking Processes...
            BindChainSeekingProcess();
            // Selection Processes...
            BindSelectionProcesses();
            // Generate Grid...
            BindGridResponsibles();
            // Data Instances...
            BindDataInstances();
            // Gem movers...
            BindGemMovers();
        }

        void BindSelectionProcessResponsibles()
        {
            Container.Bind<SelectionProvider>().AsSingle().NonLazy();
            Container.Bind<IGridMovement>().To<AnimatedMover>().AsSingle().NonLazy();
        }
        void BindGridResponsibles()
        {
            Container.Bind<ISwipeGems>().To<SwipeWithTween>().AsSingle();

            // Grid filler(instant filler, animated filler etc.)
            Container.Bind<GridFiller>().To<InstantGridFiller>().AsSingle();

            Container.Bind<IGridControls>().To<InstantiateGridWithoutAnimation>().AsSingle();

            // Gem provider(i.e., pooling, instantiating etc.)
            Container.Bind<GemProvider<BaseGem>>().To<InstantiateGem>().AsSingle().NonLazy();

            // Grid object controller(creates, contains and clears).
            Container.Bind<GridObjectController>().AsSingle();

            // A vertical grid base.
            Container.Bind<GridSystem<GridObject<BaseGem>>>()
                            .FromMethod(x => GridSystem<GridObject<BaseGem>>.CreateVerticalInstance(gridData))
                            .AsSingle().NonLazy();
        }
        void BindSelectionProcesses()
        {
            Container.Bind<Selection>().To<SingleSelection>().AsSingle().NonLazy();
            Container.Bind<Selection>().To<SimpleToSimpleSelection>().AsSingle().NonLazy();
            // TODO : Add other type of selections like simple to bomb etc.
        }
        void BindGemMovers()
        {
            Container.Bind<IGemMovement>().WithId(MoverType.Instant).To<InstantGemMover>().AsSingle();
            Container.Bind<IGemMovement>().WithId(MoverType.Swipe).To<TweenForSwipe>().AsSingle();
        }
        void BindDataInstances()
        {
            Container.BindInstance(gridData);
            Container.BindInstance(gemData);
            Container.BindInstance(fallAnimationData).NonLazy();
            Container.BindInstance(switchAnimationData).NonLazy();
        }
        void BindMonoBehaviourClasses()
        {
            Container.Bind<GameStateMachine>().FromComponentInHierarchy().AsSingle();
            Container.Bind<InputReader>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<GemSelector>().FromComponentInHierarchy().AsSingle();
        }
        void BindChainSeekingProcess()
        {
            Container.Bind<ChainProvider>().AsSingle();

            Container.Bind<ChainBase>().To<LinearPentaChain>().AsTransient();
            Container.Bind<ChainBase>().To<LinearTripleChain>().AsTransient();
            // TODO: Add other type of chain seekers.
        }
    }
}