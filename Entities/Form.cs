using FormGeneratorAPI.Authentication.Entities;

namespace FormGeneratorAPI.Entities;

public class Form
{
    public int Id { get; set; }
    public virtual User Author { get; set; }
    public string Title { get; set; }
    public DateTime EndTime { get; set; }
    
}