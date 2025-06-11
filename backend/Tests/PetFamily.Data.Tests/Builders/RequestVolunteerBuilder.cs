using AutoFixture;
using PetFamily.Data.Tests.Requests;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Data.Tests.Builders;

public static class RequestVolunteerBuilder
{
    private const int MAX_EXPERIENCE = Experience.VELUE_MAX_LENGHT;

    private static readonly Fixture _autoFixture = new Fixture();
    private static readonly Random _random = new Random();

    private static readonly List<string> _emailDomains = new List<string>()
    {
        "0-mail.com",
        "007addict.com",
        "020.co.uk",
        "027168.com",
        "0815.ru",
        "0815.su",
        "0clickemail.com",
        "0sg.net",
        "0wnd.net",
        "0wnd.org",
        "1033edge.com",
        "10mail.org",
        "10minutemail.co.za",
        "10minutemail.com",
        "11mail.com",
        "123-m.com",
        "123.com",
        "123box.net",
        "123india.com",
        "123mail.cl",
        "123mail.org",
        "123qwe.co.uk",
        "126.com",
        "126.net",
        "138mail.com",
        "139.com",
        "150mail.com",
        "150ml.com",
        "15meg4free.com",
        "163.com",
        "16mail.com",
        "188.com",
        "189.cn",
        "1auto.com",
        "1ce.us",
        "1chuan.com",
        "1colony.com",
        "1coolplace.com",
        "1email.eu",
        "1freeemail.com",
        "1fsdfdsfsdf.tk",
        "1funplace.com",
        "1internetdrive.com",
        "1mail.ml",
        "1mail.net",
        "1me.net",
        "1mum.com",
        "1musicrow.com",
        "1netdrive.com",
        "1nsyncfan.com",
        "1pad.de",
        "1under.com",
        "1webave.com",
        "1webhighway.com",
        "1zhuan.com",
        "2-mail.com",
        "20email.eu",
        "20mail.in",
        "20mail.it",
        "20minutemail.com",
        "212.com",
        "21cn.com",
        "247emails.com",
        "24horas.com",
        "2911.net",
        "2980.com",
        "2bmail.co.uk",
        "2coolforyou.net",
        "2d2i.com",
        "2die4.com",
        "2fdgdfgdfgdf.tk",
        "2hotforyou.net",
        "2mydns.com",
        "2net.us",
        "2prong.com",
        "2trom.com",
        "3000.it",
        "30minutemail.com",
        "30minutesmail.com",
        "3126.com",
        "321media.com",
        "33mail.com",
        "360.ru",
        "37.com",
        "3ammagazine.com"
    };

    public static RequestVolunteer VolunteerBuild()
    {
        var experience = _random.Next(0, MAX_EXPERIENCE + 1);
        var regionCode = _random.Next(1, 10);
        var number = _random.Next(10, int.MaxValue);
        var randomEmailNumber = _random.Next(100, 1000000);
        var randomEmailDomainIndex = _random.Next(0, _emailDomains.Count + 1);

        return _autoFixture
            .Build<RequestVolunteer>()
            .With(x => x.Email, $"{randomEmailNumber}@{_emailDomains[randomEmailDomainIndex]}")
            .With(x => x.FirstName, "Артем")
            .With(x => x.LastName, "Торак")
            .With(x => x.Surname, "Генрихович")
            .With(x => x.Experience, experience)
            .With(x => x.RegionCode, $"+{regionCode}")
            .With(x => x.Number, number.ToString())
            .Create();
    }

    public static IEnumerable<RequestVolunteer> VolunteersBuild(int count)
    {
        var requestVolunteers = new List<RequestVolunteer>();

        for (int i = 0; i < count; i++)
        {
            var requestVolunteer = VolunteerBuild();
            requestVolunteers.Add(requestVolunteer);
        }

        return requestVolunteers;
    }
}