using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Match3
{
    /// <summary>
    /// This class is responsible for selecting gems in the game.
    /// </summary>
    public class GemSelector : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// The grid system that manages the game board.
        /// </summary>
        [Inject]
        GridSystem<GridObject<BaseGem>> grid;

        /// <summary>
        /// Injected instance of SelectionProvider.
        /// </summary>
        [Inject]
        SelectionProvider selectionProvider;

        /// <summary>
        /// The input reader to get mouse input.
        /// </summary>
        [Inject]
        InputReader inputReader;

        /// <summary>
        /// The game state machine to check the current game state.
        /// </summary>
        [Inject]
        GameStateMachine gameStateMachine;

        /// <summary>
        /// The currently selected gem.
        /// </summary>
        [SerializeField] GridObject<BaseGem> selectedObject;

        /// <summary>
        /// The gem that the currently selected gem is being swapped with.
        /// </summary>
        [SerializeField] GridObject<BaseGem> objectToSwitch;
        #endregion

        #region Properties
        /// <summary>
        /// Checks if a gem can be selected and swapped with another gem.
        /// </summary>
        public bool CanSelectSwitchObject =>
            inputReader.IsPressed &&
            selectedObject != null &&
            gameStateMachine.CurrentState == gameStateMachine.Playing;
        #endregion

        #region Events
        /// <summary>
        /// Event that is invoked when two gems are selected to be swapped.
        /// </summary>
        public event Func<GridObject<BaseGem>, GridObject<BaseGem>, UniTask> OnGemsSelected;
        #endregion

        #region MonoBehaviour Callbacks
        private void OnEnable()
        {
            inputReader.OnClick += SelectGem;
            inputReader.OnRelease += HandleDeselectGem;
        }
        private void Update()
        {
            if (CanSelectSwitchObject)
            {
                var obj = GetGemAtMousePosition();
                if (SelectionIsValid(obj))
                {
                    gameStateMachine.ChangeState(gameStateMachine.OnHold);
                    objectToSwitch = obj;
                    DeselectGem().Forget();
                }
            }
        }

        private void OnDisable()
        {
            inputReader.OnClick -= SelectGem;
            inputReader.OnRelease -= HandleDeselectGem;
        }
        #endregion

        #region Methods
        // Synchronous method to handle the event
        private void HandleDeselectGem()
        {
            if (CanSelectSwitchObject) DeselectGem().Forget();
        }

        /// <summary>
        /// Gets the gem at the mouse position.
        /// </summary>
        /// <returns>The gem at the mouse position or null if no gem is found.</returns>
        public GridObject<BaseGem> GetGemAtMousePosition()
        {
            Vector2Int selectedGridIndices = grid.GetXY(inputReader.MouseworldPosition);
            return grid.GetValue(selectedGridIndices.x, selectedGridIndices.y);
        }

        /// <summary>
        /// Checks if the selection is valid.
        /// </summary>
        /// <param name="obj">The gem to check.</param>
        /// <returns>True if the selection is valid, false otherwise.</returns>
        private bool SelectionIsValid(GridObject<BaseGem> obj) => obj != null && obj != objectToSwitch && obj != selectedObject;

        /// <summary>
        /// Deselects the currently selected gem and gem to switch with.
        /// </summary>
        private async UniTaskVoid DeselectGem()
        {
            if (OnGemsSelected != null)
            {
                await OnGemsSelected.Invoke(selectedObject, objectToSwitch);
            }
            selectedObject = null;
            objectToSwitch = null;
        }

        /// <summary>
        /// Selects the gem at the mouse position.
        /// </summary>
        private void SelectGem()
        {
            selectedObject = GetGemAtMousePosition() ?? null;
        }
        #endregion
    }
}