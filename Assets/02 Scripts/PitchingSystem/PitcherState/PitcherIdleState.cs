public class PitcherIdleState : IPitcherState
{
    public void Enter(PitcherManager pitcher)
    {

        pitcher.CurrentPitchState = PitchState.Ready;
        pitcher.animator.ResetTrigger("Ready");
        pitcher.animator.ResetTrigger("WindUp");
    }

    /*public void Exit(PitcherManager pitcher)
    {
    }

    public void Update(PitcherManager pitcher)
    {
    }*/


}
