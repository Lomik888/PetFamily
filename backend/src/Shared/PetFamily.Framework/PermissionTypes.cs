namespace PetFamily.Framework;

public static class PermissionTypes
{
    public static class SpeciesModule
    {
        public const string DeleteSpecieAndBreed = "delete.species.and.breed";
    }

    public static class VolunteersModule
    {
        public static class Pet
        {
            public const string DeletePetFiles = "delete.pet.files";
            public const string UploadPetFiles = "upload.pet.files";
            public const string SetMainFilePet = "set.main.file.pet";
            public const string DeletePet = "delete.pet";
            public const string UpdateStatus = "update.status.pet";
            public const string UpdateMainInfo = "update.main.info.pet";
            public const string UpdateSerialNumberPet = "update.serial.number.pet";
            public const string UpdateSocials = "update.socials.pet";
        }

        public static class Volunteer
        {
            public const string Create = "create.volunteer";
            public const string UpdateMainInfo = "update.main.info.volunteer";
            public const string ActivateAccount = "activate.account.volunteer";
            public const string DeleteAccount = "delete.account.volunteer";
            public const string UpdateDetailsForHelp = "update.details.for.help.volunteer";
            public const string UpdateSocials = "update.socials.volunteer";
        }
    }
}