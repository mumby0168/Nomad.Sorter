using Nomad.Sorter.Domain.Entities;
using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Sorter.Domain.Extensions;

namespace Nomad.Sorter.Domain.Factories;

public class BayFactory : IBayFactory
{
    public IBay Create(string bayId) => 
        new Bay(bayId.ToBayId());
}