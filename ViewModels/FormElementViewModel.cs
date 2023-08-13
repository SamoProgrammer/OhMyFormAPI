using System.ComponentModel.DataAnnotations.Schema;

namespace FormGeneratorAPI.Entities;

public class FormElementViewModel
{
    public int Id { get; set; }
    public string Label { get; set; }
    public string Options { get; set; } = "";
    public int FormId { get; set; }
    public FormElementType Type { get; set; }

}