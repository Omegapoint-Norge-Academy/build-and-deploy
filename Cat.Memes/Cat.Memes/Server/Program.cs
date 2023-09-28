using Azure.Identity;
using Cat.Memes.Server.Services;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
        new DefaultAzureCredential());

    builder.Configuration.AddAzureAppConfiguration(options =>
        options.Connect(
            new Uri(builder.Configuration["AppConfigEndpoint"] ?? throw new InvalidOperationException()),
            new DefaultAzureCredential()));
}


// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddHttpClient<ICatMemeService, CatMemeService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrl"] ?? throw new InvalidOperationException());
    var authenticationString = $"{builder.Configuration["ApiUser"]}:{builder.Configuration["ApiPassword"]}";
    var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authenticationString));
    client.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
