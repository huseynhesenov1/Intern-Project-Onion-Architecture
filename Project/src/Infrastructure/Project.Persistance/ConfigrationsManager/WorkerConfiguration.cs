using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Entities;

namespace Project.Persistance.ConfigrationsManager
{
    public class WorkerConfiguration : IEntityTypeConfiguration<Worker>
    {
        public void Configure(EntityTypeBuilder<Worker> builder)
        {
            builder.Property(x => x.FullName).HasMaxLength(60).IsRequired();
            builder.Property(x => x.FinCode).HasMaxLength(60).IsRequired();
        }
    }
}
