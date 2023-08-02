using FormGeneratorAPI.Entities;

namespace FormGeneratorAPI.DTOs.FormElement;

public class UpdateFormElementModel
{
    public int Id { get; set; }
    public string Label { get; set; }
    public List<string> Options { get; set; }
    public int FormId { get; set; }
    public FormElementType Type { get; set; }
}