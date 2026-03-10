dotnet ef dbcontext scaffold "Host=localhost;Port=5432;Database=WindMill;Username=postgres;Password=postgres" Npgsql.EntityFrameworkCore.PostgreSQL   --output-dir DataAccess      --context MyDbContext   --no-onconfiguring        --schema public   --force



