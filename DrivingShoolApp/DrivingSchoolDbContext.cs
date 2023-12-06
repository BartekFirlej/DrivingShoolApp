using DrivingSchoolApp.Models;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;


public partial class DrivingSchoolDbContext : DbContext
{
    public DrivingSchoolDbContext()
    {
    }

    public DrivingSchoolDbContext(DbContextOptions<DrivingSchoolDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Classroom> Classrooms { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseSubject> CourseSubjects { get; set; }

    public virtual DbSet<CourseType> CourseTypes { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DrivingLesson> DrivingLessons { get; set; }

    public virtual DbSet<DrivingLicence> DrivingLicences { get; set; }

    public virtual DbSet<Lecture> Lectures { get; set; }

    public virtual DbSet<Lecturer> Lecturers { get; set; }

    public virtual DbSet<LicenceCategory> LicenceCategories { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<RequiredLicenceCategory> RequiredLicenceCategories { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=127.0.0.1;Database=DrivingSchoolDB;User ID=sa;Password=zaq1@WSX;MultipleActiveResultSets=True;Encrypt=False;");
            optionsBuilder.UseExceptionProcessor();
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Address_PK");

            entity.ToTable("Address");

            entity.Property(e => e.Id)
                .HasColumnName("ID");
            entity.Property(e => e.City)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Street)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Classroom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Classroom_PK");

            entity.ToTable("Classroom");

            entity.Property(e => e.Id)
                .HasColumnName("ID");
            entity.Property(e => e.AddressId).HasColumnName("Address_ID");

            entity.HasOne(d => d.Address).WithMany(p => p.Classrooms)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Classroom_Address_FK");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Course_PK");

            entity.ToTable("Course");

            entity.Property(e => e.Id)
                .HasColumnName("ID");
            entity.Property(e => e.BeginDate).HasColumnType("datetime");
            entity.Property(e => e.CourseTypeId).HasColumnName("CourseType_ID");
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.CourseType).WithMany(p => p.Courses)
                .HasForeignKey(d => d.CourseTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Course_CourseType_FK");
        });

        modelBuilder.Entity<CourseSubject>(entity =>
        {
            entity.HasKey(e => new { e.SubjectId, e.CourseId }).HasName("CourseSubjects_PK");

            entity.Property(e => e.SubjectId).HasColumnName("Subject_ID");
            entity.Property(e => e.CourseId).HasColumnName("Course_ID");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseSubjects)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CourseSubjects_Course_FK");

            entity.HasOne(d => d.Subject).WithMany(p => p.CourseSubjects)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CourseSubjects_Subject_FK");
        });

        modelBuilder.Entity<CourseType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CourseType_PK");

            entity.ToTable("CourseType");

