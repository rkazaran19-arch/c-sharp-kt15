namespace App.Topics.IComparer.T1_StudentSorter;

public record Student(string LastName, int Age, double Gpa);

public class StudentMultiComparer : IComparer<Student>
{
    int IComparer<Student>.Compare(Student? x, Student? y)
    {
        if (x is null && y is null)
            return 0;
        if (x is null)
            return -1;
        if (y is null)
            return 1;

        int LN = StringComparer.OrdinalIgnoreCase.Compare(x.LastName, y.LastName);
        if (LN != 0)
            return LN;

        int newage = y.Age.CompareTo(x.Age);
        if (newage != 0)
            return newage;

        return y.Gpa.CompareTo(x.Gpa);
    }
}

public static class StudentSorter
{
    public static IEnumerable<Student> Sort(IEnumerable<Student> students)
    {
        if (students == null)
            return Enumerable.Empty<Student>();

        return students.OrderBy(s => s.LastName, StringComparer.OrdinalIgnoreCase).ThenByDescending(s => s.Age).ThenByDescending(s => s.Gpa);
    }
}

