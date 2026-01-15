namespace ChatOps.App.Core;

/// <summary>
/// Объект-значение (Value Object). <br/>
/// Упрощенная реализация Value Object из библиотеки https://github.com/vkhorikov/CSharpFunctionalExtensions<br/>
/// Value Object - один из тактических паттернов подхода DDD (Domain Driven Design).<br/>
/// Важные свойства Value Object: <br/>
/// - немутабельность: состояние объекта невозможно изменить законным путем;<br/>
/// - сравнение по значению: сравнивается содержимое VO. Какое именно содержимое должно сравниваться, определяет разработчик.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Возвращает компоненты (члены) сравниваемого объекта, которые должны участвовать в сравнении.
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        var valueObject = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(
                1,
                (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                }
            );
    }

    public static bool operator ==(ValueObject? a, ValueObject? b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
        {
            return true;
        }

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject? a, ValueObject? b)
    {
        return !(a == b);
    }

    public override string ToString() => GetStringRepresentation();
    
    /// <summary>
    /// Возвращает строковое представление объекта. <br/>
    /// Можект использоваться для отладки или упрощенного отображения содержимого объекта.
    /// </summary>
    /// <returns>Строковое представление объекта.</returns>
    protected abstract string GetStringRepresentation();
}