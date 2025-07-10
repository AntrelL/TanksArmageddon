namespace Source.Scripts.Release.HitProcessing
{
    public interface IHealthImpactTarget : IImpactTarget
    {
        Health Health { get; } 
    }
}