using LoanApplicationLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanApplicationLibrary.Models
{
    public class ApplicationDocument
    {
        [Key]
        public int Id { get; set; }

        public int ApplicationId { get; set; }

        public string DocumentType { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public DateTime UploadedDate { get; set; }

        // ==========================
        // RELATIONSHIP
        // ==========================
        [ForeignKey(nameof(ApplicationId))]
        public ApplicationModel Application { get; set; }
    }
}
