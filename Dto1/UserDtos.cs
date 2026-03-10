using System;

namespace WindMill.Dto1;

public class UserFullDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public Guid RoleId { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateUserDto
{
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public Guid RoleId { get; set; }
}

public class UserQueryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public Guid RoleId { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

