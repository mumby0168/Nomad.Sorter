using Nomad.Sorter.Domain.Entities;
using Nomad.Sorter.Domain.Entities.Abstractions;

namespace Nomad.Sorter.Domain.Factories;

public interface IBayFactory
{
    IBay Create(string bayId);
}