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
    [Route("{lastName}")]
    public async Task<IActionResult> getPrescription(string lastName = "default")
    {
        if (!await _prescriptionRepository.doesDoctorExist(lastName) && lastName != "default")
        {
            return NotFound($"Doctor {lastName} doeas not exist");
        }

        var prescription = await _prescriptionRepository.getPrescription(lastName);
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