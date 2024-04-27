
public abstract class CameraState
{
    protected CameraController controller;

    public CameraState(CameraController controller)
    {
        this.controller = controller;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
}
