using Domain.DataContracts;
using MassTransit;

namespace Application.Consumers;

public class DownloadLinkConsumer : IConsumer<DownloadLink>
{
    public Task Consume(ConsumeContext<DownloadLink> context)
    {

        return Task.CompletedTask;
    }
}
