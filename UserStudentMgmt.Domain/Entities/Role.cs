namespace UserStudentMgmt.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public List<User> Users = new List<User>();
}