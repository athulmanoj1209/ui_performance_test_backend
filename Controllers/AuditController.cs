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


        [HttpGet("audit-tree")]
        public async Task<ActionResult<List<TreeNodeDto>>> GetAuditTree()
        {
            var result = await this._auditService.GetAuditTreeAsync();
            return Ok(result);
        }

        [HttpGet("get-auditor/{id}")]
        public async Task<ActionResult<List<TreeNodeDto>>> GetAuditor(int id)
        {
            var result = await this._auditService.GetAuditorByIdAsync(id);
            return Ok(result);
        }

    }
}
