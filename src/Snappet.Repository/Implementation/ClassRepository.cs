﻿using Snappet.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Snappet.Model;
using Snappet.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Snappet.Repository.Implementation.Base;

namespace Snappet.Repository.Implementation
{
    public class ClassRepository : BasicRepository<Class>, IClassRepository
    {
        public ClassRepository(SnappetContext SnappetContext)
            : base(SnappetContext, SnappetContext.Classes)
        {

        }

        public async Task<List<Model.DTO.ProgressPerUser>> GetCurrentActivity(int classID)
        {
            var progress = 
                from answer in SnappetContext.Answers
                join user in SnappetContext.Users on answer.UserId equals user.ID
                where answer.ClassId == classID
                group answer by new { Name = answer.User.Name, ID = answer.User.ID } into grp
                select new Model.DTO.ProgressPerUser
                {
                    UserID = grp.Key.ID,
                    UserName = grp.Key.Name,
                    AverageProgress = grp.Average(a => a.Progress)
                };

            return await progress.ToListAsync();
        }
    }
}
