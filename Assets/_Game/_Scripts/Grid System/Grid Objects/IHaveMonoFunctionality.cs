using Cysharp.Threading.Tasks;

namespace Match3
{
    /// <summary>
    /// A interface that holds special abilities like exploding.
    /// </summary>
    public interface IHaveMonoFunctionality
    {
        UniTask MonoFunction();
    }
}