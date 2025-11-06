namespace UserStudentMgmt.Domain.Entities;

public class DocumentType
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<Student> Students { get; set; } = new List<Student>();

    public List<Director> Directors { get; set; } = new List<Director>();
}