            entity.Property(e => e.Id)
                .HasColumnName("ID");
            entity.Property(e => e.LicenceCategoryId).HasColumnName("LicenceCategory_ID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.LicenceCategory).WithMany(p => p.CourseTypes)
                .HasForeignKey(d => d.LicenceCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CourseType_LicenceCategory_FK");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Customer_PK");

            entity.ToTable("Customer");

            entity.Property(e => e.Id)
                .HasColumnName("ID");
            entity.Property(e => e.BirthDate).HasColumnType("date");
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.SecondName)
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasMany(d => d.Lectures).WithMany(p => p.Customers)
                .UsingEntity<Dictionary<string, object>>(
                    "CustomerLecture",
                    r => r.HasOne<Lecture>().WithMany()
                        .HasForeignKey("LectureId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("CustomerLecture_Lecture_FK"),
                    l => l.HasOne<Customer>().WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("CustomerLecture_Customer_FK"),
                    j =>
                    {
                        j.HasKey("CustomerId", "LectureId").HasName("CustomerLecture_PK");
                        j.ToTable("CustomerLecture");
                        j.IndexerProperty<int>("CustomerId").HasColumnName("Customer_ID");
                        j.IndexerProperty<int>("LectureId").HasColumnName("Lecture_ID");
                    });
        });

        modelBuilder.Entity<DrivingLesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DrivingLesson_PK");

            entity.ToTable("DrivingLesson");

            entity.Property(e => e.Id)
                .HasColumnName("ID");
            entity.Property(e => e.AddressId).HasColumnName("Address_ID");
            entity.Property(e => e.LecturerId).HasColumnName("Lecturer_ID");
            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.CourseId).HasColumnName("Course_ID");
            entity.Property(e => e.LessonDate).HasColumnType("datetime");

            entity.HasOne(d => d.Address).WithMany(p => p.DrivingLessons)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DrivingLesson_Address_FK");

            entity.HasOne(d => d.Lecturer).WithMany(p => p.DrivingLessons)
                .HasForeignKey(d => d.LecturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DrivingLesson_Lecturer_FK");

            entity.HasOne(d => d.Customer).WithMany(p => p.DrivingLessons)
            .HasForeignKey(d => d.CustomerId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("DrivingLesson_Customer_FK");

            entity.HasOne(d => d.Course).WithMany(p => p.DrivingLessons)
            .HasForeignKey(d => d.CourseId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("DrivingLesson_Course_FK");
        });

        modelBuilder.Entity<DrivingLicence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DrivingLicence_PK");

            entity.ToTable("DrivingLicence");

            entity.Property(e => e.Id)
                .HasColumnName("ID");
            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.ExpirationDate).HasColumnType("date");
            entity.Property(e => e.LicenceCategoryId).HasColumnName("LicenceCategory_ID");
            entity.Property(e => e.ReceivedDate).HasColumnType("date");

            entity.HasOne(d => d.Customer).WithMany(p => p.DrivingLicences)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DrivingLicence_Customer_FK");

            entity.HasOne(d => d.LicenceCategory).WithMany(p => p.DrivingLicences)
                .HasForeignKey(d => d.LicenceCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DrivingLicence_LicenceCategory_FK");
        });

        modelBuilder.Entity<Lecture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Lecture_PK");

            entity.ToTable("Lecture");

            entity.Property(e => e.Id)
                .HasColumnName("ID");
            entity.Property(e => e.ClassroomId).HasColumnName("Classroom_ID");
            entity.Property(e => e.CourseId).HasColumnName("CourseSubjects_Course_ID");
            entity.Property(e => e.SubjectId).HasColumnName("CourseSubjects_SubjectID");
            entity.Property(e => e.LectureDate).HasColumnType("datetime");
            entity.Property(e => e.LecturerId).HasColumnName("Lecturer_ID");

            entity.HasOne(d => d.Classroom).WithMany(p => p.Lectures)
                .HasForeignKey(d => d.ClassroomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Lecture_Classroom_FK");

            entity.HasOne(d => d.Lecturer).WithMany(p => p.Lectures)
                .HasForeignKey(d => d.LecturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Lecture_Lecturer_FK");

            entity.HasOne(d => d.CourseSubjects).WithMany(p => p.Lectures)
                .HasForeignKey(d => new { d.SubjectId, d.CourseId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Lecture_CourseSubjects_FK");
        });

        modelBuilder.Entity<Lecturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Lecturer_PK");

            entity.ToTable("Lecturer");

            entity.Property(e => e.Id)
                .HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.SecondName)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LicenceCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LicenceCategory_PK");

            entity.ToTable("LicenceCategory");

            entity.Property(e => e.Id)
                .HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => new { e.CustomerId, e.CourseId }).HasName("Registration_PK");

            entity.ToTable("Registration");

            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.CourseId).HasColumnName("Course_ID");
            entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

            entity.HasOne(d => d.Course).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Registration_Course_FK");

            entity.HasOne(d => d.Customer).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Registration_Customer_FK");
        });

        modelBuilder.Entity<RequiredLicenceCategory>(entity =>
        {
            entity.HasKey(e => new { e.LicenceCategoryId, e.RequiredLicenceCategoryId }).HasName("RequiredDrivingLicences_PK");

            entity.ToTable("RequiredLicenceCategory");

            entity.Property(e => e.LicenceCategoryId).HasColumnName("LicenceCategory_ID");
            entity.Property(e => e.RequiredLicenceCategoryId).HasColumnName("RequiredLicenceCategory_ID");
            entity.Property(e => e.RequiredYears).HasColumnName("RequiredYears");

            entity.HasOne(d => d.LicenceCategory).WithMany(p => p.RequiredDrivingLicenceDrivingLicences)
                .HasForeignKey(d => d.LicenceCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RequiredLicenceCategory_LicenceCategory_FK");

            entity.HasOne(d => d.ReqLicenceCategory).WithMany(p => p.RequiredDrivingLicenceRequiredDrivingLicenceNavigations)
                .HasForeignKey(d => d.LicenceCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RequiredLicenceCategory_RequiredLicenceCategory_FK");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Subject_PK");

            entity.ToTable("Subject");

            entity.Property(e => e.Id)
                .HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
