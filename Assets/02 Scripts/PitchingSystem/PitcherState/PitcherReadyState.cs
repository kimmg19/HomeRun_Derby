public class PitcherReadyState : IPitcherState
{
    public void Enter(PitcherManager pitcher)
    {
        //Debug.Log("투구 대기");
        pitcher.animator.SetTrigger("Ready");
    }

    /*public void Update(PitcherManager pitcher)
    {
        
    }

    public void Exit(PitcherManager pitcher)
    {
        
    }   */
}
