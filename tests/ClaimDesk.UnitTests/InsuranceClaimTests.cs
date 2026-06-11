using ClaimDesk.Domain.Entities;
using ClaimDesk.Domain.Enums;
using FluentAssertions;

namespace ClaimDesk.UnitTests;

public class InsuranceClaimTests
{
    [Fact]
    public void AssignTo_WhenSubmitted_ShouldMoveClaimToAssigned()
    {
        var claim = new InsuranceClaim
        {
            Status = ClaimStatus.Submitted
        };

        claim.AssignTo(2);

        claim.AssignedAdjusterUserId.Should().Be(2);
        claim.Status.Should().Be(ClaimStatus.Assigned);
    }

    [Fact]
    public void ChangeStatus_WhenClaimIsClosed_ShouldThrow()
    {
        var claim = new InsuranceClaim
        {
            Status = ClaimStatus.Closed
        };

        var action = () => claim.ChangeStatus(ClaimStatus.InReview);

        action.Should().Throw<InvalidOperationException>();
    }
}
