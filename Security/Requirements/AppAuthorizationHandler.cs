using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RazorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RazorWeb.Security.Requirements
{
    public class AppAuthorizationHandler : IAuthorizationHandler
    {
        private readonly ILogger<AppAuthorizationHandler> _logger;
        private readonly UserManager<AppUser> _userManager;
        public AppAuthorizationHandler(ILogger<AppAuthorizationHandler> logger, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }



        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var requirements = context.PendingRequirements.ToList();
            foreach (var requirement in requirements)
            {
                if (requirement is GenZRequirement)
                {
                    if (IsGenZ(context.User, (GenZRequirement)requirement))
                        context.Succeed(requirement);


                    /*context.Succeed(requirement);*/
                }
                if (requirement is ArticleUpdateRequirement)
                {
                    bool canUpdate = CanUpdateArticle(context.User, context.Resource, (ArticleUpdateRequirement)requirement);
                    if (canUpdate)
                        context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }

        private bool CanUpdateArticle(ClaimsPrincipal user, object resource, ArticleUpdateRequirement requirement)
        {
            if (user.IsInRole("Admin"))
            {
                _logger.LogInformation("Admin cap nhat ...!");
                return true;
            };
            var article = resource as Article;
            var dateCreated = article.Created;
            var dateCanUpdate = new DateTime(requirement.Year, requirement.Month, requirement.Day);
            if (dateCreated < dateCanUpdate)
            {
                _logger.LogInformation("Qua ngay cap nhat!");
                return false;
            }
            _logger.LogInformation("Nguoi dung da cap nhat");
            return true;
        }

        private bool IsGenZ(ClaimsPrincipal user, GenZRequirement requirement)
        {
            var appUserTask = _userManager.GetUserAsync(user);
            Task.WaitAll(appUserTask);
            var appUser = appUserTask.Result;
            if (appUser.BirthDate == null)
            {
                _logger.LogInformation($"{appUser.UserName} khong co ngay thang nam sinh, khong thoa man GenZRequirement");
                return false;
            }
            int year = appUser.BirthDate.Value.Year;
            var result = (year >= requirement.FromYear && year <= requirement.ToYear);
            if (result)
            {
                _logger.LogInformation($"{appUser.UserName} thoa man GenZRequirement");
            }
            else
            {
                _logger.LogInformation($"{appUser.UserName} khong thoa man GenZRequirement");
            }
            return result;
        }
    }
}
