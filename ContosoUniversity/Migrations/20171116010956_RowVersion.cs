using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ContosoUniversity.Migrations
{
    public partial class RowVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<int>(
               name: "CourseID2",
               table: "Course",
               nullable: true);

            migrationBuilder.Sql("update Course set CourseID2 = CourseID");

            migrationBuilder.DropForeignKey(
               name: "FK_CourseAssignment_Course_CourseID"
               , table: "CourseAssignment"
               );

            migrationBuilder.DropForeignKey(
              name: "FK_Enrollment_Course_CourseID"
              , table: "Enrollment"
              );

            migrationBuilder.DropPrimaryKey(
               name: "PK_Course",
               table: "Course"
               );

           

            migrationBuilder.AddColumn<byte[]>(
               name: "RowVersion",
               table: "Department",
               type: "rowversion",
               rowVersion: true,
               nullable: true);

            migrationBuilder.AddForeignKey(
              name: "FK_CourseAssignment_Course_CourseID"
              , table: "CourseAssignment"
              , column: "CourseID"
              , principalTable: "Course"
              );

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Course_CourseID"
                , table: "Enrollment"
                , column: "CourseID"
                , principalTable: "Course"
  );


            //migrationBuilder.RenameColumn(
            //   name: "CourseID2",
            //   table: "Course",
            //   newName: "CourseID"
            //);

            migrationBuilder.AlterColumn<int>(
                name: "CourseID",
                table: "Course",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);




        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Department");

            migrationBuilder.AlterColumn<int>(
                name: "CourseID",
                table: "Course",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        }
    }
}
