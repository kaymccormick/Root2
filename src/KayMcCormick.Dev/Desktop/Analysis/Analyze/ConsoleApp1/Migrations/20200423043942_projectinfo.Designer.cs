﻿// <auto-generated />
using System;
using ConsoleAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ConsoleAnalysis.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200423043942_projectinfo")]
    partial class Projectinfo
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AnalysisAppLib.Project.ProjectInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Platform")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RepositoryUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SolutionPath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("AnalysisAppLib.Syntax.AppAssembly", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("Id");

                    b.ToTable("AppAssembly");
                });

            modelBuilder.Entity("AnalysisAppLib.Syntax.AppClrType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AssemblyId")
                        .HasColumnType("int");

                    b.Property<string>("AssemblyQualifiedName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("BaseTypeId")
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAbstract")
                        .HasColumnType("bit");

                    b.Property<bool>("IsClass")
                        .HasColumnType("bit");

                    b.Property<bool>("IsConstructedGenericType")
                        .HasColumnType("bit");

                    b.Property<bool>("IsGenericType")
                        .HasColumnType("bit");

                    b.Property<bool>("IsGenericTypeDefinition")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("AssemblyId");

                    b.HasIndex("BaseTypeId");

                    b.ToTable("AppClrType");
                });

            modelBuilder.Entity("AnalysisAppLib.Syntax.AppMethodInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AppTypeInfoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AppTypeInfoId");

                    b.ToTable("AppMethodInfo");
                });

            modelBuilder.Entity("AnalysisAppLib.Syntax.AppParameterInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AppMethodInfoId")
                        .HasColumnType("int");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<bool>("IsOptional")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AppMethodInfoId");

                    b.ToTable("AppParameterInfo");
                });

            modelBuilder.Entity("AnalysisAppLib.Syntax.AppTypeInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AppClrTypeId")
                        .HasColumnType("int");

                    b.Property<long?>("ColorValue")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ElementName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HierarchyLevel")
                        .HasColumnType("int");

                    b.Property<int?>("ParentInfoId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AppClrTypeId");

                    b.HasIndex("ParentInfoId");

                    b.ToTable("AppTypeInfos");
                });

            modelBuilder.Entity("AnalysisAppLib.Syntax.SyntaxFieldInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AppTypeInfoId")
                        .HasColumnType("int");

                    b.Property<string>("ClrTypeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ElementTypeMetadataName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCollection")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Optional")
                        .HasColumnType("bit");

                    b.Property<bool>("Override")
                        .HasColumnType("bit");

                    b.Property<string>("TypeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AppTypeInfoId");

                    b.ToTable("SyntaxFieldInfo");
                });

            modelBuilder.Entity("AnalysisAppLib.Syntax.AppClrType", b =>
                {
                    b.HasOne("AnalysisAppLib.Syntax.AppAssembly", "Assembly")
                        .WithMany()
                        .HasForeignKey("AssemblyId");

                    b.HasOne("AnalysisAppLib.Syntax.AppClrType", "BaseType")
                        .WithMany()
                        .HasForeignKey("BaseTypeId");
                });

            modelBuilder.Entity("AnalysisAppLib.Syntax.AppMethodInfo", b =>
                {
                    b.HasOne("AnalysisAppLib.Syntax.AppTypeInfo", null)
                        .WithMany("FactoryMethods")
                        .HasForeignKey("AppTypeInfoId");
                });

            modelBuilder.Entity("AnalysisAppLib.Syntax.AppParameterInfo", b =>
                {
                    b.HasOne("AnalysisAppLib.Syntax.AppMethodInfo", null)
                        .WithMany("Parameters")
                        .HasForeignKey("AppMethodInfoId");
                });

            modelBuilder.Entity("AnalysisAppLib.Syntax.AppTypeInfo", b =>
                {
                    b.HasOne("AnalysisAppLib.Syntax.AppClrType", "AppClrType")
                        .WithMany()
                        .HasForeignKey("AppClrTypeId");

                    b.HasOne("AnalysisAppLib.Syntax.AppTypeInfo", "ParentInfo")
                        .WithMany("SubTypeInfos")
                        .HasForeignKey("ParentInfoId");
                });

            modelBuilder.Entity("AnalysisAppLib.Syntax.SyntaxFieldInfo", b =>
                {
                    b.HasOne("AnalysisAppLib.Syntax.AppTypeInfo", null)
                        .WithMany("Fields")
                        .HasForeignKey("AppTypeInfoId");
                });
#pragma warning restore 612, 618
        }
    }
}
