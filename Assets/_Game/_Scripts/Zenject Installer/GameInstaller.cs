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
        }

        void BindSelectionProcessResponsibles()
        {
            Container.Bind<SelectionProvider>().AsSingle().NonLazy();
            Container.Bind<IGridMovement>().To<AnimatedMover>().AsSingle().NonLazy();
        }
        void BindGridResponsibles()
        {
            Container.Bind<IGemControls>().To<AnimatedGemController>().AsSingle();

            Container.Bind<IGridControls>().To<InstantiateGridWithoutAnimation>().AsSingle();

            // Gem provider(i.e., pooling, instantiating etc.)
            Container.Bind<GemProvider<BaseGem>>().To<InstantiateGem>().AsSingle().NonLazy();

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
            Container.Bind<ChainProvider>().AsSingle().NonLazy();

            Container.Bind<ChainBase>().To<LinearPentaChain>().AsTransient().NonLazy();
            Container.Bind<ChainBase>().To<LinearTripleChain>().AsTransient().NonLazy();
            // TODO: Add other type of chain seekers.
        }
    }
}