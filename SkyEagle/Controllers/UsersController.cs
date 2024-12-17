using Microsoft.AspNetCore.Mvc;
using SkyDTO;
using SkyDTO.Commons;
using SkyEagle.Classes;
using SkyEagle.Repositories.Interfaces;
using System.Threading.Tasks;
using System.Threading;

namespace SkyEagle.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UsersController(IUserRepository userRepository) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationReq paging, CancellationToken ct)
        {
            var result = await _userRepository.GetAllAsync(paging, ct);
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(id, ct);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDTO userDTO, CancellationToken ct)
        {
            var createdUser = await _userRepository.AddAsync(userDTO, ct);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UserDTO userDTO, CancellationToken ct)
        {
            if (id != userDTO.Id) return BadRequest("ID không khớp");
            await _userRepository.UpdateAsync(userDTO, ct);
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id, CancellationToken ct)
        {
            await _userRepository.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
