dotnet ef dbcontext scaffold "$DB_CONNECTION" Npgsql.EntityFrameworkCore.PostgreSQL   --output-dir DataAccess      --context MyDbContext   --no-onconfiguring        --schema public   --force



