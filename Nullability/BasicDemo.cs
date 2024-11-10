
using System.Diagnostics.Contracts;
using System.Text.Json.Serialization;
using NUnit.Framework;

namespace Nullability;

public class Person
{
    public Person(string firstName, string lastName, string? middleName)
    {

        FirstName = Preconditions.CheckNotNull(firstName);
        LastName = Preconditions.CheckNotNull(lastName);
        MiddleName = middleName;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
}

public class PersonDemo
{
    [Test]
    public void Constructor_FirstNameMustNotBeNull()
    {
        Assert.Throws<ArgumentNullException>(() => new Person(null!, "Doe", "Smith"));
    }
}
internal class BasicDemo
{
    public static void Run()
    {
        var person = new Person("John", "Doe", "Smith");
        var person2 = new Person("Reeta", "Acharya", null);
        PrintNameLengths(person);
        PrintNameLengths(person2);
    }
    private static void PrintNameLengths(Person person)
    {
        string first = person.FirstName;
        string last = person.LastName;
        string? middle = person.MiddleName;

        if (middle is null)
        {
            Console.WriteLine("First={0}, Last={1}",
                first.Length, last.Length);
        }
        else
        {
            Console.WriteLine("First={0}, Last={1}, Middle={2}",
                first.Length, last.Length, middle.Length);
        }
        
    }
}