using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using App.Topics.Enumerators.T2_CustomZip;

namespace App.Tests.Topics.Enumerators.T2_CustomZip;

public class CustomZipTests
{
    [Test]
    public void ZipWithPadding_EqualLength()
    {
        var a = new[] { 1, 2, 3 };
        var b = new[] { "a", "b", "c" };

        var zipped = a.ZipWithPadding(b, 0, "_").ToList();

        Assert.That(zipped, Is.EqualTo(new (int, string)[]
        {
            (1, "a"), (2, "b"), (3, "c")
        }));
    }

    [Test]
    public void ZipWithPadding_LeftLonger_UsesRightPad()
    {
        var a = new[] { 1, 2, 3 };
        var b = new[] { "x" };

        var zipped = a.ZipWithPadding(b, -1, "PAD").ToList();

        Assert.That(zipped, Is.EqualTo(new (int, string)[]
        {
            (1, "x"), (2, "PAD"), (3, "PAD")
        }));
    }

    [Test]
    public void ZipWithPadding_RightLonger_UsesLeftPad()
    {
        var a = Array.Empty<int>();
        var b = new[] { "u", "v" };

        var zipped = a.ZipWithPadding(b, 0, string.Empty).ToList();

        Assert.That(zipped, Is.EqualTo(new (int, string)[]
        {
            (0, "u"), (0, "v")
        }));
    }

    [Test]
    public void ZipWithPadding_DeferredExecution()
    {
        int leftEnumerations = 0;
        int rightEnumerations = 0;

        IEnumerable<int> Left()
        {
            leftEnumerations++;
            yield return 1;
            yield return 2;
        }

        IEnumerable<string> Right()
        {
            rightEnumerations++;
            yield return "a";
        }

        var query = Left().ZipWithPadding(Right(), 0, "_");

        // До итерации побочных эффектов быть не должно
        Assert.That(leftEnumerations, Is.EqualTo(0));
        Assert.That(rightEnumerations, Is.EqualTo(0));

        var first = query.First();
        Assert.That(first, Is.EqualTo((1, "a")));
        Assert.That(leftEnumerations, Is.EqualTo(1));
        Assert.That(rightEnumerations, Is.EqualTo(1));

        var all = query.ToList();
        Assert.That(all, Is.EqualTo(new (int, string)[] { (1, "a"), (2, "_") }));
    }

    [Test]
    public void ZipWithPadding_NullArgs_Throws()
    {
        IEnumerable<int> a = null!;
        var b = new[] { 1 };
        Assert.Throws<ArgumentNullException>(() => _ = a.ZipWithPadding(b, 0, 0).ToList());
        Assert.Throws<ArgumentNullException>(() => _ = new[] { 1 }.ZipWithPadding<int, int>(null!, 0, 0).ToList());
    }
}
