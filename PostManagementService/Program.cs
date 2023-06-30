using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PostManagementService.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PostManagementServiceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PostManagementServiceContext") ?? throw new InvalidOperationException("Connection string 'PostManagementServiceContext' not found.")));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.Audience = "api://"+ builder.Configuration["AAD:ResourceId"]; //builder.Configuration["AAD:ResourceId"];
        opt.Authority = $"{builder.Configuration["AAD:InstanceId"]}{builder.Configuration["AAD:TenantId"]}";//builder.Configuration.GetSection("AAD.ResourceId").Value;
        opt.RequireHttpsMetadata = false;
    }
    );
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "Posts_";
});


var app = builder.Build();
IConfiguration configuration = app.Configuration;
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
