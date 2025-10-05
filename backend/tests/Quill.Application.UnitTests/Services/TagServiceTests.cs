using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Quill.Application.Interfaces;
using Quill.Application.Interfaces.Repositories;
using Quill.Application.Services;
using Xunit;

namespace Quill.Application.UnitTests.Services
{
    public class TagServiceTests
    {
        private readonly Mock<ITagRepository> _tagRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly TagService _tagService;

        public TagServiceTests()
        {
            _tagRepositoryMock = new Mock<ITagRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.TagRepository).Returns(_tagRepositoryMock.Object);

            _tagService = new TagService
            (
                _unitOfWorkMock.Object,
                _mapperMock.Object
            );
        }

        // ---CREATE------------------------------------------------------------------------------------------------------- 



        // ---UPDATE-------------------------------------------------------------------------------------------------------  



        // ---DELETE-------------------------------------------------------------------------------------------------------



    }
}