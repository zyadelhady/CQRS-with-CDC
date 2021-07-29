﻿using API.Data;
using Logic.Commands;
using Logic.Entities;
using Logic.utils;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    class EnrollCommandHandler : IRequestHandler<EnrollCommand, Result>
    {
        private readonly DataContext _context;

        public EnrollCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(EnrollCommand request, CancellationToken cancellationToken)
        {
            var student = await _context.Students.FindAsync(request.EnrollDto.StudentId);
            if (student == null) return ResultFactory.Fail("No student with that id.");
            var course = await _context.Courses.FindAsync(request.EnrollDto.CourseId);
            if (course == null) return ResultFactory.Fail("No course with that id.");
            if (!Enum.IsDefined(typeof(Grade), request.EnrollDto.Grade)) return ResultFactory.Fail("Grade is invalid");
            student.Enroll(course, Enum.Parse<Grade>(request.EnrollDto.Grade));
            await _context.SaveAllAsync();
            return ResultFactory.Ok();  
        }
    }
}
