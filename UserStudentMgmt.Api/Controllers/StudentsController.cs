using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserStudentMgmt.Application.DTOs.Students;
using UserStudentMgmt.Application.Interfaces;

namespace UserStudentMgmt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var students = await _studentService.GetAllAsync();
        return Ok(students);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var student = await _studentService.GetByIdAsync(id);
        return Ok(student);
    }

    [HttpPost]
    public async Task<IActionResult> createAsync([FromBody] StudentRequestDto dto)
    {
        var created = await _studentService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] StudentRequestDto dto)
    {
        await _studentService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _studentService.DeleteAsync(id);
        return NoContent();
    }
}