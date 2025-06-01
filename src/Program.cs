using bestvinnytsa.web.Data;
using bestvinnytsa.web.Data.Mongo;
using bestvinnytsa.web.Data.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------------------
// 1. Регіструємо MongoSettings з конфігурації
// ----------------------------------------------------------------------
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings")
);

// ----------------------------------------------------------------------
// 2. Додаємо MongoClient і IMongoDatabase
// ----------------------------------------------------------------------
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddScoped(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// ----------------------------------------------------------------------
// 3. Додаємо MongoContext
// ----------------------------------------------------------------------
builder.Services.AddScoped<MongoContext>(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return new MongoContext(database);
});

// ----------------------------------------------------------------------
// 4. Регіструємо JwtSettings
// ----------------------------------------------------------------------
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings")
);

// ----------------------------------------------------------------------
// 5. Регіструємо сервіси (Authentication, Campaign, Application)
// ----------------------------------------------------------------------
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IMessageService, MessageService>();

// ----------------------------------------------------------------------
// 6. Налаштовуємо JWT-Bearer
// ----------------------------------------------------------------------
var jwtSection = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSection.GetValue<string>("SecretKey");
var issuer = jwtSection.GetValue<string>("Issuer");
var audience = jwtSection.GetValue<string>("Audience");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// ----------------------------------------------------------------------
// 7. Додаємо контролери та Swagger
// ----------------------------------------------------------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ----------------------------------------------------------------------
// 8. Викликаємо Seeder (створюємо ролі та категорії, якщо їх нема)
// ----------------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedRolesAndCategoriesAsync(scope.ServiceProvider);
}

// ----------------------------------------------------------------------
// 9. Налаштовуємо middleware
// ----------------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// (Якщо треба CORS — додайте тут)
app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
