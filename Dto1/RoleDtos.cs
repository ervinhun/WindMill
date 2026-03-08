using System;

namespace WindMill.Dto1;

public class RoleFullDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class CreateRoleDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class RoleQueryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

