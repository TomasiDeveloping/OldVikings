using Microsoft.EntityFrameworkCore;
using OldVikings.Api.Classes;
using OldVikings.Api.Database;
using OldVikings.Api.Interfaces;
using OldVikings.Api.Profiles;
using OldVikings.Api.Repositories;
using OldVikings.Api.Services;
using Quartz;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting OldVikings API . . .");
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddSerilog(options => { options.ReadFrom.Configuration(builder.Configuration); });

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddHttpClient();

    builder.Services.Configure<DiscordWebhookOptions>(builder.Configuration.GetSection("Discord:Webhooks"));

    builder.Services.AddDbContext<OldVikingsContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("OldVikings"));
    });

    builder.Services.AddAutoMapper(options => { options.AddProfile<GreetingProfile>(); });

    builder.Services.AddScoped<IGreetingRepository, GreetingRepository>();
    builder.Services.AddTransient<ITranslateService, TranslateService>();
    builder.Services.AddTransient<ITrainGuideRepository, TrainGuideRepository>();

    builder.Services.AddQuartz(options =>
    {
        var jobKey = JobKey.Create(nameof(DailyRotationJob));
        options.AddJob<DailyRotationJob>(jobKey).AddTrigger(trigger =>
        {
            trigger.ForJob(jobKey)
                .WithCronSchedule("0 1 0,3 * * ?");
        });
    });

    builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

    var app = builder.Build();

    app.UseDefaultFiles();
    app.UseStaticFiles();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();


    app.UseCors(options =>
    {
        options.AllowAnyHeader();
        options.AllowAnyMethod();
        options.AllowAnyOrigin();
    });

    app.MapControllers();

    app.MapFallbackToFile("index.html");

    app.Run();

}
catch (Exception e)
{
    Log.Fatal(e, e.Message);
}
finally
{
    Log.CloseAndFlush();
}

