using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IDatabaseSettings>(x => x.GetRequiredService<IOptions<DatabaseSettings>>().Value);
builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<CartService>();
builder.Services.AddSingleton<CheckoutService>();


var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>()!;

var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Name);

// Add IMongoDatabase to the DI container
builder.Services.AddSingleton(mongoDatabase);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
        (
            mongoDbSettings.ConnectionString, mongoDbSettings.Name
        );
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true; // Prevent client-side scripts from accessing the cookie
        options.Cookie.SameSite = SameSiteMode.None; // Allow cross-site cookie usage
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Customize expiration time
        options.SlidingExpiration = true; // Refresh expiration on activity
        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        options.Cookie.Domain = "http://localhost:5173/";
        
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173")  // Your client-side app's URL
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();  // Allow cookies and credentials
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();


}


app.UseStaticFiles();
app.UseSwagger();      // Adds the Swagger endpoint
app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors();
app.Run();
