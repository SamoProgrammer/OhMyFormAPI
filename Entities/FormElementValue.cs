using FormGeneratorAPI.Authentication.Entities;

namespace FormGeneratorAPI.Entities;

public class FormElementValue
{
    public int Id { get; set; }
    public virtual FormElement Element { get; set; }
    public string Value { get; set; }
    public virtual User AnsweredBy { get; set; }
    public DateTime AnsweredAt { get; set; } = DateTime.Now;
}