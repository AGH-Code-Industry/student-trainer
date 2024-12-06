using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<DialogueService>().AsSingle();
        Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
        Container.BindInterfacesAndSelfTo<SceneService>().AsSingle();
        Container.BindInterfacesAndSelfTo<ResourceReader>().AsSingle();

        Container.BindInterfacesAndSelfTo<DayNightCycleService>().AsSingle();
        Container.BindInterfacesAndSelfTo<CameraService>().AsSingle();
        Container.BindInterfacesAndSelfTo<BattleService>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerMovementService>().AsSingle();
    }

}