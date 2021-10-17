﻿// <auto-generated />
using ComputerDiscuss.DiscordAdminBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ComputerDiscuss.DiscordAdminBot.Migrations
{
    [DbContext(typeof(BotDBContext))]
    [Migration("20211017144723_SaperatedUserNameField")]
    partial class SaperatedUserNameField
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("ComputerDiscuss.DiscordAdminBot.Models.ConverSession", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Action")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("ChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Context")
                        .HasColumnType("TEXT");

                    b.Property<long>("CreatedTime")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("Lifetime")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("MessageId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ConverSessions");
                });

            modelBuilder.Entity("ComputerDiscuss.DiscordAdminBot.Models.Sticker", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Keyword")
                        .HasColumnType("TEXT");

                    b.Property<string>("URI")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Stickers");
                });
#pragma warning restore 612, 618
        }
    }
}
