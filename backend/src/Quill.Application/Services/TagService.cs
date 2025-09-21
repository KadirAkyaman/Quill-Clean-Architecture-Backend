using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using Quill.Application.DTOs.Tag;
using Quill.Application.Exceptions;
using Quill.Application.Interfaces;
using Quill.Application.Interfaces.Services;
using Quill.Domain.Entities;

namespace Quill.Application.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TagService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TagDto> CreateAsync(TagCreateDto tagCreateDto, CancellationToken cancellationToken)
        {
            var tag = _mapper.Map<Tag>(tagCreateDto);

            await _unitOfWork.TagRepository.AddAsync(tag, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TagDto>(tag);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var tag = await GetTagAndEnsureExists(id, cancellationToken);

            _unitOfWork.TagRepository.Remove(tag);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<TagDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var tags = await _unitOfWork.TagRepository.GetAllAsync(cancellationToken);

            return _mapper.Map<IReadOnlyList<TagDto>>(tags);
        }

        public async Task<TagDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var tag = await _unitOfWork.TagRepository.GetByIdAsync(id, cancellationToken);

            return _mapper.Map<TagDto>(tag);
        }

        public async Task<TagDto?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            var tag = await _unitOfWork.TagRepository.GetByNameAsync(name, cancellationToken);

            return _mapper.Map<TagDto>(tag);
        }

        public async Task UpdateAsync(int id, TagUpdateDto tagUpdateDto, CancellationToken cancellationToken)
        {
            var tag = await GetTagAndEnsureExists(id, cancellationToken);

            _mapper.Map(tagUpdateDto, tag);

            _unitOfWork.TagRepository.Update(tag); 
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }


        //HELPER
        private async Task<Tag> GetTagAndEnsureExists(int id, CancellationToken cancellationToken)
        {
            var tag = await _unitOfWork.TagRepository.GetByIdAsync(id, cancellationToken);

            if (tag is null)
                throw new NotFoundException($"Tag with ID {id} not found.");

            return tag;
        }
    }
}