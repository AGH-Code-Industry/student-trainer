public class LevelChangeProgress
{
    public string changePart;
    public float changeProgress;
    public float overallProgress;

    public LevelChangeProgress(string changePart, float changeProgress, float overallProgress)
    {
        this.changePart = changePart;
        this.changeProgress = changeProgress;
        this.overallProgress = overallProgress;
    }
}
