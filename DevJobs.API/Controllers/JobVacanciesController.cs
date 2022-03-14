namespace DevJobs.API.Controllers
{
    using DevJobs.Api.Models;
    using DevJobs.API.Entities;
    using DevJobs.API.Persistence.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Serilog;

    [Route("api/job-vacancies")]
    [ApiController]
    public class JobVacanciesController : ControllerBase
    {
        private readonly IJobVacancyRepository _jobVacancyRepository;
        public JobVacanciesController(IJobVacancyRepository jobVacancyRepository)
        {
            _jobVacancyRepository = jobVacancyRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var jobVacancies = _jobVacancyRepository.GetAll();

            return Ok(jobVacancies);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var jobVacancy = _jobVacancyRepository.GetById(id);

            if (jobVacancy == null)
                return NotFound();

            return Ok(jobVacancy);
        }

        /// <summary>
        ///     Cadastra uma vaga de emprego.
        /// </summary>
        /// <remarks>
        /// {
        ///     "title": "Dev .NET",
        ///     "description": "Vaga Desenvolvedor .NET",
        ///     "company": "Empresa",
        ///     "isRemote": true,
        ///     "salaryRange": "5000 - 10000"
        /// }
        /// </remarks>
        /// <param name="model">Dados da vaga.</param>
        /// <returns>Objeto recem criado.</returns>
        /// <response code="201">Sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPost]
        public IActionResult Post(AddJobVacancyInputModel model)
        {
            Log.Information("Post JobVacancy");

            var jobVacancy = new JobVacancy(
                model.Title,
                model.Description,
                model.Company,
                model.IsRemote,
                model.SalaryRange
            );

            _jobVacancyRepository.Add(jobVacancy);

            return CreatedAtAction(
                "GetById",
                new { id = jobVacancy.Id },
                jobVacancy);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateJobVacancyInputModel model)
        {
            var jobVacancy = _jobVacancyRepository.GetById(id);

            if (jobVacancy == null)
                return NotFound();

            jobVacancy.Update(model.Title, model.Description);

            _jobVacancyRepository.Update(jobVacancy);

            return NoContent();
        }
    }
}