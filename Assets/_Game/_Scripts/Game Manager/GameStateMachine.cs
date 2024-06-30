
using UnityEngine;

public class GameStateMachine : BaseStateMachine
{
    public IState Playing;
    public IState OnHold;

    public override void ChangeState(IState newState)
    {
        Debug.Log($"GameState: {newState.ToString()}");
        base.ChangeState(newState);
    }

    protected override void GenerateStates()
    {
        this.Playing = new Playing(this);
        this.OnHold = new OnHold(this);
        this.defaultState = Playing;
    }
}
