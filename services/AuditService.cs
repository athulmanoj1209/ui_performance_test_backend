using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using performance_test.DTO;

namespace performance_test.services
{
    public class AuditService
    {
        private ApplicationDbContext _dbContext;
        public AuditService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TreeNodeDto>> GetAuditTreeAsync()
        {
            // 1) Load everything flat (one DB call)
            var flat = await this._dbContext.AuditNodes
                .AsNoTracking()
                .Include(n => n.Data)
                .Include(n => n.Auditor)
                .Select(n => new
                {
                    n.Id,
                    n.ParentId,
                    n.SortOrder,

                    // JSON "data"
                    Data = new NodeDataDto
                    {
                        Name = n.Data.Name,
                        Type = n.Data.Type,
                        Size = n.Data.Size,

                        EmpId = n.Auditor != null ? n.Auditor.EmpId : null,
                        Dept = n.Auditor != null ? n.Auditor.Dept : null,
                        Contact = n.Auditor != null ? n.Auditor.Contact : null,
                        Image = n.Auditor != null ? n.Auditor.Image : null,
                        Description = n.Auditor != null ? n.Auditor.Description : null
                    }
                })
                .ToListAsync();

            //2) Create lookup: Id->TreeNodeDto
            var map = flat.ToDictionary(
                x => x.Id,
                x => new TreeNodeDto { Data = x.Data, Children = new List<TreeNodeDto>() }
            );

            // 3) Attach children to parents
            foreach (var item in flat.OrderBy(x => x.SortOrder))
            {
                if (item.ParentId != null && map.ContainsKey(item.ParentId.Value))
                {
                    map[item.ParentId.Value].Children!.Add(map[item.Id]);
                }
            }

            // 4) Return roots (ParentId == null)
            var roots = flat
                .Where(x => x.ParentId == null)
                .OrderBy(x => x.SortOrder)
                .Select(x => map[x.Id])
                .ToList();

        //Optional: remove empty children lists(to match your sample exactly)
            RemoveEmptyChildren(roots);

            return roots;
        }

        public async Task<AuditorResponseDto?> GetAuditorByIdAsync(int id)
        {
            var auditor = await _dbContext.AuditorProfiles
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new AuditorResponseDto
                {
                    Id = a.Id,
                    EmpId = a.EmpId,
                    Dept = a.Dept,
                    Contact = a.Contact,
                    Image = a.Image,
                    Description = a.Description
                })
                .FirstOrDefaultAsync();

            return auditor;
        }





        private static void RemoveEmptyChildren(List<TreeNodeDto> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.Children == null || node.Children.Count == 0)
                {
                    node.Children = null;
                }
                else
                {
                    RemoveEmptyChildren(node.Children);
                }
            }
        }
    }
}
