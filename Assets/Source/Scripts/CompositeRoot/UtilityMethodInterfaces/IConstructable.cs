namespace TanksArmageddon.CompositeRoot
{
    public interface IConstructable<T>
    {
        public void Construct(T parameters);
    }

    public interface IConstructable
    {
        public void Construct();
    }
}
