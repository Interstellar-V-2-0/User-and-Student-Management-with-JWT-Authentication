namespace UserStudentMgmt.Domain.Entities;

public abstract class Person
{
    public string Name {get; set;}
    public string LastName {get; set;}
    public string Document { get; set; }
    public string Email {get; set;}
    public string PhoneNumber {get; set;}
}