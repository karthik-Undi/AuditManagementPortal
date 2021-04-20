using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditManagementPortal.Models
{
    public class QuestionsAndType
    {
        public QuestionsAndType()
        {

        }
        public QuestionsAndType(string questions,string auditType)
        {
            Questions = questions;
            AuditType = auditType;
        }
        public string Questions { get; set; }
        public string AuditType { get; set; }
    }
}
