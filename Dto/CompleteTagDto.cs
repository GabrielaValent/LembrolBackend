using System.ComponentModel.DataAnnotations;

namespace backend_lembrol.Dto;
public class CompleteTagDto : CompleteUpdateTagDto
{
    [Required]
    public string TagId { get; set; } = string.Empty;
}
