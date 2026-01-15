using ChecktonPld.DOM.Modelos;
using Microsoft.EntityFrameworkCore;

namespace ChecktonPld.DOM.ApplicationDbContext;

public class ServiceDbContext : DbContext
{
    
    public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options)
    {
    }
    
    public DbSet<ValidacionCurp> ValidacionCurp { get; set; }   
    
}