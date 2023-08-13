using FormGeneratorAPI.Authentication.Entities;

namespace FormGeneratorAPI.Entities;

public class FormElementValueViewModel
{
    public int Id { get; set; }
    public int ElementId { get; set; }
    public string Value { get; set; }
    public int AnsweredById { get; set; }
    public DateTime AnsweredAt { get; set; } = DateTime.Now;
}