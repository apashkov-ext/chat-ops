namespace ChatOps.SharedKernel;

public static class OneOfExtensions
{
    public static Task SwitchAsync<T0, T1>(
        this OneOf.OneOf<T0, T1> value,
        Func<T0, Task> f0,
        Func<T1, Task> f1)
        => value.Match(f0, f1);
    
    public static Task SwitchAsync<T0, T1, T2>(
        this OneOf.OneOf<T0, T1, T2> value,
        Func<T0, Task> f0,
        Func<T1, Task> f1,
        Func<T2, Task> f2)
        => value.Match(f0, f1, f2);
    
    public static Task<TResult> MatchAsync<T0, T1, TResult>(
        this OneOf.OneOf<T0, T1> value,
        Func<T0, Task<TResult>> f0,
        Func<T1, Task<TResult>> f1)
        => value.Match(f0, f1);

    public static Task<TResult> MatchAsync<T0, T1, T2, TResult>(
        this OneOf.OneOf<T0, T1, T2> value,
        Func<T0, Task<TResult>> f0,
        Func<T1, Task<TResult>> f1,
        Func<T2, Task<TResult>> f2)
        => value.Match(f0, f1, f2);
}
