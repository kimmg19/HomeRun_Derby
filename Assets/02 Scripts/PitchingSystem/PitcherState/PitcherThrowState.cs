public class PitcherThrowState : IPitcherState
{
    public void Enter(PitcherManager pitcher)
    {
        pitcher.CurrentPitchState = PitchState.Throw;

        //Debug.Log("≈ı±∏");
        pitcher.ExecutePitch();
    }

    /*public void Update(PitcherManager pitcher)
    {

    }

    public void Exit(PitcherManager pitcher)
    {        

    }*/
}
