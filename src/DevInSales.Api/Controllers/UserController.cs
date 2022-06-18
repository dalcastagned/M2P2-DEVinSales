using DevInSales.Api.Dtos;
using DevInSales.Core.Data.Dtos;
using DevInSales.Core.Entities;
using DevInSales.EFCoreApi.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RegexExamples;
using SolarEnergyApi.Api.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace DevInSales.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public UserController(
            IUserService userService,
            UserManager<User> userManager,
            IConfiguration configuration
        )
        {
            _userService = userService;
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpGet]
        [SwaggerResponse(
            statusCode: StatusCodes.Status200OK,
            description: "Ok",
            type: typeof(IEnumerable<ReadUser>)
        )]
        [SwaggerResponse(statusCode: StatusCodes.Status204NoContent, description: "No Content")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status401Unauthorized,
            description: "Unauthorized"
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [SwaggerOperation(Summary = "Get users list")]
        public async Task<IActionResult> GetUsers(string? nome, string? DataMin, string? DataMax)
        {
            var users = await _userService.GetUsers(nome, DataMin, DataMax);
            if (users == null)
                return NoContent();

            return Ok(users.Select(user => new ReadUser(user)).ToList());
        }

        [HttpGet("{id}")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status200OK,
            description: "Ok",
            type: typeof(ReadUser)
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status401Unauthorized,
            description: "Unauthorized"
        )]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, description: "Not Found")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [SwaggerOperation(Summary = "Get user by id")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetById(id);

            if (user == null)
                return NotFound();

            return Ok(new ReadUser(user));
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, description: "Created")]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, description: "Bad Request")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [SwaggerOperation(Summary = "Add new user")]
        public async Task<IActionResult> SignUp(AddUser model)
        {
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                EmailConfirmed = true,
                BirthDate = model.BirthDate
            };

            var verifyEmail = new EmailValidate();

            if (!verifyEmail.IsValidEmail(user.Email))
                return BadRequest("Email inválido");

            if (user.BirthDate.AddYears(18) > DateTime.Now)
                return BadRequest("Usuário não tem idade suficiente");

            if (
                model.Password.Length < 6
                || model.Password.Length == 0
                || model.Password.All(ch => ch == model.Password[0])
            )
                return BadRequest(
                    "Senha inválida, deve conter pelo menos 6 caracteres e deve conter ao menos um caracter diferente"
                );

            var result = await _userService.SignUp(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { user = model.Username });
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, description: "Success")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status401Unauthorized,
            description: "Unauthorized"
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [SwaggerOperation(Summary = "Login")]
        public async Task<IActionResult> Login(Login login)
        {
            try
            {
                var user = await _userService.GetUser(login.Email);
                var result = await _userService.Login(user, login.Password);

                if (result.Succeeded)
                {
                    return Ok(
                        new
                        {
                            token = new GenerateJWT()
                                .Generate(user, _configuration, _userManager)
                                .Result,
                            user = login.Email
                        }
                    );
                }
                return Unauthorized("User or password incorrect");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Object reference not set to an instance of an object."))
                {
                    return Unauthorized("User or password incorrect");
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerResponse(statusCode: StatusCodes.Status204NoContent, description: "No Content")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status401Unauthorized,
            description: "Unauthorized"
        )]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, description: "Not Found")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        [SwaggerOperation(Summary = "Remove User")]
        public async Task<IActionResult> RemoveUser(int id)
        {
            var user = await _userService.GetById(id);

            if (user == null)
                return NotFound();

            var result = await _userService.RemoveUser(user);

            if (result.Succeeded)
                return NoContent();

            return BadRequest();
        }
    }
}
