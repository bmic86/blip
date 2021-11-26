namespace Chip.Random
{
    internal class RandomGenerator : IRandomGenerator
    {
        private readonly System.Random _random = new();

        int IRandomGenerator.Generate() => _random.Next(byte.MaxValue + 1);
    }
}
