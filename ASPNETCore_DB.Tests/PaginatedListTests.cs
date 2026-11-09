namespace ASPNETCore_DB.Tests;

public class PaginatedListTests
{
    // Ten numbers: 1, 2, 3, ... 10
    private static readonly IQueryable<int> TenItems = Enumerable.Range(1, 10).AsQueryable();

    [Fact]
    public void FirstPage_ContainsTheFirstItems_AndHasNoPreviousPage()
    {
        var page = PaginatedList<int>.Create(TenItems, pageIndex: 1, pageSize: 3);

        Assert.Equal(new[] { 1, 2, 3 }, page);
        Assert.False(page.HasPreviousPage);
        Assert.True(page.HasNextPage);
    }

    [Fact]
    public void TotalPages_RoundsUp_WhenTheLastPageIsPartlyFull()
    {
        var page = PaginatedList<int>.Create(TenItems, pageIndex: 1, pageSize: 3);

        Assert.Equal(4, page.TotalPages); // 3 + 3 + 3 + 1
    }

    [Fact]
    public void LastPage_ContainsTheRemainingItems_AndHasNoNextPage()
    {
        var page = PaginatedList<int>.Create(TenItems, pageIndex: 4, pageSize: 3);

        Assert.Equal(new[] { 10 }, page);
        Assert.True(page.HasPreviousPage);
        Assert.False(page.HasNextPage);
    }

    [Fact]
    public void EmptySource_GivesAnEmptyPage_WithNoNavigation()
    {
        var page = PaginatedList<int>.Create(Enumerable.Empty<int>().AsQueryable(), pageIndex: 1, pageSize: 3);

        Assert.Empty(page);
        Assert.Equal(0, page.TotalPages);
        Assert.False(page.HasNextPage);
    }
}
