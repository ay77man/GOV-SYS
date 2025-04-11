using E_Government.Core.Domain.Entities;
using E_Government.Core.Domain.RepositoryContracts.Infrastructure;
using E_Government.Core.Domain.RepositoryContracts.Persistence;
using E_Government.Core.Domain.Specification.Bills;
using E_Government.Core.DTO;
using E_Government.Core.ServiceContracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace E_Government.Core.Services
{
    public class BillingService : IBillingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBillNumberGenerator _billNumberGenerator;
        private readonly IPaymentService _paymentService;

        public BillingService(
            IUnitOfWork unitOfWork,
            IBillNumberGenerator billNumberGenerator,
            IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _billNumberGenerator = billNumberGenerator;
            _paymentService = paymentService;
        }

        public async Task<BillPaymentResult> GenerateAndPayBill(GenerateBillRequestDto request)
        {
            try
            {
                // 1. Validate customer
                // 1. Get customer WITH their meters using specification
                var customerSpec = new CustomerWithMetersSpec(request.CustomerId);
                var customer = await _unitOfWork.GetRepository<Customer>()
                                .GetFirstOrDefaultWithSpecAsync(customerSpec);

                if (customer is null)
                    return new BillPaymentResult
                    {
                        Success = false,
                        ErrorMessage = $"Customer with ID {request.CustomerId} not found"
                    };

                // 2. Validate meter exists for this type
                var meter = customer.Meters.FirstOrDefault(m => m.Type == request.Type);
                if (meter is null)
                {
                    var availableTypes = customer.Meters
                        .Select(m => m.Type.ToString())
                        .Distinct();

                    return new BillPaymentResult
                    {
                        Success = false,
                        ErrorMessage = availableTypes.Any()
                            ? $"No {request.Type} meter found. Available types: {string.Join(", ", availableTypes)}"
                            : "Customer has no registered meters"
                    };
                }


                // 3. Get latest reading using meter specification
                var readingSpec = new MeterReadingSpecs(meter.Id);
                var latestReading = await _unitOfWork.GetRepository<MeterReading>()
                                    .GetFirstOrDefaultWithSpecAsync(readingSpec);

                // 4. Create bill
                var bill = new Bill
                {
                    BillNumber = _billNumberGenerator.Generate(),
                    IssueDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(30),
                    Status = BillStatus.Pending,
                    MeterId = meter.Id,
                    CustomerId = customer.Id,
                    PreviousReading = latestReading?.Value ?? 0,
                    CurrentReading = request.CurrentReading,
                    UnitPrice = GetUnitPrice(request.Type, customer.Category),
                    Type = request.Type,

                    Amount = CalculateBillAmount(
                        currentReading: request.CurrentReading,
                        previousReading: latestReading?.Value ?? 0,
                        type: request.Type,
                        category: customer.Category)
                };

                // 5. Save bill
                await _unitOfWork.GetRepository<Bill>().AddAsync(bill);
                await _unitOfWork.CompleteAsync();

                // 6. Create payment intent
                var paymentRequest = new BillPaymentRequest
                {
                    BillId = bill.Id,
                    CustomerEmail = customer.Email // Optional
                };

                var paymentResult = await _paymentService.CreatePaymentIntent(paymentRequest);

                // 7. Update bill with payment info
                bill.StripePaymentId = paymentResult.PaymentIntentId;
                _unitOfWork.GetRepository<Bill>().Update(bill);
                await _unitOfWork.CompleteAsync();

                // 8. Return result
                return new BillPaymentResult
                {
                    Success = true,
                    PaymentIntentId = paymentResult.PaymentIntentId,
                    ClientSecret = paymentResult.ClientSecret,
                    Amount = bill.Amount,
                    BillNumber = bill.BillNumber,

                };
            }
            catch (Exception ex)
            {
                // Log error here
                return new BillPaymentResult
                {
                    Success = false,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                };
            }
        }

       

        private decimal CalculateBillAmount(decimal currentReading, decimal previousReading,
            MeterType type, CustomerCategory category)
        {
            var consumption = currentReading - previousReading;
            var unitPrice = GetUnitPrice(type, category);
            return consumption * unitPrice;
        }

        private decimal GetUnitPrice(MeterType type, CustomerCategory category)
        {
            return type switch
            {
                MeterType.Electricity => category == CustomerCategory.Residential ? 0.5m : 0.8m,
                MeterType.Water => category == CustomerCategory.Residential ? 0.3m : 0.6m,
                MeterType.Gas => category == CustomerCategory.Residential ? 0.4m : 0.7m,
                _ => 0.5m
            };
        }



        public async Task<MeterRegistrationResult> RegisterMeter(RegisterMeterDto request)
        {
            try
            {
                // Validate customer exists
                var customer = await _unitOfWork.GetRepository<Customer>()
                    .GetAsync(request.CustomerId);

                if (customer == null)
                {
                    return new MeterRegistrationResult
                    {
                        Success = false,
                        ErrorMessage = "Customer not found",
                        ErrorCode = "CUSTOMER_NOT_FOUND"
                    };
                }

                // Check if customer already has a meter of this type
                var existingMeters = await _unitOfWork.GetRepository<Meter>()
                    .GetAllAsync(); // Get all meters first

                var existingMeter = existingMeters
                    .FirstOrDefault(m => m.CustomerId == request.CustomerId && m.Type == request.Type);

                if (existingMeter != null)
                {
                    return new MeterRegistrationResult
                    {
                        Success = false,
                        ErrorMessage = $"Customer already has a {request.Type} meter registered",
                        ErrorCode = "METER_ALREADY_EXISTS"
                    };
                }

                // Create new meter
                var meter = new Meter
                {
                    MeterNumber = GenerateMeterNumber(request.Type),
                    Type = request.Type,
                    CustomerId = request.CustomerId,
                    InstallationDate = DateTime.Now,
                    IsActive = true
                };

                await _unitOfWork.GetRepository<Meter>().AddAsync(meter);
                await _unitOfWork.CompleteAsync();

                return new MeterRegistrationResult
                {
                    Success = true,
                    Meter = meter
                };
            }
            catch (Exception ex)
            {
                // Log exception here
                return new MeterRegistrationResult
                {
                    Success = false,
                    ErrorMessage = "An error occurred while registering the meter",
                    ErrorCode = "REGISTRATION_ERROR"
                };
            }
        }
        private string GenerateMeterNumber(MeterType type)
        {
            return $"BL-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

        }

    }
}