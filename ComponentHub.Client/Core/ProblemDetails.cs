namespace ComponentHub.Client.Core;

internal sealed class ProblemDetails
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; } = 400;
    public string Instance { get; set; } = null!;
    public string TraceId { get; set; } = null!;
    public IEnumerable<Error> Errors { get; set; } = new List<Error>();
    
    
    public sealed class Error
    {
        /// <summary>
        /// the name of the error or property of the dto that caused the error
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// the reason for the error
        /// </summary>
        public string Reason { get; set; } = null!;
        
        /// <summary>
        /// the code of the error
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// the severity of the error
        /// </summary>
        public string? Severity { get; set; }
    }
}