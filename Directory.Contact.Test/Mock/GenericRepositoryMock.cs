using System;
using System.Collections.Generic;
using System.Linq;
using MicroServices.Core.Entity;
using MicroServices.Infrastructure.Repository;
using Moq;

namespace Directory.Contact.Test.Mock
{
    public abstract class GenericRepositoryMock
    {
        public void SetupMock<TDocument>(Moq.Mock mockRepo, List<TDocument> collection) where TDocument : DocumentEntity
        {
            var mock = mockRepo.As<IRepository<TDocument>>();

            mock.Setup(x => x.Get(null)).Returns(collection.AsQueryable());
            
            mock.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Func<string, TDocument>(
                    x => collection.Find(q => q.Id.Equals(x))
                ));

            
            mock.Setup(x => x.CreateAsync(It.IsAny<TDocument>()))
                .ReturnsAsync(new Func<TDocument, TDocument>(x =>
                {
                    x.Id = new Guid().ToString();
                    collection.Add(x);
                    return collection.Last();
                }));
        }
    }
}