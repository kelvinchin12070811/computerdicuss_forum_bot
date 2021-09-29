﻿// <auto-generated />
using ComputerDiscuss.DiscordAdminBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ComputerDiscuss.DiscordAdminBot.Migrations
{
    [DbContext(typeof(BotDBContext))]
    [Migration("20210928145920_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("ComputerDiscuss.DiscordAdminBot.Models.Sticker", b =>
                {
                    b.Property<int>("Id")
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
