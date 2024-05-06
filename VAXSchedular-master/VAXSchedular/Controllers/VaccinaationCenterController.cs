using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VAXSchedular.core.Entities;
using VAXSchedular.core.Repository.Contract;
using VAXSchedular.Dtos;

namespace VAXSchedular.Controllers
{

	public class VaccinaationCenterController : BaseApiController
	{
		private readonly IGenericRepository<VaccinationCenter> _vaccinationCenterRepo;
		private readonly IGenericRepository<Reservation> _reservationRepo;
		private readonly IGenericRepository<User> _patientRepo;
		private readonly IVaccinationCenterRepository<VaccinationCenter> _vaccCenterRepo;
		private readonly IReservationRepository<Reservation> _reserveRepoWithVaccinesMany;
		private readonly IGenericRepository<Certificate> _certificateRepo;
		private readonly IGenericRepository<Vaccine> _vaccineRepo;

		public VaccinaationCenterController(IGenericRepository<VaccinationCenter> vaccinationCenterRepo,IGenericRepository<Reservation> reservationRepo,IGenericRepository<User> patientRepo,IVaccinationCenterRepository<VaccinationCenter> vaccCenterRepo,IReservationRepository<Reservation> reserveRepoWithVaccinesMany,IGenericRepository<Certificate> certificateRepo,IGenericRepository<Vaccine> vaccineRepo)
        {
			_vaccinationCenterRepo = vaccinationCenterRepo;
			_reservationRepo = reservationRepo;
			_patientRepo = patientRepo;
			_vaccCenterRepo = vaccCenterRepo;
			_reserveRepoWithVaccinesMany = reserveRepoWithVaccinesMany;
			_certificateRepo = certificateRepo;
			_vaccineRepo = vaccineRepo;
		}


		[Authorize(Roles = "Vaccination Center")]
		[HttpPut("ApproveReservationStatus")]

		public async Task<ActionResult> ApproveVaccinationCenter()
		{
			var id = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
			var reservations= await _reservationRepo.GetAll();
			

			foreach(var reservation in reservations)
			{
				if (reservation.VaccinationCenterId == id)
				{
					reservation.ReservationStatus = ReservationStatus.Approved;
					await _reservationRepo.Update(reservation);
				}
			}
			return Ok();
		}

		
		[Authorize(Roles ="Vaccination Center")]
		[HttpGet("ViewPatientsAssociatedWithVaccine")]
		public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatientsWithItsVaccines()
		{
			var id = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
			var vaccinationCenter=await _vaccCenterRepo.Get(id);

			var patientsDto=new List<PatientDto>();
			foreach(var vaccine in vaccinationCenter.Vaccines)
			{
				var reservations=await _reserveRepoWithVaccinesMany.GetAllForSpecficVaacine(vaccine.Id);
				foreach(var reservation in reservations)
				{
					var user = await _patientRepo.Get(reservation.PatientId);
					
					var patientDto=new PatientDto()
					{
						Email = user.Email,
						Name = user.Name,
						PhoneNumber=user.PhoneNumber,
						Address = user.Address,
						VaccineName=vaccine.Name,
						DoseNumber=reservation.DoseNumber
					};
					patientsDto.Add(patientDto);
					

				}
			}
			return Ok(patientsDto);

		}

		[Authorize(Roles = "Vaccination Center")]
		[HttpPost("createCertificate")]
		public async Task<ActionResult> CreateCertificate()
		{
			var reservations=await _reservationRepo.GetAll();
			foreach(var reservation in reservations)
			{
				if(reservation.DoseNumber==DoseNumber.Second)
				{
					var certificate = new Certificate()
					{
						ReservationId = reservation.Id,
						Reservation = reservation
					};
					await _certificateRepo.Add(certificate);
				}

				
			}
			return Ok();
		}






		



    }
}
