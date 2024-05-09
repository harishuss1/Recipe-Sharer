﻿// <auto-generated />
using System;
using Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;

#nullable disable

namespace RecipeSharer.Migrations
{
    [DbContext(typeof(RecipeSharerContext))]
    partial class RecipeSharerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            OracleModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RecipeTag", b =>
                {
                    b.Property<int>("TaggedRecipesRecipeId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("TagsTagId")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("TaggedRecipesRecipeId", "TagsTagId");

                    b.HasIndex("TagsTagId");

                    b.ToTable("RecipeTag");
                });

            modelBuilder.Entity("Recipes.Ingredient", b =>
                {
                    b.Property<int>("IngredientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IngredientId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<double>("Quantity")
                        .HasColumnType("BINARY_DOUBLE");

                    b.Property<int>("RecipeId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("UnitOfMass")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("IngredientId");

                    b.HasIndex("RecipeId");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("Recipes.Rating", b =>
                {
                    b.Property<int>("RatingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RatingId"));

                    b.Property<int>("RecipeId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("Score")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("UserId")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("RatingId");

                    b.HasIndex("RecipeId");

                    b.HasIndex("UserId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("Recipes.Recipe", b =>
                {
                    b.Property<int>("RecipeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RecipeId"));

                    b.Property<TimeSpan>("CookingTime")
                        .HasColumnType("INTERVAL DAY(8) TO SECOND(7)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("OwnerId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<TimeSpan>("PreparationTime")
                        .HasColumnType("INTERVAL DAY(8) TO SECOND(7)");

                    b.Property<int>("Servings")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("RecipeId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("Recipes.Step", b =>
                {
                    b.Property<int>("StepId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StepId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("Number")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("RecipeId")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("StepId");

                    b.HasIndex("RecipeId");

                    b.ToTable("Steps");
                });

            modelBuilder.Entity("Recipes.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TagId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Users.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Description")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("RAW(2000)");

                    b.Property<byte[]>("ProfilePicture")
                        .HasColumnType("RAW(2000)");

                    b.Property<int?>("RecipeId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("RAW(2000)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("UserId");

                    b.HasIndex("RecipeId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RecipeTag", b =>
                {
                    b.HasOne("Recipes.Recipe", null)
                        .WithMany()
                        .HasForeignKey("TaggedRecipesRecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Recipes.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Recipes.Ingredient", b =>
                {
                    b.HasOne("Recipes.Recipe", "Recipe")
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Recipes.Rating", b =>
                {
                    b.HasOne("Recipes.Recipe", "Recipe")
                        .WithMany("Ratings")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Recipes.Recipe", b =>
                {
                    b.HasOne("Users.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Recipes.Step", b =>
                {
                    b.HasOne("Recipes.Recipe", "Recipe")
                        .WithMany("Steps")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Users.User", b =>
                {
                    b.HasOne("Recipes.Recipe", null)
                        .WithMany("FavoritedBy")
                        .HasForeignKey("RecipeId");
                });

            modelBuilder.Entity("Recipes.Recipe", b =>
                {
                    b.Navigation("FavoritedBy");

                    b.Navigation("Ingredients");

                    b.Navigation("Ratings");

                    b.Navigation("Steps");
                });
#pragma warning restore 612, 618
        }
    }
}
