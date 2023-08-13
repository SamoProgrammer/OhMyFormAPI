using FormGeneratorAPI.Authentication.Entities;

namespace FormGeneratorAPI.Entities;

public class FormViewModel
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string Title { get; set; }
    public DateTime EndTime { get; set; }

}