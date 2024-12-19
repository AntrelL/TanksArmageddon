namespace TanksArmageddon.CompositeRoot
{
    public interface IFixedUpdatable : IActivatableGameObject, IEnableableComponent, IDestroyable
    {
        public void CompositeFixedUpdate();
    }
}
