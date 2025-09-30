using AutoMapper;
using Quill.Application.Interfaces;
using Quill.Domain.Entities;

namespace Quill.Application.Mappings
{
    public class TagPostCountResolver : IValueResolver<Tag, object, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagPostCountResolver(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Resolve(Tag source, object destination, int destMember, ResolutionContext context)
        {
            if (source.Posts != null && source.Posts.Any())
            {
                return source.Posts.Count;
            }

            var countTask = _unitOfWork.TagRepository.GetPostCountAsync(source.Id, CancellationToken.None);
            
            return countTask.Result;
        }
    }
}