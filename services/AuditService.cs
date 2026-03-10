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

        public async Task<SuccessOrErrorResponseDTO<List<TreeNodeDto>>> GetAuditTreeAsync()
        {
            // 1) Load everything flat (one DB call)
            try
            {

                var beforeFlat = await this._dbContext.AuditNodes
                        .AsNoTracking()
                        .Include(n => n.Data)
                        .Include(n => n.Auditor)
                        .ToListAsync();

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

                // so the data will be in the order of 

                //                [
                //  {
                //    "Id": 1,
                //    "ParentId": null,
                //    "SortOrder": 1,
                //    "Data": {
                //      "Name": "Audit Unit - IT",
                //      "Type": "IT",
                //      "Size": "12 staff",
                //      "EmpId": null,
                //      "Dept": null,
                //      "Contact": null,
                //      "Image": null,
                //      "Description": null
                //    }
                //                },
                //  {
                //                    "Id": 2,
                //    "ParentId": 1,
                //    "SortOrder": 1,
                //    "Data": {
                //                        "Name": "IT Audit Team",
                //      "Type": "IT",
                //      "Size": "4 staff",
                //      "EmpId": null,
                //      "Dept": null,
                //      "Contact": null,
                //      "Image": null,
                //      "Description": null
                //    }
                //                },
                //  {
                //                    "Id": 3,
                //    "ParentId": 2,
                //    "SortOrder": 1,
                //    "Data": {
                //                        "Name": "Alice Johnson",
                //      "Type": "Lead Auditor",
                //      "Size": null,
                //      "EmpId": "AU1001",
                //      "Dept": "IT",
                //      "Contact": "alice.johnson@company.com",
                //      "Image": "https://randomuser.me/api/portraits/women/44.jpg",
                //      "Description": "Alice Johnson is a Lead IT Auditor with over 10 years..."
                //    }
                //                },
                //  {
                //                    "Id": 4,
                //    "ParentId": 2,
                //    "SortOrder": 2,
                //    "Data": {
                //                        "Name": "Bhavesh Kumar",
                //      "Type": "Auditor",
                //      "Size": null,
                //      "EmpId": "AU1002",
                //      "Dept": "IT",
                //      "Contact": "bhavesh.kumar@company.com",
                //      "Image": "https://randomuser.me/api/portraits/men/32.jpg",
                //      "Description": "Bhavesh Kumar is an IT Auditor focused on application security..."
                //    }
                //                },
                //  ...
                //]

                //2) Create lookup: Id->TreeNodeDto
                var map = flat.ToDictionary(
                    x => x.Id,
                    x => new TreeNodeDto { Data = x.Data, Children = new List<TreeNodeDto>() }
                );

                // 3) Attach children to parents
                foreach (var item in flat.OrderBy(x => x.SortOrder))
                {
                    if (item.ParentId != null && map.ContainsKey(item.ParentId.Value)) // when we write int? ParentId in the model then .Value is required it is used to convert from int? → int 
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

                return new SuccessOrErrorResponseDTO<List<TreeNodeDto>>
                {
                    StatusCode = 200,
                    Data = roots,
                };

            }
            catch (Exception ex)
            {
                return new SuccessOrErrorResponseDTO<List<TreeNodeDto>>
                {
                    StatusCode = 500,
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<SuccessOrErrorResponseDTO<AuditorResponseDto?>> GetAuditorByIdAsync(string id)
        {
            try
            {
                var auditor = await _dbContext.AuditorProfiles
                   .AsNoTracking()
                   .Where(a => a.EmpId == id)
                   .Include(a => a.AuditorServices)
                   .Include(a => a.AuditNode)
                       .ThenInclude(an => an.Data) // optional for EF navigation, projection below will pick fields
                       .Select(a => new AuditorResponseDto
                        {
                            Id = a.Id,
                            EmpId = a.EmpId,
                            Dept = a.Dept,
                            Contact = a.Contact,
                            Image = a.Image,
                            Description = a.Description,
                            Services = a.AuditorServices.Select(s => new AuditServiceDto { 
                                id = s.Id,
                                ServiceName = s.ServiceName,
                            }).ToList(),
                            AuditNode = a.AuditNode == null ? null : new SimpleAuditNodeDto
                            {
                                 Id = a.AuditNode.Id,
                                 Data = a.AuditNode.Data == null ? null : new SimpleAuditNodeDataDto
                                 {
                                 Name = a.AuditNode.Data.Name,
                                 Type = a.AuditNode.Data.Type,
                                 Size = a.AuditNode.Data.Size
                                 }
                            }
                       }).FirstOrDefaultAsync();

                return new SuccessOrErrorResponseDTO<AuditorResponseDto?>
                {
                    StatusCode = 200,
                    Data = auditor,
                };
            }
            catch (Exception ex)
            {
                return new SuccessOrErrorResponseDTO<AuditorResponseDto?>
                {
                    StatusCode = 500,
                    ErrorMessage = ex.Message,
                };
            }
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
