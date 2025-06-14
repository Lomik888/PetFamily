using Microsoft.Extensions.Configuration;

namespace PetFamily.Volunteers.Infrastructure.BackgroundWorkers;

public sealed record HardDeleteUnActiveEntitiesWorkerOptions
{
    public const string OPTION_SECTION = "Hard_Delete_UnActive_Entities_Worker_Options";

    [ConfigurationKeyName("Add_Days_To_Find_Out_Last_Date_Valid_Volunteer")]
    public int AddDaysToFindOutLastDateValidVolunteer { get; init; }

    [ConfigurationKeyName("House_Delay")] public int BackgroundServiceDelay { get; init; }

    [ConfigurationKeyName("Add_Minutes_Delay")]
    public int AddMinutesDelay { get; init; }
}