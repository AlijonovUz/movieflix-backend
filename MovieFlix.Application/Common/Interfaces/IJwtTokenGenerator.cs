using MovieFlix.Domain.Entities;

namespace MovieFlix.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string Generate(User user);
}