namespace LoanApplicationAPI.Models.DTO
{
    public class DocumentsDto
    {
        public int Id { get; set; }

        public int ApplicationId { get; set; }

        public string DocumentType { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public DateTime UploadedDate { get; set; }

    }
}
