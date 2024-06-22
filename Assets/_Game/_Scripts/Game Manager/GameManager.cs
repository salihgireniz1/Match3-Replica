using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class GameManager : MonoSingleton<GameManager>
    {
        GameStateMachine gameStateMachine;
        public bool IsRunning => gameStateMachine.CurrentState == gameStateMachine.Playing;
        private void Awake()
        {
            gameStateMachine = GetComponent<GameStateMachine>();
        }
        public void HoldGame()
        {
            gameStateMachine.ChangeState(gameStateMachine.OnHold);
        }
    }
}
