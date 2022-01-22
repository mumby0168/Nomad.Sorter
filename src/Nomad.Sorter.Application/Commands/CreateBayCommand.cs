using Convey.CQRS.Commands;

namespace Nomad.Sorter.Application.Commands;

public record CreateBayCommand(
    string BayId) : ICommand;