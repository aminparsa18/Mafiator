using Bogus;
using Mafiator.Common.Helpers;
using Mafiator.Entities;
using Mafiator.Entities.Identity;
using System;
using System.Collections.Generic;

namespace Mafiator.Data;
public class FakeDbData
{
    public static List<User> InitFakeUsers(int count)
    {
        var userFaker = new Faker<User>()
           .RuleFor(u => u.Id, _ => Guid.NewGuid())
           .RuleFor(p => p.CountryCode, _ => "TR")
           .RuleFor(p => p.Code, _ => RandomHelper.CreateRandomText(10))
           .RuleFor(u => u.DisplayName, f => f.Name.FirstName())
           .RuleFor(u => u.Image, f => f.Internet.Avatar())
           .RuleFor(u => u.Score, _ => 100)
           .RuleFor(u => u.UserName, f => f.Internet.UserName())
           .RuleFor(u => u.PasswordHash, f => f.Internet.Password())
           .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
           .RuleFor(u => u.PhoneNumberConfirmed, _ => true);

        return userFaker.Generate(count);
    }

    public static List<Avatar> InitFakeAvatars(int count)
    {
        var avatarFaker = new Faker<Avatar>()
           .RuleFor(u => u.Id, _ => Guid.NewGuid())
           .RuleFor(p => p.Name, f => f.Internet.Avatar());

        return avatarFaker.Generate(count);
    }
}
