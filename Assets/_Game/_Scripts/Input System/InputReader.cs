using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Match3
{
    public class InputReader : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private bool isPressed;

        [SerializeField]
        private Vector2 mousePosition;
        Match3InputActions inputActions;
        InputAction clickAction;
        InputAction mousePositionAction;
        #endregion

        #region Properties
        public bool IsPressed => isPressed;
        public Vector2 MousePosition => mousePosition;
        public Vector3 MouseworldPosition => Camera.main.ScreenToWorldPoint(mousePosition);
        #endregion
        #region Actions
        public event Action OnClick;
        public event Action OnRelease;
        #endregion

        private void Awake()
        {
            inputActions = new();
            inputActions.Match3.Enable();
            clickAction = inputActions.Match3.Click;
            mousePositionAction = inputActions.Match3.MousePosition;
        }
        private void OnEnable()
        {
            clickAction.started += OnClickStarted;
            clickAction.canceled += OnClickCanceled;
            mousePositionAction.performed += OnMousePosition;
        }
        private void OnDestroy()
        {
            clickAction.performed -= OnClickStarted;
            clickAction.canceled -= OnClickCanceled;
            mousePositionAction.performed -= OnMousePosition;
        }
        private void OnMousePosition(InputAction.CallbackContext context)
        {
            mousePosition = context.ReadValue<Vector2>();
        }
        private void OnClickStarted(InputAction.CallbackContext context)
        {
            isPressed = true;
            OnClick?.Invoke();
        }
        private void OnClickCanceled(InputAction.CallbackContext context)
        {
            isPressed = false;
            OnRelease?.Invoke();
        }
    }
}
