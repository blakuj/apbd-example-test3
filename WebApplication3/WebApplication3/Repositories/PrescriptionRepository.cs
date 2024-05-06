using Microsoft.Data.SqlClient;
using WebApplication3.DTOs;

namespace WebApplication3.Repositories;

public class PrescriptionRepository
{
    private readonly IConfiguration _configuration;

    public PrescriptionRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public async Task<bool> doesDoctorExist(string LastName)
    {
        var query = "Select 1 from Doctor where LastName = @LastName";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@LastName", LastName);

        await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();

        return result is not null;
    }

   
    

    public async Task<PrescriptionDTO[]> getPrescription()
    {
    
       var query = "SELECT P.IdPrescription as IDPrescription, P.Date as Date,P.DueDate as DueDate," +
                " Pat.LastName as Patient_Last_Name," +
                " D.LastName as Doctor_Last_Name" +
                " from Prescription P join Doctor D on P.IdDoctor = D.IdDoctor join Patient Pat on P.IdPatient = Pat.IdPatient " +
                " ORDER BY Date desc";
   

    List<PrescriptionDTO> prescriptions = new List<PrescriptionDTO>();

    await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));

    await using SqlCommand command = new SqlCommand();

    command.Connection = connection;
    command.CommandText = query;

    await connection.OpenAsync();

    var reader = await command.ExecuteReaderAsync();

    var preIdOrdinal = reader.GetOrdinal("IDPrescription");
    var preDateOrdinal = reader.GetOrdinal("Date");
    var preDueDateOrdinal = reader.GetOrdinal("DueDate");
    var patientLastNameOrdinal = reader.GetOrdinal("Patient_Last_Name");
    var doctorLastNameOrdinal = reader.GetOrdinal("Doctor_Last_Name");

    while (await reader.ReadAsync())
    {
        PrescriptionDTO prescriptionDto = new PrescriptionDTO()
        {
            idPrecsription = reader.GetInt32(preIdOrdinal),
            date = reader.GetDateTime(preDateOrdinal),
            dueDate = reader.GetDateTime(preDueDateOrdinal),
            patientLastName = reader.GetString(patientLastNameOrdinal),
            doctorLastName = reader.GetString(doctorLastNameOrdinal)
        };
        
        prescriptions.Add(prescriptionDto);
    }

    return prescriptions.ToArray();
}


    public async Task<PrescriptionDTO[]> getPrescriptionFiltered(string docLastName)
    {
   
       var query = "SELECT P.IdPrescription as IDPrescription, P.Date as Date,P.DueDate as DueDate," +
                " Pat.LastName as Patient_Last_Name," +
                " D.LastName as Doctor_Last_Name" +
                " from Prescription P join Doctor D on P.IdDoctor = D.IdDoctor join Patient Pat on P.IdPatient = Pat.IdPatient " +
                " WHERE D.LastName = @docName" +
                " ORDER BY Date desc";
    

    List<PrescriptionDTO> prescriptions = new List<PrescriptionDTO>();

    await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));

    await using SqlCommand command = new SqlCommand();

    command.Connection = connection;
    command.CommandText = query;
    command.Parameters.AddWithValue("@docName", docLastName);

    await connection.OpenAsync();

    var reader = await command.ExecuteReaderAsync();

    var preIdOrdinal = reader.GetOrdinal("IDPrescription");
    var preDateOrdinal = reader.GetOrdinal("Date");
    var preDueDateOrdinal = reader.GetOrdinal("DueDate");
    var patientLastNameOrdinal = reader.GetOrdinal("Patient_Last_Name");
    var doctorLastNameOrdinal = reader.GetOrdinal("Doctor_Last_Name");

    while (await reader.ReadAsync())
    {
        PrescriptionDTO prescriptionDto = new PrescriptionDTO()
        {
            idPrecsription = reader.GetInt32(preIdOrdinal),
            date = reader.GetDateTime(preDateOrdinal),
            dueDate = reader.GetDateTime(preDueDateOrdinal),
            patientLastName = reader.GetString(patientLastNameOrdinal),
            doctorLastName = reader.GetString(doctorLastNameOrdinal)
        };
        
        prescriptions.Add(prescriptionDto);
    }

    return prescriptions.ToArray();
}
    
    
    public async Task addPrescription(newPrescriptionDTO newPrescriptionDto)
    {
        var query = "Insert into Prescription values (@Date, @DueDate, @IdPatient,@IdDoctor)";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = query;
        
        command.Parameters.AddWithValue("@Date",newPrescriptionDto.date);
        command.Parameters.AddWithValue("@DueDate",newPrescriptionDto.dueDate);
        command.Parameters.AddWithValue("@IdPatient",newPrescriptionDto.IdPatient);
        command.Parameters.AddWithValue("@IdDoctor",newPrescriptionDto.IdDoctor);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();

    }

}