using Microsoft.EntityFrameworkCore;
using WindMill.DataAccess;

namespace WindMill.Util;

public class DataSeeder(MyDbContext ctx, IConfiguration config)
{
    public async Task Initialize()
    {
        var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<DataSeeder>();
        logger.LogInformation("Starting role seeding...");
        // Adding default roles
        var rolesList = await ctx.Roles.ToListAsync();
        var existingRoleNames = rolesList.Select(r => r.Name).ToHashSet();

        var rolesToAdd = new[]
        {
            new { Name = "admin" },
            new { Name = "user" },
            new { Name = "engineer" }
        };

        foreach (var role in rolesToAdd)
        {
            if (!existingRoleNames.Contains(role.Name))
            {
                ctx.Roles.Add(new Role { Name = role.Name });
            }
        }

        ctx.SaveChanges();

        // Adding default users
        logger.LogInformation("Starting default users seeding...");
        
        var usersList = await ctx.Users.ToListAsync();
        var existingUserNames = usersList.Select(u => u.Username).ToHashSet();
        var pepper = config["SECRET"]?.Substring(0, 16);

        var usersToAdd = new[]
        {
            new User
            {
                Name ="Super Admin",
                Username = config["SUPER_USER_NAME"] ?? "admin",
                Password = GenerateHashPass.Generate(config["SUPER_PASSWORD"] ?? "adminpass", pepper),
                RoleId = ctx.Roles.First(r => r.Name == "admin").Id
            },
            new User
            {
                Name ="Normal User",
                Username = "user", 
                Password = GenerateHashPass.Generate("userpass"),
                RoleId = ctx.Roles.First(r => r.Name == "user").Id
            },
            new User
            {
                Name ="Engineer",
                Username = "engineer", 
                Password = GenerateHashPass.Generate("engineerpass"),
                RoleId = ctx.Roles.First(r => r.Name == "engineer").Id
            }
        };

        foreach (var user in usersToAdd)
        {
            if (!existingUserNames.Contains(user.Username))
            {
                ctx.Users.Add(user);
            }
        }

        ctx.SaveChanges();
    }
}