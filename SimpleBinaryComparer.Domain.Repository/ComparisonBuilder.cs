using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleBinaryComparer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleBinaryComparer.Domain.Repository
{
    public class ComparisonBuilder : IEntityTypeConfiguration<Comparison>
    {
        public void Configure(EntityTypeBuilder<Comparison> builder)
        {
            builder.HasKey(c => c.Id);
        }
    }
}
