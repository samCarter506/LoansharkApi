using LoanApplicationAPI.Models;
using LoanApplicationAPI.Models.DTO;
using LoanApplicationAPI.Services;
using LoanApplicationLibrary.Data;
using LoanApplicationLibrary.DataAccess;
using LoanApplicationLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Security.Claims;
using System.Text.Json;

namespace LoanApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private ILoanCalculationData loanCalculationData { get; set; }

        public IApplicationData applicationData { get; set; }
        public IBankingData bankingData { get; set; }
        public IEmployerData employerData { get; set; }
        public ApplicationController(ApplicationDbContext db, IDataAccess data)
        {
            _context = db;
            applicationData = new ApplicationData(data);
            bankingData = new BankingData(data);
            employerData = new EmployerData(data);
            loanCalculationData = new LoanCalculationData(db);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetApplications()
        {
            try
            {
                var applications = await _context.Applications
                    .Include(a => a.Employer)
                    .Include(a => a.Documents)
                    .Include(a => a.Loan)
                    .Include(a => a.Expenses)
                    .Select(a => new ApplicantDto
                    {
                        Id = a.Id,
                        NationalId = a.NationalId,
                        Email = a.Email,
                        Cellphone = a.Cellphone,
                        Address = a.Address,
                        FullName = a.FullName,
                        Surname = a.Surname,

                        // EMPLOYER
                        Employer = a.Employer != null
                            ? a.Employer.Employer
                            : "",

                        GrossSalary = a.Employer != null
                            ? a.Employer.GrossSalary
                            : 0,

                        NetSalary = a.Employer != null
                            ? a.Employer.NetSalary
                            : 0,

                        // LOAN
                        Amount = a.Loan != null
                            ? a.Loan.Amount
                            : 0,

                        Status = a.Loan != null
                            ? a.Loan.Status
                            : "Pending",

                        interestRate = a.Loan != null
                            ? (a.Loan.interestRate ?? 0)
                            : 0,

                        loanTermMonths = a.Loan != null
                            ? (a.Loan.loanTermMonths ?? 0)
                            : 0,

                        MonthlyPayment = a.Loan != null
                            ? Convert.ToDouble(a.Loan.monthlyPayment ?? 0)
                            : 0,

                        // EXPENSES
                        dependents = a.Expenses != null
                            ? a.Expenses.dependents
                            : 0,

                        rentAmount = a.Expenses != null
                            ? a.Expenses.rentAmount
                            : 0,

                        foodExpenses = a.Expenses != null
                            ? a.Expenses.foodExpenses
                            : 0,

                        transportExpenses = a.Expenses != null
                            ? a.Expenses.transportExpenses
                            : 0,

                        electricityExpenses = a.Expenses != null
                            ? a.Expenses.electricityExpenses
                            : 0,

                        waterExpenses = a.Expenses != null
                            ? a.Expenses.waterExpenses
                            : 0,

                        existingLoanRepayments = a.Expenses != null
                            ? a.Expenses.existingLoanRepayments
                            : 0,

                        otherExpenses = a.Expenses != null
                            ? a.Expenses.otherExpenses
                            : 0,

                        monthlyExpenses = a.Expenses != null
                            ? a.Expenses.monthlyExpenses
                            : 0,

                        TotalExpense = a.Expenses != null
                            ? a.Expenses.TotalExpense
                            : 0,

                        // DOCUMENTS
                        Documents = a.Documents.Select(d => new DocumentsDto
                        {
                            ApplicationId = d.ApplicationId,
                            DocumentType = d.DocumentType,
                            FileName = d.FileName,
                            FilePath = d.FilePath,
                            UploadedDate = d.UploadedDate
                        }).ToList()
                    })
                    .ToListAsync();

                return Ok(applications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = ex.Message,
                    InnerException = ex.InnerException?.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }
        [HttpGet("checkstatus/{nationalId}")]
        public async Task<IActionResult> CheckStatus(string nationalId)
        {
            var result = await _context.StatusDtos
                .FromSqlRaw("EXEC checkStatus @NationalId = {0}", nationalId)
                .ToListAsync();

            if (result == null || !result.Any())
            {
                return NotFound("No application found");
            }

            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplication(int id)
        {
            var application = await _context.Applications
               // .Include(a => a.Banking)
                .Include(a => a.Employer)
                .Include(a => a.Documents)
                .Include(a => a.Loan)
                .Include(a => a.Expenses)
                .Where(a => a.Id == id)
                .Select(a => new ApplicantDto
                {
                    Id = a.Id,
                    NationalId = a.NationalId,
                    Email = a.Email,
                    Cellphone = a.Cellphone,
                    Address = a.Address,
                    FullName = a.FullName,
                    Surname = a.Surname,

                    //// BANKING
                    //AccountType = a.Banking.AccountType,
                    //BankName = a.Banking.BankName,
                    //Branch = a.Banking.Branch,
                    //PaymentDate = a.Banking.PaymentDate,

                    //// EMPLOYER
                    Employer = a.Employer.Employer,
                    GrossSalary = a.Employer.GrossSalary,
                    NetSalary = a.Employer.NetSalary,

                    // LOAN
                    Amount = a.Loan.Amount,
                    Status = a.Loan.Status,
                    interestRate = a.Loan.interestRate,
                    loanTermMonths = a.Loan.loanTermMonths,

                    // EXPENSES
                    dependents = a.Expenses.dependents,
                    rentAmount = a.Expenses.rentAmount,
                    foodExpenses = a.Expenses.foodExpenses,
                    transportExpenses = a.Expenses.transportExpenses,
                    electricityExpenses = a.Expenses.electricityExpenses,
                    waterExpenses = a.Expenses.waterExpenses,
                    existingLoanRepayments = a.Expenses.existingLoanRepayments,
                    otherExpenses = a.Expenses.otherExpenses,
                    monthlyExpenses = a.Expenses.monthlyExpenses,
                    TotalExpense = a.Expenses.TotalExpense,

                    // DOCUMENTS
                    Documents = a.Documents.Select(d => new DocumentsDto
                    {
                        ApplicationId = d.ApplicationId,
                        DocumentType = d.DocumentType,
                        FileName = d.FileName,
                        FilePath = d.FilePath,
                        UploadedDate = d.UploadedDate
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (application == null)
                return NotFound("Application not found");

            return Ok(application);
        }
        [Authorize]
        [HttpPost]

        public async Task<IActionResult> CreateApplication([FromForm] ApplicantDto obj)
        {
            ApplicationModel applicationModel = new ApplicationModel();
            BankingModel bankingModel = new BankingModel();
            EmployerModel employerModel = new EmployerModel();
            ApplicationDocument document = new ApplicationDocument();
            ExpensesModel expenses = new ExpensesModel();
            LoanModel loan = new LoanModel();


            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                applicationModel.Email = obj.Email;
                applicationModel.Surname = obj.Surname;
                applicationModel.Address = obj.Address;
                applicationModel.Cellphone = obj.Cellphone;
                applicationModel.FullName = obj.FullName;
                applicationModel.NationalId = obj.NationalId;


                _context.Applications.Add(applicationModel);
                // await _context.SaveChangesAsync();

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new
                    {
                        Error = ex.Message,
                        InnerError = ex.InnerException?.Message,
                        StackTrace = ex.StackTrace
                    });
                }




                employerModel.ApplicationId = applicationModel.Id;
                employerModel.NetSalary = obj.NetSalary;
                employerModel.GrossSalary = obj.GrossSalary;
                employerModel.Employer = obj.Employer;

                _context.Employers.Add(employerModel);
                //===========================
                //Expenses
                //===========================
                expenses.ApplicationId = applicationModel.Id;
                expenses.dependents = obj.dependents;
                expenses.waterExpenses = obj.waterExpenses;
                expenses.otherExpenses = obj.otherExpenses;
                expenses.transportExpenses = obj.transportExpenses;
                expenses.foodExpenses = obj.foodExpenses;
                expenses.electricityExpenses = obj.electricityExpenses;
                expenses.existingLoanRepayments = obj.existingLoanRepayments;
                expenses.rentAmount = obj.rentAmount;

                _context.MonthlyExpense.Add(expenses);

                //===========================
                // Loan
                //===========================
                loan.Amount = obj.Amount;
                loan.Status = "Pending";
                // loan.loanTermMonths= obj.loanTermMonths;
                //  loan.interestRate=obj.interestRate;
                loan.ApplicationId = applicationModel.Id;
                _context.Loan.Add(loan);

                // ==========================
                // ID DOCUMENT
                // ==========================
                if (obj.IdDocument != null)
                {
                    var path = await SaveFile(obj.IdDocument);

                    _context.Documents.Add(new ApplicationDocument
                    {
                        ApplicationId = applicationModel.Id,
                        DocumentType = "ID Document",
                        FileName = obj.IdDocument.FileName,
                        FilePath = path,
                        UploadedDate = DateTime.Now
                    });
                }

                // ==========================
                // BANK STATEMENT
                // ==========================
                if (obj.BankStatement != null)
                {
                    var path = await SaveFile(obj.BankStatement);

                    _context.Documents.Add(new ApplicationDocument
                    {
                        ApplicationId = applicationModel.Id,
                        DocumentType = "Bank Statement",
                        FileName = obj.BankStatement.FileName,
                        FilePath = path,
                        UploadedDate = DateTime.Now
                    });
                }

                // ==========================
                // PAYSLIP
                // ==========================
                if (obj.Payslip != null)
                {
                    var path = await SaveFile(obj.Payslip);

                    _context.Documents.Add(new ApplicationDocument
                    {
                        ApplicationId = applicationModel.Id,
                        DocumentType = "Payslip",
                        FileName = obj.Payslip.FileName,
                        FilePath = path,
                        UploadedDate = DateTime.Now
                    });
                }


                string json = JsonSerializer.Serialize(obj);
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                UserAudit userAudit = new UserAudit
                {
                    TransactionType = "Application Add",
                    TransactionData = json,
                    TransactionDate = DateTime.Now,
                    CreatedById = userId
                };

                _context.Audit.Add(userAudit);

                // =====================
                // SAVE ALL
                // =====================
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new
                {
                    message = "Application created successfully",
                    applicationId = applicationModel.Id
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return BadRequest(new
                {
                    error = ex.Message,
                    stack = ex.StackTrace
                });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApplication(int id, [FromBody] ApplicantDto obj)
        {
            var application = await _context.Applications
                .Include(a => a.Banking)
                .Include(a => a.Employer)
                .Include(a => a.Loan)
                .Include(a => a.Expenses)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null)
                return NotFound("Application not found");

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // APPLICATION
                application.Email = obj.Email;
                application.Surname = obj.Surname;
                application.Address = obj.Address;
                application.Cellphone = obj.Cellphone;
                application.FullName = obj.FullName;
                application.NationalId = obj.NationalId;

                // BANKING
                //if (application.Banking != null)
                //{
                //    application.Banking.BankName = obj.BankName;
                //    application.Banking.Branch = obj.Branch;
                //    application.Banking.AccountType = obj.AccountType;
                //    application.Banking.PaymentDate = obj.PaymentDate;
                //}

                // EMPLOYER
                if (application.Employer != null)
                {
                    application.Employer.Employer = obj.Employer;
                    application.Employer.NetSalary = obj.NetSalary;
                    application.Employer.GrossSalary = obj.GrossSalary;
                }

                // LOAN
                if (application.Loan != null)
                {
                    application.Loan.Amount = obj.Amount;
                    application.Loan.interestRate = obj.interestRate;
                    application.Loan.loanTermMonths = obj.loanTermMonths;
                    application.Loan.Status = obj.Status;
                }

                // EXPENSES
                if (application.Expenses != null)
                {
                    application.Expenses.rentAmount = obj.rentAmount;
                    application.Expenses.foodExpenses = obj.foodExpenses;
                    application.Expenses.transportExpenses = obj.transportExpenses;
                    application.Expenses.electricityExpenses = obj.electricityExpenses;
                    application.Expenses.waterExpenses = obj.waterExpenses;
                    application.Expenses.existingLoanRepayments = obj.existingLoanRepayments;
                    application.Expenses.otherExpenses = obj.otherExpenses;
                    application.Expenses.dependents = obj.dependents;
                    application.Expenses.monthlyExpenses = obj.monthlyExpenses;
                }

                string json = JsonSerializer.Serialize(obj);

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; ;

                UserAudit userAudit = new UserAudit
                {
                    TransactionType = "Application Update",
                    TransactionData = json,
                    TransactionDate = DateTime.Now,
                    CreatedById = userId
                };

                _context.Audit.Add(userAudit);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Application updated successfully"
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return BadRequest(new
                {
                    error = ex.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("manage/{id}")]
        public async Task<IActionResult> ManageApplications(
    int id,
    [FromBody] LoanChargeDto obj)
        {
            try
            {
                var application = await _context.Applications
                    .Include(a => a.Loan)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (application == null)
                {
                    return NotFound();
                }

                // Update Loan Details
                application.Loan.interestRate = obj.interestRate;
                application.Loan.loanTermMonths = obj.loanTermMonths;
                application.Loan.Status = obj.Status;

                decimal monthlyPayment = 0;

                // Only calculate if not rejected
                if (obj.Status != "Rejected")
                {
                    var result = await loanCalculationData.LoanCalculations(id);

                    monthlyPayment = result.MonthlyPayment;

                    application.Loan.monthlyPayment = result.MonthlyPayment;
                    application.Loan.RemainingBalance = result.Balance;
                }
                else
                {
                    application.Loan.monthlyPayment = 0;
                    application.Loan.RemainingBalance = 0;
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Loan updated successfully",
                    monthlyPayment = monthlyPayment,
                    status = application.Loan.Status
                });
            }
            catch (Exception err)
            {
                return StatusCode(500, new
                {
                    error = err.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("calcpayment/{id}")]
        public async Task<IActionResult> CalculatePayment(int id)
        {
            try
            {
                // =========================
                // GET APPLICATION
                // =========================

                var application = await _context.Applications
                    .Include(a => a.Loan)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (application == null)
                {
                    return NotFound("Application not found");
                }

                // =========================
                // CALCULATE LOAN
                // =========================

                var result =
                    await loanCalculationData.LoanCalculations(id);

                // =========================
                // UPDATE LOAN TABLE
                // =========================

                application.Loan.monthlyPayment =
                    result.MonthlyPayment;
                application.Loan.RemainingBalance = result.Balance;

                // OPTIONAL APPROVAL LOGIC
                if (result.Balance > 2000)
                {
                    application.Loan.Status = "Approved";
                }
                else
                {
                    application.Loan.Status = "Rejected";
                }

                // =========================
                // SAVE
                // =========================

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Loan processed successfully",

                    monthlyPayment = result.MonthlyPayment,

                    loanAmount = result.TotalLoanAmount,

                    remainingBalance = result.Balance,

                    status = application.Loan.Status
                });
            }
            catch (Exception err)
            {
                return StatusCode(500, new
                {
                    error = err.Message
                });
            }
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            if (file == null) return null;

            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads"
            );

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName =
                Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/" + uniqueFileName;
        }




    }

    public class LoanChargeDto
    {
        public int loanTermMonths { get; set; }
        public int interestRate { get; set; }
        public string Status { get; set; }

    }
}