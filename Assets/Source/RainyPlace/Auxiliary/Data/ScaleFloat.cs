namespace RainyPlace
{
    public class ScaleFloat : Scale<float>
    {
        public ScaleFloat(float value, float min, float max) : base(value, min, max) { }

        protected override bool IsInRange(float value, float min, float max) => value.IsInRange(min, max);
    }
}
