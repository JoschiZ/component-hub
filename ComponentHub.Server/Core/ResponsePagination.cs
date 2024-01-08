namespace ComponentHub.Server.Core;


internal interface IPaginatedResponse
{
    ResponsePagination Pagination { get; }
}

internal sealed record ResponsePagination(int CurrentPage, int PageSize, int TotalItems)
{
    public static ResponsePagination CreateFromRequest(IPaginatedRequest request, int totalItems) =>
        new ResponsePagination(request.Page, request.PageSize, totalItems);
}; 