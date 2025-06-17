using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<DialogueService>().AsSingle();
        //Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
        Container.BindInterfacesAndSelfTo<InputService>().FromNewComponentOnNewGameObject().WithGameObjectName("InputServiceInstance").AsSingle();
        Container.BindInterfacesAndSelfTo<EventBus>().AsSingle();
        Container.BindInterfacesAndSelfTo<SceneService>().AsSingle();
        //Container.BindInterfacesAndSelfTo<LevelService>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelService>().FromNewComponentOnNewGameObject().WithGameObjectName("LevelServiceInstance").AsSingle();
        Container.BindInterfacesAndSelfTo<ResourceReader>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameService>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelSerializer>().AsSingle();

        Container.BindInterfacesAndSelfTo<DayNightCycleService>().AsSingle();
        Container.BindInterfacesAndSelfTo<CameraService>().AsSingle();
        Container.BindInterfacesAndSelfTo<BattleService>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerService>().AsSingle();

        Container.BindInterfacesAndSelfTo<InventoryService>().AsSingle();
        Container.BindInterfacesAndSelfTo<ItemUsingService>().AsSingle();

        Container.BindInterfacesAndSelfTo<QuestService>().AsSingle();
    }

}