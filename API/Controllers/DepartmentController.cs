using Application.Core;
using Application.Departments;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DepartmentController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<Department>>> GetDepartments([FromQuery] PagingParams param)
        {
            return HandlePagedResult(await Mediator.Send(new List.Query { Params = param }));
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] Department department)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Department = department }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(Guid id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { DepartmentId = id }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(Guid id, [FromBody] DepartmentDto department)
        {
            return HandleResult(await Mediator.Send(new Edit.Command { Id = id, Department = department }));
        }
    }
}