using System.ComponentModel.DataAnnotations.Schema;

namespace FormGeneratorAPI.Entities;

public class FormElement
{
    public int Id { get; set; }
    public string Label { get; set; }
    public string Options { get; set; } = "";
    public virtual Form Form { get; set; }
    public FormElementType Type { get; set; }

}