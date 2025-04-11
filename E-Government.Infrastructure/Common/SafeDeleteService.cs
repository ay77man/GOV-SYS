using E_Government.Infrastructure._Data;

public class SafeDeleteService
{
    private readonly ApplicationDbContext _context;

    public SafeDeleteService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task DeleteCustomerWithDependencies(int customerId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 1. حذف جميع المدفوعات المرتبطة
            var payments =  _context.Payments
                .Where(p => p.CustomerId == customerId)
                .ToList();
            _context.Payments.RemoveRange(payments);

            // 2. حذف جميع العدادات المرتبطة
            var meters =  _context.Meters
                .Where(m => m.CustomerId == customerId)
                .ToList();
            _context.Meters.RemoveRange(meters);

            // 3. حذف العميل نفسه
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}