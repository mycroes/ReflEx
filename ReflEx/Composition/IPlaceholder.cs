namespace ReflEx.Composition
{
    public interface IPlaceholder<out T>
    {
        T Value { get; }
    }

    public interface IPlaceholder<in TIn, out TOut>
    {
        TOut Invoke(TIn @in);
    }
}