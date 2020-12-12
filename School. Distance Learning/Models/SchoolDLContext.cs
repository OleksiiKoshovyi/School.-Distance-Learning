using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace School._Distance_Learning.Models
{
    public partial class SchoolDLContext : DbContext
    {
        public SchoolDLContext()
        {
        }

        public SchoolDLContext(DbContextOptions<SchoolDLContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admins> Admins { get; set; }
        public virtual DbSet<GradeSubject> GradeSubject { get; set; }
        public virtual DbSet<Grades> Grades { get; set; }
        public virtual DbSet<GradesInfo> GradesInfo { get; set; }
        public virtual DbSet<GroupPupil> GroupPupil { get; set; }
        public virtual DbSet<GroupTypeSubject> GroupTypeSubject { get; set; }
        public virtual DbSet<GroupTypes> GroupTypes { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<Holidays> Holidays { get; set; }
        public virtual DbSet<Homeworks> Homeworks { get; set; }
        public virtual DbSet<Pupils> Pupils { get; set; }
        public virtual DbSet<SkippingClasses> SkippingClasses { get; set; }
        public virtual DbSet<Subjects> Subjects { get; set; }
        public virtual DbSet<TeacherSubject> TeacherSubject { get; set; }
        public virtual DbSet<TeacherSubjectGroup> TeacherSubjectGroup { get; set; }
        public virtual DbSet<Teachers> Teachers { get; set; }
        public virtual DbSet<TeachersInfo> TeachersInfo { get; set; }
        public virtual DbSet<Timetables> Timetables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admins>(entity =>
            {
                entity.HasKey(e => e.AdminId)
                    .HasName("PK__Admins__719FE48897B06BA6");

                entity.HasIndex(e => e.Login)
                    .HasName("UQ__Admins__5E55825B603EDF50")
                    .IsUnique();

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GradeSubject>(entity =>
            {
                entity.HasIndex(e => new { e.GradeId, e.SubjectId })
                    .HasName("UQ__GradeSub__CE39C06C2DC93FCE")
                    .IsUnique();

                entity.HasOne(d => d.Grade)
                    .WithMany(p => p.GradeSubject)
                    .HasForeignKey(d => d.GradeId)
                    .HasConstraintName("FK__GradeSubj__Grade__4F7CD00D");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.GradeSubject)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK__GradeSubj__Subje__5070F446");
            });

            modelBuilder.Entity<Grades>(entity =>
            {
                entity.HasKey(e => e.GradeId)
                    .HasName("PK__Grades__54F87A576FD4A9A0");

                entity.HasIndex(e => new { e.FirstYear, e.Letter })
                    .HasName("UQ__Grades__3152B92BB12C5D1E")
                    .IsUnique();

                entity.Property(e => e.Letter)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GradesInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("GradesInfo");

                entity.Property(e => e.GradeId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<GroupPupil>(entity =>
            {
                entity.HasIndex(e => new { e.GroupId, e.PupilId })
                    .HasName("UQ__GroupPup__B661272EF6E7A586")
                    .IsUnique();

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupPupil)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK__GroupPupi__Group__4AB81AF0");

                entity.HasOne(d => d.Pupil)
                    .WithMany(p => p.GroupPupil)
                    .HasForeignKey(d => d.PupilId)
                    .HasConstraintName("FK__GroupPupi__Pupil__4BAC3F29");
            });

            modelBuilder.Entity<GroupTypeSubject>(entity =>
            {
                entity.HasIndex(e => new { e.GroupTypeId, e.SubjectId })
                    .HasName("UQ__GroupTyp__88D8E096811D6EF9")
                    .IsUnique();

                entity.HasOne(d => d.GroupType)
                    .WithMany(p => p.GroupTypeSubject)
                    .HasForeignKey(d => d.GroupTypeId)
                    .HasConstraintName("FK__GroupType__Group__4222D4EF");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.GroupTypeSubject)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK__GroupType__Subje__4316F928");
            });

            modelBuilder.Entity<GroupTypes>(entity =>
            {
                entity.HasKey(e => e.GroupTypeId)
                    .HasName("PK__GroupTyp__12195AADB20005A8");

                entity.Property(e => e.GroupTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Groups>(entity =>
            {
                entity.HasKey(e => e.GroupId)
                    .HasName("PK__Groups__149AF36A2132D546");

                entity.HasOne(d => d.Grade)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.GradeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Groups__GradeId__46E78A0C");

                entity.HasOne(d => d.GroupType)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.GroupTypeId)
                    .HasConstraintName("FK__Groups__GroupTyp__45F365D3");
            });

            modelBuilder.Entity<Holidays>(entity =>
            {
                entity.HasKey(e => e.HolidayId)
                    .HasName("PK__Holidays__2D35D57A467E3C58");

                entity.Property(e => e.HolidayName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<Homeworks>(entity =>
            {
                entity.HasKey(e => e.HomeworkId)
                    .HasName("PK__Homework__FDE46A72023B2A12");

                entity.HasIndex(e => new { e.PassDate, e.TeacherSubjectGroupId })
                    .HasName("UQ__Homework__4170E12CA8C0EBE4")
                    .IsUnique();

                entity.Property(e => e.PassDate).HasColumnType("date");

                entity.Property(e => e.Homework)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.TeacherSubjectGroup)
                    .WithMany(p => p.Homeworks)
                    .HasForeignKey(d => d.TeacherSubjectGroupId)
                    .HasConstraintName("FK__Homeworks__Teach__17036CC0");
            });

            modelBuilder.Entity<Pupils>(entity =>
            {
                entity.HasKey(e => e.PupilId)
                    .HasName("PK__Pupils__2FBD445BC288FA21");

                entity.HasIndex(e => e.Login)
                    .HasName("UQ__Pupils__5E55825BC798B48C")
                    .IsUnique();

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Patronymic)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.SurName)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.HasOne(d => d.Grade)
                    .WithMany(p => p.Pupils)
                    .HasForeignKey(d => d.GradeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Pupils__GradeId__32E0915F");
            });

            modelBuilder.Entity<SkippingClasses>(entity =>
            {
                entity.HasKey(e => e.SkippingClassId)
                    .HasName("PK__Skipping__B39D31681FE0B8EE");

                entity.HasIndex(e => new { e.PupilId, e.SkippingDate })
                    .HasName("UQ__Skipping__D5D0D57F460C9ABB")
                    .IsUnique();

                entity.HasOne(d => d.Pupil)
                    .WithMany(p => p.SkippingClasses)
                    .HasForeignKey(d => d.PupilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SkippingC__Pupil__1AD3FDA4");
            });

            modelBuilder.Entity<Subjects>(entity =>
            {
                entity.HasKey(e => e.SubjectId)
                    .HasName("PK__Subjects__AC1BA3A80162DB2F");

                entity.Property(e => e.SubjectName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TeacherSubject>(entity =>
            {
                entity.HasIndex(e => new { e.TeacherId, e.SubjectId })
                    .HasName("UQ__TeacherS__7733E35F2B254D6D")
                    .IsUnique();

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.TeacherSubject)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK__TeacherSu__Subje__5629CD9C");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.TeacherSubject)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("FK__TeacherSu__Teach__5535A963");
            });

            modelBuilder.Entity<TeacherSubjectGroup>(entity =>
            {
                entity.HasIndex(e => new { e.TeacherSubjectId, e.GroupId })
                    .HasName("UQ__TeacherS__4A040B71A8FE9FF1")
                    .IsUnique();

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.TeacherSubjectGroup)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK__TeacherSu__Group__5BE2A6F2");

                entity.HasOne(d => d.TeacherSubject)
                    .WithMany(p => p.TeacherSubjectGroup)
                    .HasForeignKey(d => d.TeacherSubjectId)
                    .HasConstraintName("FK__TeacherSu__Teach__5AEE82B9");
            });

            modelBuilder.Entity<Teachers>(entity =>
            {
                entity.HasKey(e => e.TeacherId)
                    .HasName("PK__Teachers__EDF2596491304414");

                entity.HasIndex(e => e.Login)
                    .HasName("UQ__Teachers__5E55825B674299F1")
                    .IsUnique();

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Patronymic)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.RecruitmentDate).HasColumnType("date");

                entity.Property(e => e.SurName)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TeachersInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("TeachersInfo");

                entity.Property(e => e.TeacherId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Timetables>(entity =>
            {
                entity.HasKey(e => e.TimetableId)
                    .HasName("PK__Timetabl__68413F605B1B032D");

                entity.HasOne(d => d.TeacherSubjectGroup)
                    .WithMany(p => p.Timetables)
                    .HasForeignKey(d => d.TeacherSubjectGroupId)
                    .HasConstraintName("FK__Timetable__Teach__619B8048");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
