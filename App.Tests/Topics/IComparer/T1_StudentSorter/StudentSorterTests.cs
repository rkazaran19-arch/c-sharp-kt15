using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using App.Topics.IComparer.T1_StudentSorter;

namespace App.Tests.Topics.IComparer.T1_StudentSorter;

public class StudentSorterTests
{
    [Test]
    public void Sort_Empty_ReturnsEmpty()
    {
        var input = new List<Student>();
        var sorted = StudentSorter.Sort(input).ToList();
        Assert.That(sorted, Is.Empty);
    }

    [Test]
    public void Sort_Basic_MultiCriteria()
    {
        var list = new List<Student>
        {
            new("Petrov", 20, 3.2),
            new("Ivanov", 22, 4.0),
            new("Ivanov", 19, 4.5),
            new("Sidorov", 21, 3.5),
            new("ivanov", 20, 3.9),
        };

        var res = StudentSorter.Sort(list).ToList();

        var expected = new List<Student>
        {
            new("Ivanov", 22, 4.0),
            new("ivanov", 20, 3.9),
            new("Ivanov", 19, 4.5),
            new("Petrov", 20, 3.2),
            new("Sidorov", 21, 3.5),
        };

        Assert.That(
            res,
            Is.EqualTo(expected).Using<Student>((a, b) =>
                string.Equals(a.LastName, b.LastName, StringComparison.OrdinalIgnoreCase) &&
                a.Age == b.Age &&
                Math.Abs(a.Gpa - b.Gpa) < 1e-9 ? 0 : -1)
        );
    }

    [Test]
    public void Sort_CaseInsensitive_LastName()
    {
        var list = new List<Student>
        {
            new("a", 20, 1.0),
            new("B", 20, 1.0),
            new("A", 20, 1.0),
            new("b", 20, 1.0),
        };
        var res = StudentSorter.Sort(list).ToList();
        var lastNames = res.Select(s => s.LastName.ToLowerInvariant()).ToList();
        Assert.That(lastNames, Is.EqualTo(new[] { "a", "a", "b", "b" }));
    }

    [Test]
    public void Sort_Ties_ByGpaDesc_WhenAgeEqual()
    {
        var list = new List<Student>
        {
            new("Ivanov", 20, 3.0),
            new("Ivanov", 20, 4.0),
            new("Ivanov", 20, 3.5),
        };
        var res = StudentSorter.Sort(list).ToList();
        var gpas = res.Select(s => s.Gpa).ToArray();
        Assert.That(gpas, Is.EqualTo(new[] { 4.0, 3.5, 3.0 }));
    }
}
