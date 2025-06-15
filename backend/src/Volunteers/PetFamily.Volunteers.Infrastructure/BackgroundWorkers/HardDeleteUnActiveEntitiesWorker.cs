using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.SharedKernel.Validation;
using PetFamily.Volunteers.Application.Abstractions;

namespace PetFamily.Volunteers.Infrastructure.BackgroundWorkers;

public class HardDeleteUnActiveEntitiesWorker : BackgroundService
{
    private readonly ILogger<HardDeleteUnActiveEntitiesWorker> _logger;
    private readonly IServiceScopeFactory _scope;
    private readonly IOptions<HardDeleteUnActiveEntitiesWorkerOptions> _workersOptions;

    public HardDeleteUnActiveEntitiesWorker(
        ILogger<HardDeleteUnActiveEntitiesWorker> logger,
        IServiceScopeFactory scope,
        IOptions<HardDeleteUnActiveEntitiesWorkerOptions> workersOptions)
    {
        Validator.Guard.NotNull(logger);
        Validator.Guard.NotNull(scope);
        Validator.Guard.NotNull(workersOptions);

        _logger = logger;
        _scope = scope;
        _workersOptions = workersOptions;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Worker started. Hard delete delay: {0}h entity to delete delay: {1}days",
            _workersOptions.Value.BackgroundServiceDelay,
            _workersOptions.Value.AddDaysToFindOutLastDateValidVolunteer);

        using var timer = new PeriodicTimer(
            TimeSpan.FromHours(_workersOptions.Value.BackgroundServiceDelay));

        do
        {
            _logger.LogInformation("HardDeleteUnActiveEntitiesWorker is working");
            var count = await PerformHardDeleteAsync(stoppingToken);
            _logger.LogInformation("HardDeleteUnActiveEntitiesWorker done. Deleted: {0}", count);
        } while (await timer.WaitForNextTickAsync(stoppingToken));
    }

    private async Task<int> PerformHardDeleteAsync(CancellationToken stoppingToken)
    {
        using var scope = _scope.CreateScope();

        var volunteerRepository = scope
            .ServiceProvider
            .GetRequiredService<IVolunteerRepository>();

        var dateTimeLasDateValidVolunteer = DateTime
            .UtcNow
            .AddDays(_workersOptions.Value.AddDaysToFindOutLastDateValidVolunteer);


        var countDeleted = await volunteerRepository.HardDeleteAllSofDeletedAsync(
            dateTimeLasDateValidVolunteer,
            _workersOptions.Value.AddMinutesDelay,
            stoppingToken);

        return countDeleted;
    }
}