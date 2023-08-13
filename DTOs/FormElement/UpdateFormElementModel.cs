using FormGeneratorAPI.Entities;

namespace FormGeneratorAPI.DTOs.FormElement;

public class UpdateFormElementModel
{   public int Id { get; set; }
    public string Label { get; set; }
    public string Options { get; set; }
    public FormElementType Type { get; set; }
    public int FormId { get; set; }
}