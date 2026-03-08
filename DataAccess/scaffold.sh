dotnet ef dbcontext scaffold "$CONN_STR" Npgsql.EntityFrameworkCore.PostgreSQL   --output-dir DataAccess      --context-namespace MyDbContext   --no-onconfiguring        --schema public   --force



