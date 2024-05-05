using System.ComponentModel.DataAnnotations;

namespace WebApplication3.DTOs;

public class newPrescriptionDTO
{
    [Required]
    public DateTime date { get; set; }
    [Required]
    public DateTime dueDate { get; set; }
    [Required]
    public int IdPatient { get; set; }
    [Required]
    public int IdDoctor { get; set; }
}