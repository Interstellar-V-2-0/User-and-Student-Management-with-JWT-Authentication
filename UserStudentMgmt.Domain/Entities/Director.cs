namespace UserStudentMgmt.Domain.Entities;

public class Director : Person
{
    public int Id { get; set; }
    
    public int DocumentTypeId { get; set; }
    public DocumentType? DocumentType { get; set; }
    
}