using FormGeneratorAPI.Entities;

namespace FormGeneratorAPI.DTOs.FormElement;

public class UpdateFormElementValueModel
{
    public int ElementId { get; set; }
    public string Value { get; set; }
    public int AnsweredById { get; set; }
}