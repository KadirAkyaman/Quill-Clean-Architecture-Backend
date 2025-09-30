using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Quill.Application.Interfaces;
using Quill.Domain.Entities;

namespace Quill.Application.Mappings
{
    public class CategoryPostCountResolver : IValueResolver<Category, object, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryPostCountResolver(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public int Resolve(Category source, object destination, int destMember, ResolutionContext context)
        {
            if (source.Posts != null && source.Posts.Any())
            {
                return source.Posts.Count;
            }

            var countTask = _unitOfWork.PostRepository.GetCountByCategoryIdAsync(source.Id, CancellationToken.None);
            
            return countTask.Result;
        }
    }
}