using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using performance_test.DTO;
using performance_test.services;

namespace performance_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private AuditService _auditService;

        public AuditController(AuditService AuditService) {
            this._auditService = AuditService;
        }


        [HttpGet("audit-tree/{id}")]
        public async Task<ActionResult<List<SuccessOrErrorResponseDTO<TreeNodeDto>>>> GetAuditTree(int id)
        {
            var result = await this._auditService.GetAuditTreeAsync(id);
            return Ok(result);
        }

        [HttpGet("get-auditor/{id}")]
        public async Task<ActionResult<List<TreeNodeDto>>> GetAuditor(string id)
        {
            var result = await this._auditService.GetAuditorByIdAsync(id);
            return Ok(result);
        }

    }
}
