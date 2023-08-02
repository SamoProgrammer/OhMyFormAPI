namespace FormGeneratorAPI.Entities;

public class FormElement
{
    public int Id { get; set; }
    public string Label { get; set; }
    public List<string> Options { get; set; }
    public virtual Form Form { get; set; }
    public FormElementType Type { get; set; }
    
}