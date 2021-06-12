using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MeetupAPI.Entities;
using Microsoft.AspNetCore.Authorization;

namespace MeetupAPI.Authorization
{
    public class BudgetResourceOperationHandler : AuthorizationHandler<ResourceOperationRequirement, Budget>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement,
            Budget resource)
        {
            if (requirement.OperationType == OperationType.Create || requirement.OperationType == OperationType.Read)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (resource.createdBy == userId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
