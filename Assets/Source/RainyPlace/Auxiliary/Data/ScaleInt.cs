namespace RainyPlace
{
    public class ScaleInt : Scale<int>
    {
        public ScaleInt(int value, int min, int max) : base(value, min, max) { }

        protected override bool IsInRange(int value, int min, int max) => value.IsInRange(min, max);
    }
}
