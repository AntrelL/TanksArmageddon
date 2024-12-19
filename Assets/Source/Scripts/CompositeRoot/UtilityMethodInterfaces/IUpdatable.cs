namespace TanksArmageddon.CompositeRoot
{
    public interface IUpdatable : IActivatableGameObject, IEnableableComponent, IDestroyable
    {
        public void CompositeUpdate();
    }
}
