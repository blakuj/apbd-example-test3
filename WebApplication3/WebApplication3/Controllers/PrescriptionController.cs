using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTOs;
using WebApplication3.Repositories;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionController : ControllerBase
{
    private readonly PrescriptionRepository _prescriptionRepository;

    public PrescriptionController(PrescriptionRepository prescriptionRepository)
    {
        _prescriptionRepository = prescriptionRepository;
    }


    [HttpGet]
   
    public async Task<IActionResult> getPrescription([FromQuery] string? lastName )
    {
        object? prescription;
        if (lastName==null)
        {
            prescription = await _prescriptionRepository.getPrescription();
        }
        else
        {
            if (!await _prescriptionRepository.doesDoctorExist(lastName))
            {
                return NotFound($"Doctor {lastName} doeas not exist");
            }
            prescription = await _prescriptionRepository.getPrescriptionFiltered(lastName);
        }
        
        return Ok(prescription);
    }


    [HttpPost]
    public async Task<IActionResult> addPrecription([FromBody] newPrescriptionDTO newPrescription)
    {

        if (newPrescription.dueDate <= newPrescription.date)
        {
            return BadRequest("Due date is later than date");
        }
        
        
        _prescriptionRepository.addPrescription(newPrescription);
        return Ok();
    }
}