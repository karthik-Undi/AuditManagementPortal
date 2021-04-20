using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AuthorizationAPI.Model
{
    public partial class Audit
    {
        public int Auditid { get; set; }
        public string ProjectName { get; set; }
        public string ProjectManagerName { get; set; }
        public string ApplicationOwnerName { get; set; }
        public string AuditType { get; set; }
        public DateTime? AuditDate { get; set; }
        public string ProjectExecutionStatus { get; set; }
        public string RemedialActionDuration { get; set; }
    }
}
