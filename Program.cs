﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace lab7ManyToMany
{
    class Program
    {
        static void List()
        {
            using (var db = new AppDbContext())
            {
                var allStuff = db.Courses.Include(p => p.StudentCourses).ThenInclude(ep => ep.Student);
    
                foreach (var course in allStuff)
                {
                    Console.WriteLine($"{course.CourseName} -");
                    foreach (var student in course.StudentCourses)
                    {
                        Console.WriteLine($"\t{student.Student.FirstName} {student.Student.LastName} {student.StudentGPA}");
                    }
                    Console.WriteLine();
                }
            }
        }
        static void Main(string[] args)
        {
            using (var db = new AppDbContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                List<Student> students = new List<Student>() {
                    new Student {FirstName = "John", LastName = "K"},
                    new Student {FirstName = "Bon", LastName = "Fire"},
                    new Student{FirstName = "Chris", LastName = "Tne"},
                    new Student {FirstName = "Dan", LastName = "iel"},
                };

                List<Course>courses = new List<Course>() {
                    new Course {CourseName = "Project Management"},
                    new Course {CourseName = "Computer Info"}
                };

                List<StudentCourse> joinTable = new List<StudentCourse>() {
                    new StudentCourse {Student = students[0], Course = courses[0],  StudentGPA=3.0},
                    new StudentCourse {Student = students[1], Course = courses[0], StudentGPA = 3.8},
                    new StudentCourse {Student =students[2], Course= courses[0], StudentGPA = 3.4},
                    new StudentCourse {Student =students[3], Course = courses[0],StudentGPA = 2.8},
                    new StudentCourse {Student =students[1], Course = courses[1], StudentGPA =3.8},
                    new StudentCourse {Student = students[3], Course = courses[1], StudentGPA = 2.8},
                };

                db.AddRange(students);
                db.AddRange(courses);
                db.AddRange(joinTable);
                db.SaveChanges();
            }
            List();

            using (var db = new AppDbContext())
            {
            int studentIdToRemove=2;
            int courseIDToRemove=1;

            StudentCourse stToRemove = db.StudentCourses.Find(studentIdToRemove, courseIDToRemove);
            Student s = db.Students.Find(studentIdToRemove);
            db.Remove(stToRemove);
            db.SaveChanges();

            }
           
             using (var db = new AppDbContext())
            {
                Student student5=new Student { FirstName = "James", LastName = "L", };
                //Course c=new Course {CourseName = "Project Management"};
                Course c=db.Courses.Where(u=>u.CourseName=="Project Management").First(); 
                StudentCourse st=new StudentCourse {Student = student5, Course = c,  StudentGPA=3.0};
                
                db.Add(st);
                db.Add(student5);
                db.SaveChanges();
            }
            List();

        }
    }
}
