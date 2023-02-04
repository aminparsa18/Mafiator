using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc(document =>
{
    document.Version = "v1.0";
});

var app = builder.Build();

app.UseAuthorization();
app.UseFastEndpoints(f =>
{
    f.Versioning.Prefix = "v";
});
app.UseSwaggerGen();
app.UseHttpsRedirection();

app.Run();