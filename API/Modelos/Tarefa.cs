using System.ComponentModel.DataAnnotations;

namespace API.Modelos;

public class Tarefa
{

    public int Id { get; set; }

    [Required]
    public int StatusId { get; set; }

    [Required]
    public string  ? Titulo { get; set; }
    
    [Required]
    [DataType(DataType.Date)]
    public DateTime DataVencimento { get; set; }

    public Status ? Status { get; set; }

}