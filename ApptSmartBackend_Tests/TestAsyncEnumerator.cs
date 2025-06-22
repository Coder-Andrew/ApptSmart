using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

// Used Claude 3.7 Sonnet and ChatGpt 4o in order to get these classes for mocking
public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner) =>
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));

    public T Current => _inner.Current;

    public ValueTask DisposeAsync()
    {
        _inner.Dispose();
        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());
}

public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    public TestAsyncQueryProvider(IQueryProvider inner) =>
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));

    public IQueryable CreateQuery(Expression expression)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        return new TestAsyncEnumerable<TEntity>(expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        return new TestAsyncEnumerable<TElement>(expression);
    }

    public object? Execute(Expression expression)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        return _inner.Execute(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        return _inner.Execute<TResult>(expression);
    }

    public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        return new TestAsyncEnumerable<TResult>(expression);
    }

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        Type resultType = typeof(TResult);

        // Handle Task<T> return type
        if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            Type innerType = resultType.GetGenericArguments()[0];

            MethodInfo? executeMethod = typeof(IQueryProvider)
                .GetMethod(nameof(IQueryProvider.Execute))
                ?.MakeGenericMethod(innerType);

            if (executeMethod == null)
                throw new InvalidOperationException("Could not find Execute method on IQueryProvider.");

            object? innerResult = executeMethod.Invoke(_inner, new object[] { expression });

            MethodInfo? fromResultMethod = typeof(Task)
                .GetMethod(nameof(Task.FromResult))
                ?.MakeGenericMethod(innerType);

            if (fromResultMethod == null)
                throw new InvalidOperationException("Could not find FromResult method on Task.");

            return (TResult)fromResultMethod.Invoke(null, new[] { innerResult })!;
        }

        // Handle ValueTask<T> return type
        if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(ValueTask<>))
        {
            Type innerType = resultType.GetGenericArguments()[0];

            MethodInfo? executeMethod = typeof(IQueryProvider)
                .GetMethod(nameof(IQueryProvider.Execute))
                ?.MakeGenericMethod(innerType);

            if (executeMethod == null)
                throw new InvalidOperationException("Could not find Execute method on IQueryProvider.");

            object? innerResult = executeMethod.Invoke(_inner, new object[] { expression });

            MethodInfo? fromResultMethod = typeof(Task)
                .GetMethod(nameof(Task.FromResult))
                ?.MakeGenericMethod(innerType);

            if (fromResultMethod == null)
                throw new InvalidOperationException("Could not find FromResult method on Task.");

            Task? task = (Task?)fromResultMethod.Invoke(null, new[] { innerResult });

            if (task == null)
                throw new InvalidOperationException("Task.FromResult returned null.");

            return (TResult)Activator.CreateInstance(
                typeof(ValueTask<>).MakeGenericType(innerType),
                new[] { task })!;
        }

        // Direct execution for non-Task types
        return _inner.Execute<TResult>(expression);
    }
}

public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable ?? throw new ArgumentNullException(nameof(enumerable))) { }

    public TestAsyncEnumerable(Expression expression) : base(expression ?? throw new ArgumentNullException(nameof(expression))) { }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
        new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
}