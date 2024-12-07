using System.Text;
using Identity.Application.Interfaces.Domain;
using Identity.Application.Mapping;
using Identity.Application.Services;
using Identity.Domain.Interfaces;
using Identity.Domain.UnitOfWorks;
using Identity.Infrastructure;
using Identity.Infrastructure.Repositories;
using Identity.Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

#region Master

builder.Services.RegisterMapsterConfiguration();

#endregion


#region Database

var connectionString = builder.Configuration.GetConnectionString("IdentityService_DB");
builder.Services.AddDbContext<IdentityContext>(x => x.UseSqlServer(connectionString));

#endregion

#region Transient

builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IAccountService, AccountService>();

builder.Services.AddTransient<IJwtTokenRepository, JwtTokenRepository>();
builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

#endregion

builder.Services.AddControllers();
var message = "";

#region Authentication

builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opt =>
    {
        // for development only
        opt.RequireHttpsMetadata = false;
        opt.SaveToken = true;
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:SecretKey"])),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            TokenDecryptionKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:EncryptionKey"]))
        };

        opt.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "text/plain";
                message = "احراز هویت با شکست مواجه شد.";
                return context.Response.WriteAsync(message);
            },
            OnTokenValidated = async context =>
            {
                var _jwtService = context.HttpContext.RequestServices.GetRequiredService<IJwtTokenService>();
                var token = context.SecurityToken.ToString().Replace("Bearer ", "");
                var result = await _jwtService.CheckTokenValidation(token);
                if (result.Result != true)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "text/plain";
                    context.Fail(" از اوناششششششش احراز هویت با شکست مواجه شد.");
                }
                context.Success();
            }
        };
    });

#endregion


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWT Auth Sample",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer jhfdkj.jkdsakjdsa.jkdsajk\""
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
;

var app = builder.Build();

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
