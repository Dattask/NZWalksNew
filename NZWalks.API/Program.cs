using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Repository;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Added options to Swigger to add Authentication to Swagger itself for JWT Token purpose
builder.Services.AddSwaggerGen(options =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter a valid JWT bearer toekn",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new string[]{ } }
    });
});


//Used to Validate Models using Fluent Validation 
builder.Services.AddFluentValidation(options =>
    options.RegisterValidatorsFromAssemblyContaining<Program>());


// Used to get Connection String from AppSettings.json file
builder.Services.AddDbContext<DbContextNZWalks>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalks"));
});

//Used to To Map Region Class - creating object
builder.Services.AddScoped<IRegionRepository, RegionRepository>();

//Used to To Map Walk Class - creating object
builder.Services.AddScoped<IWalkRepository, WalkRepository>();

//Used to To Map WalkDifficulty Class - creating object
builder.Services.AddScoped<IWalkDifficultyRepository, WalkDifficultyRepository>();

//Used to inject Static users class - Users Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Used to inject Token class - IToken Repository
builder.Services.AddScoped<ITokenHandler, TokenHander>();


//Below used for AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);


//Configure JWT Services for API Authentication and Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        });

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
