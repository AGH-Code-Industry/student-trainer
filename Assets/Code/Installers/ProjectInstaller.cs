using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<DialogueService>().AsSingle();
        Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
        Container.BindInterfacesAndSelfTo<SceneService>().AsSingle();
    }

}