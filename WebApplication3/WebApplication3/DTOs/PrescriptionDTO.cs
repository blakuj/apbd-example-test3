namespace WebApplication3.DTOs;

public class PrescriptionDTO
{
    public int idPrecsription { get; set; }
    public DateTime date { get; set; }
    public DateTime dueDate { get; set; }
    public string patientLastName { get; set; }
    public string? doctorLastName { get; set; }

}