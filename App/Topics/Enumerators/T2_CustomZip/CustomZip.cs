namespace App.Topics.Enumerators.T2_CustomZip;

public static class EnumerableEx
{
    public static IEnumerable<(TFirst First, TSecond Second)> ZipWithPadding<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, TFirst firstPad, TSecond secondPad)
    {
        if (first == null) throw new ArgumentNullException(nameof(first));
        if (second == null) throw new ArgumentNullException(nameof(second));

        return ZipWithPaddingC(first, second, firstPad, secondPad);
    }

    public static IEnumerable<(TFirst First, TSecond Second)> ZipWithPaddingC<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, TFirst firstPad, TSecond secondPad)
    {
        using var enumerator1 = first.GetEnumerator();
        using var enumerator2 = second.GetEnumerator();

        while (true)
        {
            var hasFirst1 = enumerator1.MoveNext();
            var hasFirst2 = enumerator2.MoveNext();

            if (!hasFirst1 && !hasFirst2)
            {
                break;
            }

            yield return (
                hasFirst1 ? enumerator1.Current : firstPad,
                hasFirst2 ? enumerator2.Current : secondPad
            );
        }
    }
}