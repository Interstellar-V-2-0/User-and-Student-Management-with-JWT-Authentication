namespace UserStudentMgmt.Domain.Entities;

public class User 
{
    public int Id { get; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    
    public int RoleId { get; set; }
    public Role Role { get; set; }
}