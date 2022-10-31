using ExpensesData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesData.Context;
public class OutLaysConfiguration
{
    public static void Configure(EntityTypeBuilder<Outlay> builder)
    {
        builder.ToTable("outlays")
            .HasKey(outlay => outlay.Id);

        builder.Property(outlay => outlay.Description)
            .IsRequired(false);

        builder.Property(outlay => outlay.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(outlay => outlay.RoomId)
            .HasColumnName("room_id")
            .IsRequired();

        builder.Ignore(outlay => outlay.ToReadable);

        builder.HasOne(outlay => outlay.User)
            .WithMany(user => user.Outlays)
            .HasForeignKey(outlay => outlay.UserId);

        builder.HasOne(outlay => outlay.Room)
            .WithMany(room => room.Outlays)
            .HasForeignKey(outlay => outlay.RoomId);
    }
}
