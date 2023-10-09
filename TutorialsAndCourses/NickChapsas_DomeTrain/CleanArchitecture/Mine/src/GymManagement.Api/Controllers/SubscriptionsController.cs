using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Application.Subscriptions.Queries.GetSubscription;
using GymManagement.Contracts.Subscriptions;
using GymManagement.Domain.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISender _mediator;

    public SubscriptionsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{subscriptionId:guid}")]
    public async Task<ActionResult<SubscriptionResponse>> GetSubscription(Guid subscriptionId)
    {
        var query = new GetSubscriptionQuery(subscriptionId);

        var getSubscriptionResult = await _mediator.Send(query);

        return getSubscriptionResult.MatchFirst(
            subscription => Ok(subscription.ToSubscriptionResponse()),
            error => Problem()
        );
    }

    [HttpPost]
    public async Task<ActionResult<SubscriptionResponse>> CreateSubscription(CreateSubscriptionRequest request)
    {
        var command = new CreateSubscriptionCommand(
            request.SubscriptionType,
            request.AdminId
        );

        var createSubscriptionResult = await _mediator.Send(command);

        return createSubscriptionResult.MatchFirst(
            subscription =>
                Ok(subscription.ToSubscriptionResponse()),
            error => Problem()
        );
    }
}