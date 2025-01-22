using CampusCourses.Data;
using CampusCourses.Email;
using CampusCourses.Services;
using CampusCourses.Services.Exceptions;
using CampusCourses.Services.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Reflection;
using System.Text;
using Quartz;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var emailConfig = builder.Configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    q.AddJob<EmailJob>(opts =>
        opts.WithIdentity("EmailJob")
            .StoreDurably()
    );
    q.AddTrigger(opts => opts
        .ForJob("EmailJob")
        .WithIdentity("EmailsTrigger")
        .StartNow()
        .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(20)
            .RepeatForever())
    );
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ValidateModel());
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Logging.AddConsole();

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IConnectionMultiplexer>(provider => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")));

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<HelperService>();
builder.Services.AddScoped<TokenBlacklistService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<StartDataService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

                if (authHeader != null && authHeader.StartsWith("Bearer "))
                {
                    var token = authHeader.Substring("Bearer ".Length).Trim();

                    if (token == null)
                    {
                        context.Fail("Неверный токен");
                        return;
                    }

                    var blacklistService = context.HttpContext.RequestServices.GetRequiredService<TokenBlacklistService>();

                    try
                    {
                        if (await blacklistService.IsTokenRevoked(token))
                        {
                            context.Fail("Токен истек");
                        }
                    }
                    catch (Exception ex)
                    {
                        context.Fail("Ошибка сервера");
                    }
                }
            },
            OnChallenge = async context =>
            {
                context.HandleResponse();
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = StatusCodes.Status401Unauthorized;

                var message = context.Error ?? "Пользователь не авторизован";
                var error = new
                {
                    StatusCode = response.StatusCode,
                    Message = message
                };

                await response.WriteAsJsonAsync(error);
            }
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    var securitySheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT token **Bearer {token}**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securitySheme);

    var securiryRequirement = new OpenApiSecurityRequirement
    {
        {securitySheme, new string[] {} }
    };
    c.AddSecurityRequirement(securiryRequirement);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();

    dbContext.Database.Migrate();

    var startDataService = scope.ServiceProvider.GetRequiredService<StartDataService>();

    if (!dbContext.Accounts.Any())
    {
        var accounts = startDataService.CreateAccounts();
        dbContext.Accounts.AddRange(accounts);
        await dbContext.SaveChangesAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CampusCourses.Services.Exceptions.Middleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
