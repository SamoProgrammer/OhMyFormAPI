namespace FormGeneratorAPI.DTOs.Form;

public class AddFormModel
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string Title { get; set; }
    public DateTime EndTime { get; set; }

}