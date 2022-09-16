using Mafiator.Common.Extensions;
using Mafiator.Data;
using Mafiator.Entities.Identity;
using Mafiator.Service.Contracts.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Impl.Identity
{

    public class IdentityDbInitializer : IIdentityDbInitializer
    {
       // private readonly IOptionsSnapshot<SiteSettings> _adminUserSeedOptions;
        private readonly IIdentityService _applicationUserManager;
        private readonly ILogger<IdentityDbInitializer> _logger;
        private readonly IApplicationRoleManager _roleManager;
        private readonly IServiceScopeFactory _scopeFactory;

        public IdentityDbInitializer(
            IIdentityService applicationUserManager,
            IServiceScopeFactory scopeFactory,
            IApplicationRoleManager roleManager,
            //IOptionsSnapshot<SiteSettings> adminUserSeedOptions,
            ILogger<IdentityDbInitializer> logger
            )
        {
            _applicationUserManager = applicationUserManager;
            _scopeFactory = scopeFactory;
            _roleManager = roleManager;
            //_adminUserSeedOptions = adminUserSeedOptions;
            _logger = logger;
        }

        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// Will create the database if it does not already exist.
        /// </summary>
        public void Initialize()
        {
            using var serviceScope = _scopeFactory.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();
        }

        /// <summary>
        /// Adds some default values to the IdentityDb
        /// </summary>
        public void SeedData()
        {
            using var serviceScope = _scopeFactory.CreateScope();
            var identityDbSeedData = serviceScope.ServiceProvider.GetService<IIdentityDbInitializer>();
            var result = identityDbSeedData.SeedDatabaseWithAdminUserAsync().Result;

            if (result == IdentityResult.Failed())
            {
                throw new InvalidOperationException(result.DumpErrors());
            }
        }

        public async Task<IdentityResult> SeedDatabaseWithAdminUserAsync()
        {
            //var fakeAvatars = FakeDbData.InitFakeAvatars(10);
            //using var serviceScope = _scopeFactory.CreateScope();
            //using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            //await context.Avatar.AddRangeAsync(fakeAvatars);
            //await context.SaveChangesAsync();
            // var adminUserSeed = _adminUserSeedOptions.Value.AdminUserSeed;

            // var name = adminUserSeed.Username;
            //var password = adminUserSeed.Password;
            //var email = adminUserSeed.Email;
            //var roleName = adminUserSeed.RoleName;
            //var firstName = adminUserSeed.FirstName;
            //var lastName = adminUserSeed.LastName;

            //var thisMethodName = nameof(SeedDatabaseWithAdminUserAsync);

            //var adminUser = await _applicationUserManager.FindByNameAsync(name);
            //if (adminUser != null)
            //{
            //    _logger.LogInformation($"{thisMethodName}: adminUser already exists.");
            //    return IdentityResult.Success;
            //}

            ////Create the `Admin` Role if it does not exist
            var adminRole = await _roleManager.FindByNameAsync(Constants.AdminRole);
            if (adminRole == null)
            {
                adminRole = new Role()
                {
                    Id = Guid.NewGuid(),
                    Name = Constants.AdminRole
                };
                var adminRoleResult = await _roleManager.CreateAsync(adminRole);
                if (adminRoleResult != IdentityResult.Failed()) return IdentityResult.Success;
                _logger.LogError($"Player Role Creation failed. {adminRoleResult.DumpErrors()}");
                return IdentityResult.Failed();
            }
            _logger.LogInformation("Admin Role already exists.");
            ////Create the `Player` Role if it does not exist
            var playerRole = await _roleManager.FindByNameAsync(Constants.PlayerRole);
            if (playerRole == null)
            {
                playerRole = new Role()
                {
                    Id = Guid.NewGuid(),
                    Name = Constants.PlayerRole
                };
                var playerRoleResult = await _roleManager.CreateAsync(playerRole);
                if (playerRoleResult != IdentityResult.Failed()) return IdentityResult.Success;
                _logger.LogError($"Player Role Creation failed. {playerRoleResult.DumpErrors()}");
                return IdentityResult.Failed();
            }
            _logger.LogInformation("Player Role already exists.");
            //adminUser = new User
            //{
            //    Id = StringExtensions.GenerateId(10),
            //    UserName = name,
            //    Email = email,
            //    EmailConfirmed = true,
            //    LockoutEnabled = false
            //};
            //var adminUserResult = await _applicationUserManager.CreateAsync(adminUser, password);
            //if (adminUserResult == IdentityResult.Failed())
            //{
            //    _logger.LogError($"{thisMethodName}: adminUser CreateAsync failed. {adminUserResult.DumpErrors()}");
            //    return IdentityResult.Failed();
            //}

            //var setLockoutResult = _applicationUserManager.SetLockoutEnabledAsync(adminUser, enabled: false).Result;
            //if (setLockoutResult == IdentityResult.Failed())
            //{
            //    _logger.LogError($"{thisMethodName}: adminUser SetLockoutEnabledAsync failed.");
            //    return IdentityResult.Failed();
            //}

            //var addToRoleResult = await _applicationUserManager.AddToRoleAsync(adminUser, adminRole.Name);
            //if (addToRoleResult == IdentityResult.Failed())
            //{
            //    _logger.LogError($"{thisMethodName}: adminUser AddToRoleAsync failed. {addToRoleResult.DumpErrors()}");
            //    return IdentityResult.Failed();
            //}


            //var claimlist = new List<Claim>();

            //foreach (var controller in _mvcActionsDiscovery.GetAllSecuredControllerActionsWithPolicy(ConstantPolicies.DynamicPermission))
            //{
            //    foreach (var action in controller.MvcActions)
            //    {
            //        claimlist.Add(new Claim(ConstantPolicies.DynamicPermissionClaimType, action.ActionId));
            //    }
            //}
            //var Result = await _applicationUserManager.AddClaimsAsync(adminUser, claimlist);
            //if (!Result.Succeeded)
            //{
            //    _logger.LogError($"{thisMethodName}: adminUser AddToClaimsAsync failed. {Result.DumpErrors()}");
            //    return IdentityResult.Failed();
            //}

            return IdentityResult.Success;
        }
    }
}