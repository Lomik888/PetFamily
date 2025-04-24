using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace PetFamily.Application.BackgroundWorkers.HardDeleteWorker;

public sealed record HardDeleteUnActiveEntitiesWorkerOptions : IOptions<HardDeleteUnActiveEntitiesWorkerOptions>
{
    public const string OPTIONSECTION = "Hard_Delete_UnActive_Entities_Worker_Options";

    [ConfigurationKeyName("Add_Days_To_Find_Out_Last_Date_Valid_Volunteer")]
    public int AddDaysToFindOutLastDateValidVolunteer { get; set; }

    [ConfigurationKeyName("House_Delay")] public int BackgroundServiceDelay { get; set; }

    public HardDeleteUnActiveEntitiesWorkerOptions Value { get; }
}