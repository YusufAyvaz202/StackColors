using Player;
using UI;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerBonusController>().FromComponentInHierarchy().AsSingle().NonLazy();
        
        // Bind the WinLoseUI as a singleton
        Container.Bind<WinLoseUI>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<BonusUI>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}
