
using LabApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Options;
using LabApi.Repo;
using LabApi.Middlewares;
using LabApi.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;


namespace LabApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<AddAppNameHeaderFilter>();
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyUser", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ITIDbContext>(
                options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"
                    ),
                    SqlOptions => SqlOptions.EnableRetryOnFailure()
                    )
                );
            builder.Services.AddAuthentication(
                opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }
                ).AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes("201b499cb3cf7c611eb045aef9e0001d36caf72c5aa335612bba83013943197da43e4c40771c87eea1e5c8c640a19804563bebc67f17d18fb22b34a4d33f3ff3f793132a3d17b19e842eaa1239db2f0d7e37ff52f42241fbb21f2e8ea1eab84671bb9179b6bde36d83b9cc81a56d496cd0327f71a45ae017183a997ecd713cac7d9a64dab8d76ba19cf8d4399ba5e4886e5ff59356bc73ced9aca54acf79c4964647318b8778c6f953339d0a41cf11d9f28e2acb8754f7b9cbc509a3bf45ee15f6bfd8e9b93bd38872f13b22d0f0ee3b4a34bd16c1048216270939099075cef444e9e20e3ccf19b4525c86a7d5876d046fa95c6953cea3b93eb9952bd36b05d1")),
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidIssuer = "http://localhost:5272/",
                        ValidateIssuerSigningKey = true
                    };
                 }
                );
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            // ...existing code...

            // Update your SwaggerGen configuration
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Lab API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
            });

            // ...existing code...
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("AllowAnyUser");
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.MapControllers();
            app.Run();
        }
    }
}
