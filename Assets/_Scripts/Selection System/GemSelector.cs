using System;
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
        GridSystem<GridObject<Gem>> grid;

        /// <summary>
        /// The input reader to get mouse input.
        /// </summary>
        InputReader inputReader;

        /// <summary>
        /// The game state machine to check the current game state.
        /// </summary>
        GameStateMachine gameStateMachine;

        /// <summary>
        /// The currently selected gem.
        /// </summary>
        [SerializeField] GridObject<Gem> selectedObject;

        /// <summary>
        /// The gem that the currently selected gem is being swapped with.
        /// </summary>
        [SerializeField] GridObject<Gem> objectToSwitch;
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
        public event Action<GridObject<Gem>, GridObject<Gem>> OnGemsSelected;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor that injects necessary dependencies.
        /// </summary>
        [Inject]
        void Construct(InputReader inputReader, GameStateMachine gameStateMachine)
        {
            this.gameStateMachine = gameStateMachine;
            this.inputReader = inputReader;
        }
        #endregion

        #region MonoBehaviour Callbacks
        private void OnEnable()
        {
            inputReader.OnClick += SelectGem;
            inputReader.OnRelease += DeselectGem;
        }

        private void LateUpdate()
        {
            if (CanSelectSwitchObject)
            {
                var obj = GetGemAtMousePosition();
                if (SelectionIsValid(obj))
                {
                    objectToSwitch = obj;
                    DeselectGem();
                }
            }
        }

        private void OnDisable()
        {
            inputReader.OnClick -= SelectGem;
            inputReader.OnRelease -= DeselectGem;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the gem at the mouse position.
        /// </summary>
        /// <returns>The gem at the mouse position or null if no gem is found.</returns>
        public GridObject<Gem> GetGemAtMousePosition()
        {
            Vector2Int selectedGridIndices = grid.GetXY(inputReader.MouseworldPosition);
            return grid.GetValue(selectedGridIndices.x, selectedGridIndices.y);
        }

        /// <summary>
        /// Checks if the selection is valid.
        /// </summary>
        /// <param name="obj">The gem to check.</param>
        /// <returns>True if the selection is valid, false otherwise.</returns>
        private bool SelectionIsValid(GridObject<Gem> obj) => obj != null && obj != objectToSwitch && obj != selectedObject;

        /// <summary>
        /// Deselects the currently selected gem and gem to switch with.
        /// </summary>
        private void DeselectGem()
        {
            OnGemsSelected?.Invoke(selectedObject, objectToSwitch);

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