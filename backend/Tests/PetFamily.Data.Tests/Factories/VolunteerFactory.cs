﻿using PetFamily.Data.Tests.Requests;
using PetFamily.Volunteers.Domain;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using PetFamily.Volunteers.Domain.ValueObjects.SharedVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO.Collections;

namespace PetFamily.Data.Tests.Factories;

public static class VolunteerFactory
{
    public static Volunteer CreateVolunteer(RequestVolunteer requestVolunteer)
    {
        var volunteerId = VolunteerId.Create(requestVolunteer.VolunteerId).Value;
        var name = Name.Create(
            requestVolunteer.FirstName,
            requestVolunteer.LastName,
            requestVolunteer.Surname).Value;
        //var email = Email.Create(requestVolunteer.Email).Value;
        var description = Description.Create(requestVolunteer.Description).Value;
        //var experience = Experience.Create(requestVolunteer.Experience).Value;
        var phoneNumber = PhoneNumber.Create(
            requestVolunteer.RegionCode,
            requestVolunteer.Number).Value;
        //var socialNetworks = SocialNetworks.CreateEmpty().Value;
        //var detailsForHelps = DetailsForHelps.CreateEmpty().Value;
        var files = Files.CreateEmpty().Value;

        var volunteer = new Volunteer(
            volunteerId,
            name,
            //email,
            description,
            //experience,
            phoneNumber,
            //socialNetworks,
            //detailsForHelps,
            files);

        return volunteer;
    }

    public static IEnumerable<Volunteer> CreateVolunteers(IEnumerable<RequestVolunteer> requestVolunteers)
    {
        var volunteers = new List<Volunteer>();

        foreach (var volunteerRequest in requestVolunteers)
        {
            var volunteer = CreateVolunteer(volunteerRequest);
            volunteers.Add(volunteer);
        }

        return volunteers;
    }
}